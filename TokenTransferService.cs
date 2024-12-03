using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Threading.Tasks;

namespace BatchAirdropClaim
{
    public class TokenTransferService
    {
        private readonly Web3 _web3;
        private readonly string _targetWalletAddress;
        private readonly string _targetWalletPrivateKey;

        public TokenTransferService(string rpcUrl, string targetWalletAddress, string targetWalletPrivateKey)
        {
            _web3 = new Web3(new Account(targetWalletPrivateKey), rpcUrl);
            _targetWalletAddress = targetWalletAddress;
            _targetWalletPrivateKey = targetWalletPrivateKey;
        }

        public async Task<decimal> GetBalanceAsync(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return Web3.Convert.FromWei(balance.Value);
        }

        public async Task<string> TransferAsync(string fromAddress, string toAddress, decimal amount)
        {
            var transaction = await _web3.Eth.GetEtherTransferService()
                                             .TransferEtherAndWaitForReceiptAsync(toAddress, amount);
            return transaction.TransactionHash;
        }

        public async Task EnsureSufficientGasAsync(string address, decimal threshold = 0.01m)
        {
            var balance = await GetBalanceAsync(address);
            if (balance < threshold)
            {
                var amountToTransfer = threshold - balance;
                await TransferAsync(_targetWalletAddress, address, amountToTransfer);
            }
        }
    }
}