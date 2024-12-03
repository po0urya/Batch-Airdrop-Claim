using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAirdropClaim
{
    public class AirdropService
    {
        private readonly string _rpcUrl;
        private readonly string _airdropContractAddress;
        private readonly string _airdropContractAbi;
        private readonly string _functionName;
        private readonly string _functionData;
        private readonly Dictionary<string, string> _wallets; // Key: Address, Value: PrivateKey

        public AirdropService(string rpcUrl, string airdropContractAddress, string airdropContractAbi, string functionName, string functionData, string csvFilePath)
        {
            _rpcUrl = rpcUrl;
            _airdropContractAddress = airdropContractAddress;
            _airdropContractAbi = airdropContractAbi;
            _functionName = functionName;
            _functionData = functionData;
            _wallets = LoadWalletsFromCsv(csvFilePath);
        }

        private Dictionary<string, string> LoadWalletsFromCsv(string csvFilePath)
        {
            var wallets = new Dictionary<string, string>();
            var lines = File.ReadAllLines(csvFilePath);
            foreach (var line in lines.Skip(1)) // Skip header
            {
                var parts = line.Split(',');
                var address = parts[1].Trim();
                var privateKey = parts[2].Trim();
                wallets[address] = privateKey;
            }
            return wallets;
        }

        public async Task<string> ExecuteAirdropAsync()
        {
            var successCount = 0;
            var failedCount = 0;

            foreach (var wallet in _wallets)
            {
                var address = wallet.Key;
                var privateKey = wallet.Value;

                if (await ExecuteAirdropForWalletAsync(address, privateKey))
                {
                    successCount++;
                }
                else
                {
                    failedCount++;
                }
            }

            return $"Airdrop execution completed. Success: {successCount}, Failed: {failedCount}";
        }

        private async Task<bool> ExecuteAirdropForWalletAsync(string address, string privateKey)
        {
            try
            {
                var web3 = new Web3(new Account(privateKey), _rpcUrl);
                var contract = web3.Eth.GetContract(_airdropContractAbi, _airdropContractAddress);
                var function = contract.GetFunction(_functionName);

                var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
                var gasLimit = await function.EstimateGasAsync(address, null, null, _functionData);

                var transactionInput = function.CreateTransactionInput(address, gasPrice, gasLimit, _functionData);
                var signedTransaction = await web3.Eth.TransactionManager.SignTransactionAsync(transactionInput);

                var transactionHash = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransaction);

                return true;
            }
            catch (RpcResponseException ex)
            {
                // Handle exception
                Console.WriteLine($"Error executing airdrop for wallet {address}: {ex.Message}");
                return false;
            }
        }
    }

}