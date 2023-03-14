using System;

namespace Faucet.WebApi.Model
{
    public class UserTransaction
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }
    }
}
