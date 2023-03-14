using Faucet.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Faucet.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FaucetController : Controller
    {
        private readonly IFaucetService _faucetService;

        public FaucetController(IFaucetService faucetService)
        {
            _faucetService = faucetService ?? throw new ArgumentNullException(nameof(faucetService));
        }


        [HttpPost]
        [Route("{address}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Execute([FromBody] string address)
        {
            var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

            try
            {
                await _faucetService.Execute(address, ip.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
