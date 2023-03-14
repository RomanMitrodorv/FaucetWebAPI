using Nethereum.RPC.Eth.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Faucet.WebApi.Services
{
    public interface IBlockchainService
    {
        Task<List<TransactionReceipt>> SendTransaction(string toAddress);
    }
}
