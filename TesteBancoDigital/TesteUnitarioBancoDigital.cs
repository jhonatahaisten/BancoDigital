using BancoDigital.Domain.Entities;
using BancoDigital.Domain.Services;
using BancoDigital.Infrastructure;
using BancoDigital.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TesteBancoDigital
{
    public class TesteUnitarioBancoDigital
    {
        private readonly BancoDigitalContexto _bancoDigitalContexto;
        private readonly AccountRepository _accountRepository;
        private readonly AccountService _acountService;

        public TesteUnitarioBancoDigital()
        {
            var builder = new DbContextOptionsBuilder<BancoDigitalContexto>();
            builder.UseInMemoryDatabase(databaseName: "BancoDigitalDb");

            var dbContextOptions = builder.Options;
            _bancoDigitalContexto = new BancoDigitalContexto(dbContextOptions);         
            _bancoDigitalContexto.Database.EnsureDeleted();
            _bancoDigitalContexto.Database.EnsureCreated();

            _accountRepository = new AccountRepository(_bancoDigitalContexto);
            _acountService = new AccountService(_accountRepository);
        }

        [Fact]
        public void AccountNotFound()
        {
            Account account = _accountRepository.GetAccount(45);
            Assert.Null(account);
        }

        [Fact]
        public void AccountFound()
        {
            int numberAccount = 46;
            CreateAccount(numberAccount);            
            Account account = _accountRepository.GetAccount(numberAccount);
            Assert.NotNull(account);
        }

        [Fact]
        public void SetAccountTest()
        {
            Account conta = new Account { Conta = 47 };
            _accountRepository.SetAccount(conta);   
            Assert.True((conta.Id > 0));
        }

        [Fact]
        public void TestCreateAccount()
        {            
            Assert.StartsWith("Conta Criada:", _acountService.CreateAccount(48));           
        }

        [Fact]
        public void TestCreateAccountExist()
        {               
            CreateAccount(49);
            Assert.StartsWith("Conta já existente.", _acountService.CreateAccount(49));
        }

        [Fact]
        public void TestDepositAccountSuccess()
        {
            int numberAccount = 50;
            CreateAccount(numberAccount);  
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 1000 };
            ActionResult<Account> account = _acountService.DepositAccount(accountWhithdraw);
            Assert.True(account.Value.Saldo == 1000);    
        }

        [Fact]
        public void TestDepositAccountNotSuccess()
        {        
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Valor = 1000 };
            ActionResult<Account> account = _acountService.DepositAccount(accountWhithdraw);
            Assert.Null(account.Value);
        }

        [Fact]
        public void TestWhithdrawAccountNotSuccess()
        {
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Valor = 1000 };
            ActionResult<Account> account = _acountService.WhithdrawAccount(accountWhithdraw);
            Assert.Null(account.Value);
        }

        [Fact]
        public void TestWhithdrawAccountNot()
        {
            int numberAccount = 51;
            CreateAccount(numberAccount);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 1000 };
            ActionResult<Account> account = _acountService.WhithdrawAccount(accountWhithdraw);
            Assert.Null(account.Value);
        }        

        [Fact]
        public void TestWhithdrawAccountZero()
        {
            int numberAccount = 52;
            CreateAccount(numberAccount);
            CreateAccountBalance(numberAccount, 1000);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 0 };
            ActionResult<Account> account = _acountService.WhithdrawAccount(accountWhithdraw);
            Assert.Null(account.Value);
        }

        [Fact]
        public void TestWhithdrawAccountSuccess()
        {
            int numberAccount = 53;
            CreateAccountBalance(numberAccount, 1000);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 50 };
            ActionResult<Account> account = _acountService.WhithdrawAccount(accountWhithdraw);
            Assert.True(account.Value.Saldo == 950);
        }

        [Fact]
        public void BalanceAccounttNotSuccess()
        {
            int numberAccount = 54;
            CreateAccountBalance(numberAccount, 1000);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount };
            ActionResult<double> saldo = _acountService.BalanceAccount(accountWhithdraw);
            Assert.True(saldo.Value != 0);
        }

        [Fact]
        public void BalanceAccounttNotFound()
        {
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Valor = 1000 };
            ActionResult<double> saldo = _acountService.BalanceAccount(accountWhithdraw);
            Assert.True(saldo.Value == 0);
        }

        private void CreateAccountBalance(int account, double saldo)
        {
            CreateAccount(account);
            AccountWhithdraw conta = new AccountWhithdraw { Conta = account, Valor = saldo };       
            _acountService.DepositAccount(conta);
        }

        private Account CreateAccount(int account)
        {
            Account conta = new Account { Conta = account };
            AccountRepository accountRepository = new AccountRepository(_bancoDigitalContexto);
            accountRepository.SetAccount(conta);
            return conta;
        }
    }
}
