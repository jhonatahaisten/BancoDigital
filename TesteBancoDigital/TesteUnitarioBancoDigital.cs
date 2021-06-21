
using BancoDigital.Domain.Entities;
using BancoDigital.Domain.Repositories;
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
        public TesteUnitarioBancoDigital()
        {
            var builder = new DbContextOptionsBuilder<BancoDigitalContexto>();
            builder.UseInMemoryDatabase(databaseName: "BancoDigitalDb");

            var dbContextOptions = builder.Options;
            _bancoDigitalContexto = new BancoDigitalContexto(dbContextOptions);         
            _bancoDigitalContexto.Database.EnsureDeleted();
            _bancoDigitalContexto.Database.EnsureCreated();
        }

        [Fact]
        public void AccountNotFound()
        {
            var teste = new AccountRepository(_bancoDigitalContexto);
            Account account = teste.GetAccount(45);
            Assert.Null(account);
        }
        
        private Account CreateAccount(int account)
        {
            Account conta = new Account { Conta = account };
            var teste = new AccountRepository(_bancoDigitalContexto);
            teste.SetAccount(conta);
            return conta;
        }

        [Fact]
        public void AccountFound()
        {
            int numberAccount = 46;
            CreateAccount(numberAccount);

            var teste = new AccountRepository(_bancoDigitalContexto);
            Account account = teste.GetAccount(numberAccount);
            Assert.NotNull(account);
        }

        [Fact]
        public void SetAccountTest()
        {
            Account conta = new Account { Conta = 47 };
            var teste = new AccountRepository(_bancoDigitalContexto);
            
            teste.SetAccount(conta);           
          
            Assert.True((conta.Id > 0));
        }

        [Fact]
        public void TestCreateAccount()
        {
            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var teste = new AccountService(iAccountRepository);

            Assert.StartsWith("Conta Criada:", teste.CreateAccount(48));           
        }

        [Fact]
        public void TestCreateAccountExist()
        {
            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var teste = new AccountService(iAccountRepository);
            CreateAccount(49);
            Assert.StartsWith("Conta já existente.", teste.CreateAccount(49));
        }

        [Fact]
        public void TestDepositAccountSuccess()
        {
            int numberAccount = 50;
            CreateAccount(numberAccount);          

            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 1000 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.DepositAccount(accountWhithdraw);

            Assert.True(teste.Value.Saldo == 1000);    
        }

        [Fact]
        public void TestDepositAccountNotSuccess()
        {        
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Valor = 1000 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.DepositAccount(accountWhithdraw);

            Assert.Null(teste.Value);
        }

        //Teste de saque: conta não cadastrada
        [Fact]
        public void TestWhithdrawAccountNotSuccess()
        {
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Valor = 1000 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.WhithdrawAccount(accountWhithdraw);

            Assert.Null(teste.Value);
        }

        //Teste de saque: saldo não suficiente
        [Fact]
        public void TestWhithdrawAccountNot()
        {
            int numberAccount = 51;
            CreateAccount(numberAccount);

            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 1000 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.WhithdrawAccount(accountWhithdraw);

            Assert.Null(teste.Value);
        }
        private void CreateAccountBalance(int account, double saldo)
        {
            CreateAccount(account);
            AccountWhithdraw conta = new AccountWhithdraw { Conta = account , Valor = saldo};

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);  
            var teste = new AccountService(iAccountRepository);

            teste.DepositAccount(conta);
                    
        }
        //Teste de saque: saldo menor que zero
        [Fact]
        public void TestWhithdrawAccountZero()
        {
            int numberAccount = 52;
            CreateAccount(numberAccount);

            CreateAccountBalance(numberAccount, 1000);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 0 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.WhithdrawAccount(accountWhithdraw);

            Assert.Null(teste.Value);
        }

        //Teste de saque: sucesso
        [Fact]
        public void TestWhithdrawAccountSuccess()
        {
            int numberAccount = 53;
            CreateAccountBalance(numberAccount, 1000);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount, Valor = 50 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.WhithdrawAccount(accountWhithdraw);

            Assert.True(teste.Value.Saldo == 950);
        }

        [Fact]
        public void BalanceAccounttNotSuccess()
        {
            int numberAccount = 54;
            CreateAccountBalance(numberAccount, 1000);
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Conta = numberAccount };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.BalanceAccount(accountWhithdraw);

            Assert.True(teste.Value != 0);
        }

        [Fact]
        public void BalanceAccounttNotFound()
        {
            AccountWhithdraw accountWhithdraw = new AccountWhithdraw { Valor = 1000 };

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var accountService = new AccountService(iAccountRepository);

            var teste = accountService.BalanceAccount(accountWhithdraw);

            Assert.True(teste.Value == 0);
        }
    }
}
