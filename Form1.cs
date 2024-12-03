using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchAirdropClaim
{
    public partial class Form1 : Form
    {
        private List<WalletInfo> walletInfos;
        private readonly TokenTransferService tokenTransferService;
        private readonly TokenCheckAndTransferService tokenCheckAndTransferService;
        private readonly AirdropService airdropService;
        private readonly AppSettings appSettings;

        public Form1()
        {
            InitializeComponent();

            try
            {
                appSettings = ConfigHelper.LoadAppSettings();
                if (appSettings == null)
                {
                    MessageBox.Show("appSettings could not be loaded.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appSettings: {ex.Message}");
                return;
            }

            tokenTransferService = new TokenTransferService(appSettings.RpcUrl, appSettings.TargetWalletAddress, appSettings.TargetWalletPrivateKey);
            tokenCheckAndTransferService = new TokenCheckAndTransferService(appSettings.RpcUrl, appSettings.TokenContractAddress, appSettings.TokenContractAbi, appSettings.ImportFileName);
            airdropService = new AirdropService(appSettings.RpcUrl, appSettings.AirdropContractAddress, appSettings.AirdropContractAbi, appSettings.AirdropFunctionName, appSettings.AirdropFunctionData, appSettings.ImportFileName);
        }
        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (appSettings == null)
            {
                MessageBox.Show("appSettings could not be loaded.");
                return;
            }

            _ = MonitorWalletsAsync();
        }
        private void btnLoadFile_Click(object sender, System.EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    if (filePath.EndsWith(".csv"))
                    {
                        walletInfos = FileHelper.ImportCsv(filePath);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        walletInfos = FileHelper.ImportExcel(filePath);
                    }

                    dataGridView1.DataSource = walletInfos;
                }
            }
        }

        private async void btnCheckAndTransferTokens_Click(object sender, System.EventArgs e)
        {
            if (walletInfos == null || walletInfos.Count == 0)
            {
                MessageBox.Show("No wallet information available.");
                return;
            }

            foreach (var wallet in walletInfos)
            {
                var balance = await tokenCheckAndTransferService.GetTokenBalanceAsync(wallet.Address);
                if (balance > 0)
                {
                    try
                    {
                        await tokenCheckAndTransferService.TransferTokenAsync(wallet.Address, appSettings.TargetWalletAddress, balance);
                    }
                    catch (RpcResponseException ex)
                    {
                        if (ex.Message.Contains("insufficient funds for gas"))
                        {
                            await HandleInsufficientGasAsync(wallet.Address, balance);
                        }
                        else
                        {
                            MessageBox.Show($"An error occurred while transferring tokens for address {wallet.Address}: {ex.Message}");
                            Clipboard.SetText(ex.Message);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while transferring tokens for address {wallet.Address}: {ex.Message}");
                        Clipboard.SetText(ex.Message);

                    }
                }
            }
        }

        private async Task HandleInsufficientGasAsync(string fromAddress, decimal amount)
        {
            try
            {
                // Fetch target wallet private key
                string targetWalletPrivateKey = appSettings.TargetWalletPrivateKey;

                // Create account for the target wallet
                var targetAccount = new Account(targetWalletPrivateKey);

                // Get gas price from the error message and increase by 25%
                var gasPrice = await tokenCheckAndTransferService.GetGasPriceAsync();
                //gasPrice *= 1.25m; // Dönüşüm yapılmalı

                // Calculate gas amount needed for the transaction
                var gasAmount = await tokenCheckAndTransferService.EstimateGasAsync(appSettings.TargetWalletAddress, fromAddress);

                //var gasPriceHex = new HexBigInteger((gasPrice * 1000000000).ToString()); // Convert to Wei
                var transactionInput = new TransactionInput
                {
                    From = appSettings.TargetWalletAddress,
                    To = fromAddress,
                    Value = new HexBigInteger(Web3.Convert.ToWei(gasPrice * gasAmount, UnitConversion.EthUnit.Wei)),
                };
                transactionInput.Gas = new HexBigInteger(gasAmount);


                // Send transaction to target wallet
                var web3 = new Web3(targetAccount, appSettings.RpcUrl);
                var transactionHash = await web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);

                // Retry the main transaction after successfully handling insufficient gas
                await tokenCheckAndTransferService.TransferTokenAsync(fromAddress, appSettings.TargetWalletAddress, amount);
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.Message);

                MessageBox.Show($"An error occurred while handling insufficient gas for address {fromAddress}: {ex.Message}");

            }
        }

        private async void btnExecuteAirdrop_Click(object sender, System.EventArgs e)
        {
            await airdropService.ExecuteAirdropAsync();
        }


        private async Task MonitorWalletsAsync()
        {
            while (true)
            {
                if (walletInfos == null || walletInfos.Count == 0)
                {
                    await Task.Delay(500); // .5 second delay
                    continue;
                }

                foreach (var wallet in walletInfos)
                {
                    var balance = await tokenCheckAndTransferService.GetTokenBalanceAsync(wallet.Address);
                    if (balance > 0)
                    {
                        await tokenCheckAndTransferService.TransferTokenAsync(wallet.Address, appSettings.TargetWalletAddress, balance);
                    }

                    await Task.Delay(500); // .5 second delay
                }
            }
        }
    }
}