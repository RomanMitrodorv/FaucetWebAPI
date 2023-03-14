using Microsoft.Extensions.Configuration;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Faucet.WebApi.Services
{
    public class BlockchainService : IBlockchainService
    {
        private const int GWEI = 147;
        public IConfiguration Configuration { get; }

        private readonly string PRIVATE_KEY;

        private readonly string CHAIN_URL;

        private readonly string SEND_AMOUNT;

        public BlockchainService(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            PRIVATE_KEY = Configuration["PrivateKey"];
            CHAIN_URL = Configuration["ChainUrl"];
            SEND_AMOUNT = Configuration["SendAmount"];
        }

        public async Task<List<TransactionReceipt>> SendTransaction(string toAddress)
        {
            var account = new Account(PRIVATE_KEY);

            var web3 = new Web3(account, CHAIN_URL);

            var amountInWei = Web3.Convert.ToWei(SEND_AMOUNT);

            var gasPrice = Web3.Convert.ToWei(0.000000001 * GWEI);

            var transactions = new List<TransactionInput>()
            {
                new TransactionInput()
                {
                    From = account.Address,
                    To = toAddress,
                    Value = new HexBigInteger(amountInWei),
                    GasPrice = new HexBigInteger(gasPrice)
                }
            };

            return await web3.TransactionManager.TransactionReceiptService.SendRequestsAndWaitForReceiptAsync(transactions);
        }
    }
}
