using BancoDigital.Domain.Entities;

namespace BancoDigital.Domain.Repositories
{
    public interface IAccountRepository 
    {
        void SetAccount(Account account);
        Account GetAccount(int account);
        void SetDeposit(Account account);
        void SetWhithdraw(Account account);
    }
}
