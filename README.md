# ua-bd-funeraria
Projeto Base de Dados 2025

Grupo P2G7
- Diogo Silva - nMec108212
- Duarte Santos - nMec113304


## SQl DDL - Data Definition Language
[SQL DDL File](db/01_CreateTables.sql "SQL DDL File") 


## Configuração da Conexão com a Base de Dados
Para testar a aplicação com suas próprias credenciais de banco de dados, é necessário modificar o arquivo:

`ui/funeraria/Entities/Database.cs` (linhas 19-24)

Neste arquivo, localize as seguintes linhas de código que contêm as informações de conexão:

```csharp
private static string serverAddress = "tcp:mednat.ieeta.pt\\SQLSERVER,8101";
private static string databaseName = "p2g7";
private static string databaseUsername = "p2g7";
private static string databasePassword = "Zeca_Duarte_2025";
private static string connectionString = "data source=" + serverAddress + ";initial catalog=" + databaseName + ";uid=" + databaseUsername + ";password=" + databasePassword;
```

Nestas linhas de código, apenas devem ser substituidas as credenciais de logins para o servidor de base de dados: nome(databaseName), utilizador(databaseUsername), password(databasePassword) tal como o o endereço do servidor(serverAddress). 


    