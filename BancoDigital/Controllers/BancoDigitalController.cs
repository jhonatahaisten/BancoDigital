using Microsoft.AspNetCore.Mvc;
using BancoDigital.Domain.Services;
using BancoDigital.Domain.Entities;

namespace BancoDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancoDigitalController : ControllerBase
    {
        private readonly IAccountService _iAccountService;

        public BancoDigitalController(IAccountService iAccountService)
        {
            _iAccountService = iAccountService;
        }

        [HttpPost("criarConta/{conta}")]
        public ActionResult<string> CreateAccountDigital(int conta)
        {
            return _iAccountService.CreateAccount(conta);
        }

        [HttpPut("depositar")]
        public ActionResult<Account> DepositAccountDigital(AccountWhithdraw accountWhithdraw)
        {
            return _iAccountService.DepositAccount(accountWhithdraw);
        }

        [HttpPut("sacar")]
        public ActionResult<Account> WhithdrawAccountDigital(AccountWhithdraw accountWhithdraw)
        {
            return _iAccountService.WhithdrawAccount(accountWhithdraw);
        }

        [HttpGet("saldo")]
        public ActionResult<double> BalanceAccountDigital(AccountWhithdraw accountWhithdraw)
        {
            return _iAccountService.BalanceAccount(accountWhithdraw);
        }

    }
}