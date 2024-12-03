# Batch-Airdrop-Claim

![Batch-Airdrop-Claim](https://github.com/po0urya/Batch-Airdrop-Claim/blob/main/ss.jpg)


Batch Airdrop Claim is a versatile tool designed to automate the process of claiming and optionally transferring airdrop tokens for multiple wallets on any blockchain network. This tool uses a CSV file containing wallet details and interacts with smart contracts to execute claims efficiently.

---

## Features

- **Multi-Wallet Support**: Handle airdrop claims for multiple wallets simultaneously using a CSV file.
- **Customizable RPC Connection**: Easily connect to any blockchain network using the specified RPC URL.
- **Dynamic Smart Contract Interaction**: Works with any airdrop contract and token contract by specifying their ABIs and addresses.
- **Gas Management**: Adjust gas price and gas limit dynamically for optimized performance.
- **Token Transfer**: After claiming tokens, optionally transfer them to a target wallet.
- **Configurable Execution**: Toggle features like gas adjustments, claim checks, and more through a configuration file.

---

## Requirements

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- A valid RPC URL for the target blockchain network.
- ABI and address of the airdrop contract and token contract.
- A CSV file (`airdrop.csv`) containing the wallet addresses for claims.

---

## Configuration

The tool is configured via an `appsettings.json` file. Below is an explanation of the configuration options:

### Configuration Parameters

```json
{
  "ImportFileName": "airdrop.csv",
  "RpcUrl": "rpcurl",
  "TokenContractAddress": "0xaddress",
  "TokenContractAbi": "[Token ABI JSON]",
  "AirdropContractAddress": "0xYourAirdropContractAddress",
  "AirdropContractAbi": "[Airdrop Contract ABI JSON]",
  "AirdropFunctionName": "functionnamehere",
  "AirdropFunctionData": "data",
  "TargetWalletAddress": "0xaddress",
  "TargetWalletPrivateKey": "0xprivatekey",
  "GasPriceMultiplier": 1.1,
  "GasLimitMultiplier": 1.1,
  "CheckAirdrop": false
}
```

### Key Settings

- **`ImportFileName`**: Name of the CSV file containing wallet addresses.
- **`RpcUrl`**: RPC endpoint URL for the blockchain network.
- **`TokenContractAddress`**: Address of the token contract for transferring tokens.
- **`TokenContractAbi`**: ABI of the token contract for interacting with its methods.
- **`AirdropContractAddress`**: Address of the airdrop contract.
- **`AirdropContractAbi`**: ABI of the airdrop contract.
- **`AirdropFunctionName`**: Function name in the airdrop contract to call for claiming tokens.
- **`TargetWalletAddress`**: Target wallet to transfer claimed tokens (optional).
- **`TargetWalletPrivateKey`**: Private key of the target wallet (for signing transactions).
- **`GasPriceMultiplier`**: Multiplier for gas price to handle network congestion.
- **`GasLimitMultiplier`**: Multiplier for gas limit.
- **`CheckAirdrop`**: Set to `true` to check if a wallet is eligible for the airdrop before claiming.

---

## How to Use

1. **Prepare the CSV File**  
   Create a CSV file (`airdrop.csv`) containing wallet addresses for the airdrop. Each row should represent a wallet.

2. **Set Configuration**  
   Edit the `appsettings.json` file to provide:
   - RPC URL.
   - Airdrop and token contract details.
   - Target wallet details (optional).

3. **Run the Application**  

4. **Monitor Execution**  
   The tool will read wallet addresses from the CSV, interact with the specified airdrop contract, and claim tokens for each wallet.

---

## Example `appsettings.json`

```json
{
  "ImportFileName": "airdrop.csv",
  "RpcUrl": "https://mainnet.infura.io/v3/your-project-id",
  "TokenContractAddress": "0x6b175474e89094c44da98b954eedeac495271d0f",
  "TokenContractAbi": "[Token Contract ABI]",
  "AirdropContractAddress": "0x1234567890abcdef1234567890abcdef12345678",
  "AirdropContractAbi": "[Airdrop Contract ABI]",
  "AirdropFunctionName": "claimTokens",
  "AirdropFunctionData": "",
  "TargetWalletAddress": "0xabcdefabcdefabcdefabcdefabcdefabcdefabcd",
  "TargetWalletPrivateKey": "your-private-key-here",
  "GasPriceMultiplier": 1.5,
  "GasLimitMultiplier": 1.2,
  "CheckAirdrop": true
}
```

---

## Example `airdrop.csv`

```
No,Address,PrivateKey
1,0xaddress,0xprivatekey
2,0xaddress,0xprivatekey
```

---

## Notes

- Ensure that the RPC URL and private keys are valid and secure.
- Use a reliable gas price estimation service to set multipliers appropriately.
- Test with a small number of wallets before scaling to a full list.

---

## License

This project is licensed under the MIT License.
