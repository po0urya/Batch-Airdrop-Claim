using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BatchAirdropClaim
{
    public class TokenCheckAndTransferService
    {
        private readonly string _rpcUrl;
        private readonly string _tokenContractAddress;
        private readonly string _tokenContractAbi;
        private readonly Dictionary<string, string> _wallets; // Key: Address, Value: PrivateKey

        public TokenCheckAndTransferService(string rpcUrl, string tokenContractAddress, string tokenContractAbi, string csvFilePath)
        {
            _rpcUrl = rpcUrl;
            _tokenContractAddress = tokenContractAddress;
            _tokenContractAbi = tokenContractAbi;
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

        public async Task<decimal> GetTokenBalanceAsync(string walletAddress)
        {
            var web3 = new Web3(_rpcUrl);
            var contract = web3.Eth.GetContract(_tokenContractAbi, _tokenContractAddress);
            var balanceOfFunction = contract.GetFunction("balanceOf");
            var balance = await balanceOfFunction.CallAsync<BigInteger>(walletAddress);
            return Web3.Convert.FromWei(balance);
        }
        public async Task<BigInteger> GetGasPriceAsync()
        {
            var web3 = new Web3(_rpcUrl);
            // Use Nethereum to get gas price
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();

            // Return gas price
            return gasPrice.Value;
        }

        public async Task<Nethereum.Hex.HexTypes.HexBigInteger> EstimateGasAsync(string fromAddress, string toAddress)
        {
            // Use Nethereum to estimate gas
            var web3 = new Web3(_rpcUrl);

            var contract = web3.Eth.GetContract(_tokenContractAbi, _tokenContractAddress);
            var transferFunction = contract.GetFunction("transfer");
            var gas = await transferFunction.EstimateGasAsync(fromAddress, null, null, toAddress, Web3.Convert.ToWei(1));
            return gas;
        }

        [Function("transfer", "bool")]
        public class TransferFunction : FunctionMessage
        {
            [Parameter("address", "recipient", 1)]
            public string To { get; set; }

            [Parameter("uint256", "amount", 2)]
            public BigInteger TokenAmount { get; set; }
        }
        public async Task<string> TransferTokenAsync(string fromAddress, string toAddress, decimal amount)
        {
            if (!_wallets.ContainsKey(fromAddress))
            {
                throw new Exception($"Wallet with address {fromAddress} not found in the CSV file.");
            }

            var privateKey = _wallets[fromAddress];

            var web3 = new Web3(new Account(privateKey), _rpcUrl);
            var contract = web3.Eth.GetContract(_tokenContractAbi, _tokenContractAddress);
            var decimalsFunction = contract.GetFunction("decimals");
            var decimalCount = await decimalsFunction.CallAsync<int>();
            var amountWithDecimals = amount * (decimal)Math.Pow(10, decimalCount);

            var transferFunction = contract.GetFunction("transfer");

            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            var gasLimit = await transferFunction.EstimateGasAsync(fromAddress, null, null, toAddress, amountWithDecimals);

            var amountWei = new Nethereum.Hex.HexTypes.HexBigInteger((BigInteger)amountWithDecimals);

            // Get the nonce
            var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(fromAddress);

            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();

            var transfer = new TransferFunction()
            {
                To = toAddress,
                TokenAmount = amountWei
            };

            var estimate = await transferHandler.EstimateGasAsync(_tokenContractAddress, transfer);
            transfer.Gas = estimate.Value;

            var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(_tokenContractAddress, transfer);


            return transactionReceipt.TransactionHash;
        }
    }
}