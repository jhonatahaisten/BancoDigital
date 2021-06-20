using BancoDigital.Domain.Entities;
using BancoDigital.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _iAccountRepository;

        public AccountService(IAccountRepository iAccountRepository)
        {
            _iAccountRepository = iAccountRepository;
        }

        public ActionResult<string> CreateAccount(int accountNumber)
        {
            var contaValida = _iAccountRepository.GetAccount(accountNumber);

            if (contaValida == null)
            {
                Account novaConta = new Account
                {
                    Conta = accountNumber,
                    Saldo = 0
                };

                _iAccountRepository.SetAccount(novaConta);
                return "Conta Criada: " + novaConta.Conta;
            }

            return "Conta já existente!";
        }

        public ActionResult<Account> DepositAccount(AccountWhithdraw accountWhithdraw)
        {
            var contaValida = _iAccountRepository.GetAccount(accountWhithdraw.Conta);

            if (contaValida == null)
            {
                return new BadRequestObjectResult("Conta não cadastrada.");
            }

            contaValida.Saldo += accountWhithdraw.Valor;
            _iAccountRepository.SetDeposit(contaValida);

            return contaValida;
        }

        public ActionResult<Account> WhithdrawAccount(AccountWhithdraw accountWhithdraw)
        {
            var contaValida = _iAccountRepository.GetAccount(accountWhithdraw.Conta);

            if (contaValida == null)
            {
                return new BadRequestObjectResult("Conta não cadastrada.");
            }

            if (contaValida.Saldo < accountWhithdraw.Valor)
            {
                return new BadRequestObjectResult("Saldo insuficiente.");
            }

            if (accountWhithdraw.Valor <= 0)
            {
                return new BadRequestObjectResult("Valor para saque precisa ser maior que zero.");
            }

            contaValida.Saldo -= accountWhithdraw.Valor;
            _iAccountRepository.SetWhithdraw(contaValida);

            return contaValida;
        }

        public ActionResult<double> BalanceAccount(AccountWhithdraw accountWhithdraw)
        {
            var contaValida = _iAccountRepository.GetAccount(accountWhithdraw.Conta);

            if (contaValida == null)
            {
                return new BadRequestObjectResult("Conta não cadastrada.");
            }

            return contaValida.Saldo;
        }
    }
}
