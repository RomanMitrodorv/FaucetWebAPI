using System.Threading.Tasks;

namespace Faucet.WebApi.Services
{
    public interface IFaucetService
    {
        Task Execute(string address, string ip);
    }
}
