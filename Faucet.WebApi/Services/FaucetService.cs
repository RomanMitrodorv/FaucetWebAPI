using Faucet.WebApi.Infastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet.WebApi.Services
{
    public class FaucetService : IFaucetService
    {
        private IBlockchainService _blockhainService;
        private UserTransactionContext _userTransactionContext;

        public FaucetService(IBlockchainService blockhainService, UserTransactionContext userTransactionsContext)
        {
            _blockhainService = blockhainService ?? throw new ArgumentNullException(nameof(blockhainService));
            _userTransactionContext = userTransactionsContext ?? throw new ArgumentNullException(nameof(userTransactionsContext));
        }

        public async Task Execute(string address, string ip)
        {
            DateTime today = DateTime.Today;

            var userTran = _userTransactionContext.UserTransactions
                .Where(x => x.IP == ip
                && x.Address == address
                && x.Date >= today).FirstOrDefault();

            if (userTran == null)
            {
                await _blockhainService.SendTransaction(address);

                _userTransactionContext.UserTransactions.Add(new Model.UserTransaction()
                {
                    Address = address,
                    Date = DateTime.Now,
                    IP = ip
                });

                _userTransactionContext.SaveChanges();

            }
        }
    }
}
