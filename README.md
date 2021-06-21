# Desafio Técnico: Criação de API

**Arquitetura da API:**

- C# com .Net core 5.0
- Banco de dados SQL Server dockerizado
- Injeção de dependência
- Repository-Service pattern
- Orientação a objetos
- Testes unitários com xUnit e banco de dados em memória
- API Rest com swagger
- Geração de relatório da cobertura de testes com coverlet

# Preparação para execução da API

Restaure as dependências do NUGET pela interface do VS ou pelo comando:

```console
dotnet restore 
```


**Certifique-se que o docker esteja rodando na sua máquina.**



Rode o comando abaixo para inicializar o banco:
```console
docker run -e "ACCEPT_EULA=Y" -e 'MSSQL_PID=Express' -e "SA_PASSWORD=Funcional2021" -p 1433:1433 --name sql1 -h sql1 -d mcr.microsoft.com/mssql/server:2019-latest
```


Abra o "Package Manager Console" e rode o comando abaixo no diretório do projeto "BancoDigital.Infrastructure":
```console
update-database
```

Rode a aplicação de forma manual no VS

**Para facilitar os testes, foi enviado uma colection do Postman.**

# Testes unitários

Para realizar os testes unitários, rode o comando abaixo, onde será executado todos os testes na aplicação e ao final será gerado um xml:
```console
dotnet test --collect:"XPlat Code Coverage" --settings runsettings.xml
```
O resultado dos testes será criado no diretório do projeto ...\BancoDigital\TesteBancoDigital\TestResults

Copie o caminho completo "coverage.cobertura.xml", a qual foi gerado no diretório do projeto de testes, em seguida, rode o comando conforme o exemplo abaixo, substituindo os campos com os caminhos da sua máquina:
```console
dotnet C:\Users\Jhonata\.nuget\packages\reportgenerator\4.8.10\tools\net5.0\ReportGenerator.dll "-reports:C:\Users\Jhonata\source\repos\BancoDigital\TesteBancoDigital\TestResults\fca46ab9-080c-449a-ae52-7401cbf9f79b\coverage.cobertura.xml" "-targetdir:C:\coveragereport"
```	
Agora basta acessar o resultado dos teste pelo index.html no caminho 

```console
C:\coveragereport\index.html
```	
