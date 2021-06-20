using BancoDigital.Domain.Entities;
using BancoDigital.Domain.Repositories;
using BancoDigital.Infrastructure.Data;
using System.Linq;

namespace BancoDigital.Infrastructure
{
    public class AccountRepository :IAccountRepository
    {
        private readonly BancoDigitalContexto _context;

        public AccountRepository(BancoDigitalContexto context)
        {
            _context = context;
        }

        //Verifica se a conta existe no banco
        public Account GetAccount(int account)
        {
            return _context.Account.Where(a => a.Conta == account).FirstOrDefault();
        }

        //Insere uma nova conta
        public void SetAccount(Account account)
        {
            _context.Account.Add(account);
            _context.SaveChanges();
        }

        //Efetua deposito
        public void SetDeposit(Account account)
        {
            _context.SaveChanges();
        }

        //Efetua saque
        public void SetWhithdraw(Account account)
        {
            _context.SaveChanges();
        }
    }
}
