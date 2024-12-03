namespace BatchAirdropClaim
{
    public class AppSettings
    {
        public string ImportFileName { get; set; }
        public string RpcUrl { get; set; }
        public string TokenContractAddress { get; set; }
        public string TokenContractAbi { get; set; }
        public string AirdropContractAddress { get; set; }
        public string AirdropContractAbi { get; set; }
        public string AirdropFunctionName { get; set; }
        public string AirdropFunctionData { get; set; }
        public string TargetWalletAddress { get; set; }
        public string TargetWalletPrivateKey { get; set; }
        public double GasPriceMultiplier { get; set; }
        public double GasLimitMultiplier { get; set; }
        public bool CheckAirdrop { get; set; }
    }
}