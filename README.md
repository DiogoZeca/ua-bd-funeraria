# ua-bd-funeraria
Data Base Project for the course of Database at University of Aveiro Group P2G7
- Diogo Silva - nMec108212
- Duarte Santos - nMec113304


## SQl DDL - Data Definition Language
[SQL DDL File](db/01_CreateTables.sql "SQL DDL File") 


## Database Connection Configuration
To test the application with your own database credentials, you need to modify the file:

`ui/funeraria/Entities/Database.cs` (lines 19-24)

In this file, locate the following lines of code that contain the connection information:

```csharp
private static string serverAddress = "tcp:mednat.ieeta.pt\\SQLSERVER,8101";
private static string databaseName = "p2g7";
private static string databaseUsername = "p2g7";
private static string databasePassword = "Zeca_Duarte_2025";
private static string connectionString = "data source=" + serverAddress + ";initial catalog=" + databaseName + ";uid=" + databaseUsername + ";password=" + databasePassword;
```

In these lines of code, only the login credentials for the database server should be replaced: name (databaseName), username (databaseUsername), password (databasePassword) as well as the server address (serverAddress). 


    