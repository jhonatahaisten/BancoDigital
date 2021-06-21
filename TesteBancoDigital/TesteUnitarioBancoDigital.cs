
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
            int numberAccount = 999;
            CreateAccount(numberAccount);

            var teste = new AccountRepository(_bancoDigitalContexto);
            Account account = teste.GetAccount(numberAccount);
            Assert.NotNull(account);
        }

        [Fact]
        public void SetAccountTest()
        {
            Account conta = new Account { Conta = 45 };
            var teste = new AccountRepository(_bancoDigitalContexto);
            
            teste.SetAccount(conta);           
          
            Assert.True((conta.Id > 0));
        }

        [Fact]
        public void TestCreateAccount()
        {
            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var teste = new AccountService(iAccountRepository);

            Assert.StartsWith("Conta Criada:", teste.CreateAccount(45));           
        }

        [Fact]
        public void TestCreateAccountExist()
        {
            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var teste = new AccountService(iAccountRepository);

            Assert.StartsWith("Conta já existente.", teste.CreateAccount(45));
        }

        [Fact]
        public void TestDepositAccountSuccess()
        {
            int numberAccount = 999;
            CreateAccount(numberAccount);

            IAccountRepository iAccountRepository = new AccountRepository(_bancoDigitalContexto);
            var teste = new AccountService(iAccountRepository);

            AccountWhithdraw conta = new AccountWhithdraw { Conta = 998, Valor = 1000};   

            Assert.Null(teste.DepositAccount(conta).Value);


        }

    }
}
