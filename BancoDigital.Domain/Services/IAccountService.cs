using BancoDigital.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.Domain.Services
{
    public interface IAccountService
    {
        ActionResult<string> CreateAccount(int account);
        ActionResult<Account> DepositAccount(AccountWhithdraw accountWhithdraw);
        ActionResult<Account> WhithdrawAccount(AccountWhithdraw accountWhithdraw);
        ActionResult<double> BalanceAccount(AccountWhithdraw AccountWhithdraw);  
    }
}
