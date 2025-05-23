# BD: Trabalho Prático APF-T

**Grupo**: P2G7

- Diogo Silva, MEC: 108212
- Duarte Santos, MEC: 113304

## Instruções de Execução (Development)

Pode alterar as credenciais de acesso à base de dados na class `Database` no projeto, que se encontra dentro da pasta `./ui/funeraria/Entities/` modificando as variáveis `serverAddress`, `databaseName`, `databaseUsername` e `databasePassword`.

[Ficheiro Database](./ui/funeraria/Entities/Database "Database.cs")

---

## Introdução

A nossa base de dados tem como objetivo apoiar na gestão de serviços funerários, organizando de forma eficiente os processos, clientes, produtos, cerimónias e recursos humanos envolvidos.

---

## Análise de Requisitos

O sistema deve permitir:
- Registo e autenticação de utilizadores responsáveis por gerir processos:
    - Para o Registo é necessário: Nome, Email, Nome de utilizador, Palavra-passe e uma imagem (opcional);
    - Para a autenticação apenas o Nome do utilizador e a password;
- Possuir processos criados por um user (funcionário) que representam um serviço fúnebre com toda a informação detalhada;
- Que os funcionários registem os dados essenciais do requerente (client). Esses dados devem incluir:
    - Nome, Contacto, BI e Grau de parentesco em relação ao falecido;
- Que as informações pessoais sobre o falecido que devem ser registadas:
    - Nome, Sexo, Data de nascimento, Nacionalidade, Estado civil, Residência e BI;
- Ter cada processo associado a um funeral;
- A possibilidade de haver cerimónia religiosa e para tal vai ser preciso:
    - Padre;
    - Igreja;
- Gerir o inventário e encomendas de produtos. Dentro dos produtos temos os seguintes tipos:
    - Flores;
    - Urnas;
    - Caixão;
- Ter Floristas que são fornecedores de flores;
- Registar o local e preço associado ao destino final do corpo;
- Escolher entre cremação ou enterro:
    - Em caso de enterro, apenas precisamos de um caixão e de um cemitério;
    - No caso de cremação, vamos precisar de uma Urna, um caixão e de um crematório;

---

## DER - Diagrama Entidade Relacionamento/Entity Relationship Diagram

### Versão final

![DER Diagram!](./DER.png "DER Diagram")

---

## ER - Esquema Relacional

### Versão Final

![ER Diagram!](./ER.png "ER Diagram")

---

## ​SQL DDL - Data Definition Language

[SQL DDL File](./db/01_ddl.sql "SQLFileQuestion")

---

## SQL DML - Data Manipulation Language

### Authentication

#### Login

Inicialmente é apresentado uma página de login para poder aceder à aplicação. Para tal usamos um stored procedure `AuthenticateUser` para fazer a autenticação com as credenciais do utilizador. O SP  `AuthenticateUser` devolve o id do utilizador se as credenciais estiverem corretas, caso contrário envia um error message.

<img src="screenshots/login.png" alt="Login!" title="AnImage" width="400"/>


#### Register

Neste campo, podemos fazer novo registo de novos funcionários na aplição, sendo necessário fazer login após o registo. Para o registo usamos o SP `RegisterUser`. Não são precisas queries adicionais, porque após o registo é preciso fazer login.

<img src="screenshots/register.png" alt="Register!" title="AnImage" width="400"/>


#### View Profile

Nesta secção é possível ver todas as informações relacionadas ao funcionário. Neste página tb podemos editar e eleminar o mesmo.

```sql
SELECT * FROM Users WHERE id = @UserId
```
<img src="screenshots/EditProfile.png" alt="Register!" title="AnImage" width="400"/>

Para atualizar as informações do utilizador, utilizamos a stored procedure `sp_updateUser`.

```sql
-- Para apagar a conta usamos:
DELETE FROM Users WHERE id = @UserID
```

#### Inventory Page
Nesta secção é feita a gestão de inventário da funerária, nomeadamente dos  caixões, urnas e das flores. Para dar fetch de todo o inventário fazemos:

```sql
SELECT * FROM Products
```

<img src="screenshots/Inventory.png" alt="Register!" title="AnImage" width="400"/>

Além disso podemos filtrar os produtos por tipo. Para tal, usamos uma view para ir buscar o Type de cada produto, `vw_AllProducts`. Nesta query, é necessário passar o `@productID` para selecionar o tipo do mesmo.

```sql
SELECT Tipo FROM vw_AllProducts WHERE id = @productID
```

No exemplo abaixo estamos apenas a selecionar os produtos do tipo `coffin`.

<img src="screenshots/InventoryFilters.png" alt="Register!" title="AnImage" width="400"/>

Ainda no página do inventário, podemos comprar mais quantidade de um produto e ver os detalhes do mesmo. 

Para atualizar o stock de um produto em especifico:

```sql
-- Selecionar o produto
SELECT stock FROM Products WHERE id = @productId

-- Atualizar o stock
UPDATE Products SET stock = stock + @quantity WHERE id = @productId
```

<img src="screenshots/ShopProduct.png" alt="Register!" title="AnImage" width="400"/>


Para ver detalhes do mesmo:

<img src="screenshots/InfoProduct.png" alt="Register!" title="AnImage" width="400"/>


#### DataBase Page

A aba DataBase da funerária é onde o funcionário faz a gestão de:
- Cemitérios;
- Crematórios;
- Padres;
- Igrejas;
- Florista;

<img src="screenshots/DataBasePage.png" alt="Register!" title="AnImage" width="400"/>

**Gestão de um crematório**

Dentro da página de Base de Dados, se selecionarmos a aba do crematório, vamos diretamente para a página de gestão de crematórios. Aqui, podemos criar/editar/eliminar crematórios.

<img src="screenshots/CrematoryPage.png" alt="Register!" title="AnImage" width="400"/>

**Criação de um novo crematório**

Para adicionar um novo crematório temos que fazer uso de um sp `sp_addCrematory`. Para o fazer temos que dar uma `@Location`, um `@Contact` e um `@Price`.

```sql
CREATE PROCEDURE sp_addCrematory
    @Location NVARCHAR(255),
	@Contact INT,
	@Price DECIMAL(18, 2),
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS (SELECT * FROM Crematory WHERE contact = @Contact)
        BEGIN
            SET @Status = 0;
            SET @Message = 'Cemetery with contact already exist!';
            RETURN;
        END

        INSERT INTO dbo.Crematory(location, contact, price)
        VALUES(@Location, @Contact, @Price)

        SET @Status = 1;
        SET @Message = 'Crematory add successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO
```

<img src="screenshots/AddCrematory.png" alt="Register!" title="AnImage" width="250"/>

**Editar um Crematório**

Como referido anteriormente, é possível editar um crematório, e para isso utilizamos a stored procedure `sp_updateCrematory`.


<img src="screenshots/EditCrematory.png" alt="Register!" title="AnImage" width="250"/>



Para apagar um crematório é utilizada a stored procedure `sp_removeGameImage`.

```sql
DELETE FROM Crematory WHERE id = @CrematoryID
```


```
Esta lógica é aplicada para os restantes elementos da página Base dados da funerária.
```

#### Process Page

Nesta página é feita a gestão de todos os processos funebres. No fundo, aqui é onde o funcinário cria um processo relativo a um funeral e detalha toda a informação do mesmo. 

<img src="screenshots/ProcessPage.png" alt="Register!" title="AnImage" width="400"/>


Primeiramente quando queremos criar um novo processo precisamos de lhe associar um número (process number). Para tal fazemos uso de uma UDF para verificar a existência de um processo já com esse número, `findProcNumberExists`.

```sql
CREATE FUNCTION dbo.findProcNumberExists(@proc_number INT)
RETURNS BIT
AS 
BEGIN
    DECLARE @exists BIT;

    IF EXISTS (
        SELECT 1
        FROM Process pr
        WHERE pr.num_process = @proc_number
    )
        SET @exists = 1;
    ELSE
        SET @exists = 0;

    RETURN @exists;
END
GO
```

Para criar um processo vamos precisar das seguintes entidades:
- Deceased;
- Person;
- Representative;
- Client;
- Funeral;
- Have;
- Cremation; (dependendo do tipo de funeral)
- Burial; (dependendo do tipo de funeral)

Este processo de criação do processo é feito usando o SP `sp_addProcess`. Como este passo envolve inserção de dados em diversas tabelas então fazemos uso do `TRANSACTION` (para caso de erro dar rollback). 

**Eliminar um processo**

Para eliminar um processo usamos um SP `sp_DeleteProcess` e ainda um trigger `trg_DeleteProcess`. Este último serve de suporte que para quando eliminamos um processo, o mesmo remover informação sobre o processo em causa em tabelas associadas.

```sql
CREATE TRIGGER trg_DeleteProcess
ON dbo.Process
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the process IDs that are being deleted
    DECLARE @deletedProcesses TABLE (process_id INT);
    INSERT INTO @deletedProcesses SELECT num_process FROM deleted;

    -- Delete from Flowers first (references Process directly)
    DELETE FROM dbo.Flowers
    WHERE process_num IN (SELECT process_id FROM @deletedProcesses);

    -- Delete from Cremation (relies on Funeral)
    DELETE FROM dbo.Cremation
    WHERE funeral_id IN (SELECT process_id FROM @deletedProcesses);

    -- Delete from Burial (relies on Funeral)
    DELETE FROM dbo.Burial
    WHERE funeral_id IN (SELECT process_id FROM @deletedProcesses);

    -- Delete from Funeral
    DELETE FROM dbo.Funeral
    WHERE num_process IN (SELECT process_id FROM @deletedProcesses);

    -- Finally delete from Process table
    DELETE FROM dbo.Process
    WHERE num_process IN (SELECT process_id FROM @deletedProcesses);
END;
GO
```
<img src="screenshots/CreateProcess.png" alt="Register!" title="AnImage" width="400"/>


---

### Aplicação de Cursors

No nosso projeto de gestão de funerária, os cursores são principalmente utilizados para o cálculo dinâmico de orçamentos de processos fúnebres, implementados na stored procedure `sp_UpdateFuneralBudgets`. Eis como funcionam:

```sql
CREATE PROCEDURE sp_UpdateFuneralBudgets
AS
BEGIN
    DECLARE @process_id INT,
            @cemetery_price DECIMAL(10,2),
            @crematory_price DECIMAL(10,2),
            @priest_price DECIMAL(10,2),
            @container_price DECIMAL(10,2),
            @flower_price DECIMAL(10,2),
            @total_price DECIMAL(10,2),
            @funeral_type NVARCHAR(50);

    DECLARE funeral_cursor CURSOR FOR
        SELECT f.num_process,
            CASE
                WHEN EXISTS (SELECT 1 FROM Burial WHERE funeral_id = f.num_process) THEN 'Burial'
                WHEN EXISTS (SELECT 1 FROM Cremation WHERE funeral_id = f.num_process) THEN 'Cremation'
                ELSE NULL
            END AS funeral_type
            FROM Funeral f;

    OPEN funeral_cursor;
    FETCH NEXT FROM funeral_cursor INTO @process_id, @funeral_type;

    WHILE @@FETCH_STATUS = 0
    BEGIN

        -- Reset prices
        SET @cemetery_price = 0;
        SET @crematory_price = 0;
        SET @container_price = 0;
        SET @flower_price = 0;
        SET @priest_price = 0;

        -- Handle Burial
        IF @funeral_type = 'Burial'
        BEGIN
            SELECT @cemetery_price = c.price
            FROM Cemetery c
            JOIN Burial b ON b.cemetery_id = c.id
            WHERE b.funeral_id = @process_id;

            SELECT @container_price = p.price
            FROM Burial b
            JOIN Coffin cf ON cf.id = b.coffin_id
            JOIN Products p ON p.id = cf.id
            WHERE b.funeral_id = @process_id;
        END

        -- Handle Cremation
        IF @funeral_type = 'Cremation'
        BEGIN
            SELECT @crematory_price = c.price
            FROM Crematory c
            JOIN Cremation b ON b.crematory_id = c.id
            WHERE b.funeral_id = @process_id;

            DECLARE @cremation_coffin_price DECIMAL(10,2) = 0;
            DECLARE @urn_price DECIMAL(10,2) = 0;

            SELECT @cremation_coffin_price = p.price
            FROM Cremation c
            JOIN Coffin cf ON cf.id = c.coffin_id
            JOIN Products p ON p.id = cf.id
            WHERE c.funeral_id = @process_id;

            SELECT @urn_price = p.price
            FROM Cremation c
            JOIN Urn u ON u.id = c.urn_id
            JOIN Products p ON p.id = u.id
            WHERE c.funeral_id = @process_id;

            SET @container_price = ISNULL(@cremation_coffin_price, 0) + ISNULL(@urn_price, 0);
        END

        -- Preço das flores: Produtos ligados à tabela Flowers
        SELECT @flower_price = SUM(p.price * f.quantity)
        FROM Flowers f
        JOIN Products p ON p.id = f.id
        WHERE f.process_num = @process_id;

        -- Preço do padre
        SELECT @priest_price = p.price
        FROM Funeral f
        JOIN Priest p ON p.representative_bi = f.priest_bi
        WHERE f.num_process = @process_id;


        -- Soma total
        SET @total_price = ISNULL(@cemetery_price, 0) +
                           ISNULL(@crematory_price, 0) +
                           ISNULL(@container_price, 0) +
                           ISNULL(@flower_price, 0) +
                           ISNULL(@priest_price, 0);

        PRINT 'PROCESS ID: ' + CAST(@process_id AS VARCHAR);
        PRINT 'Cemetery Price: ' + CAST(ISNULL(@cemetery_price, 0) AS VARCHAR);
        PRINT 'Crematory Price: ' + CAST(ISNULL(@crematory_price, 0) AS VARCHAR);
        PRINT 'Container Price: ' + CAST(ISNULL(@container_price, 0) AS VARCHAR);
        PRINT 'Flower Price: ' + CAST(ISNULL(@flower_price, 0) AS VARCHAR);
        PRINT 'Priest Price: ' + CAST(ISNULL(@priest_price, 0) AS VARCHAR);
        PRINT 'TOTAL: ' + CAST(@total_price AS VARCHAR);
        PRINT '------------------------------------';

        UPDATE Process
        SET budget = @total_price
        WHERE num_process = @process_id;

        FETCH NEXT FROM funeral_cursor INTO @process_id, @funeral_type;
    END;

    CLOSE funeral_cursor;
    DEALLOCATE funeral_cursor;
END;
GO
```

Este cursor percorre todos os registos na tabela Funeral, determinando se cada processo é um enterro ou uma cremação

#### Funcionamento do Cálculo de Orçamentos
- Para cada registo do funeral, o cursor:

1. Reinicia variáveis de preço no início de cada iteração
2. Aplica cálculos diferentes consoante o tipo de funeral:
    - Para Enterro: Obtém o preço do cemitério e do caixão
    - Para Cremação: Obtém o preço do crematório, do caixão e da urna
3. Calcula custos adicionais que são comuns a todos os funerais:
    - Preço das flores
    - Honorários do padre
4. Soma todos os componentes para chegar ao orçamento total
5. Atualiza a tabela Process com o valor calculado

#### Integração com a Aplicação
O cursor é invocado através do método `UpdateProcessBudget()` na classe `Database.cs`, que chama a stored procedure:

```csharp
public void UpdateProcessBudget() {
    using (SqlCommand cmd = new SqlCommand("sp_UpdateFuneralBudgets", conn)) {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
    }
}
```
Vantagens:
1. **Cálculo Personalizado**: Permite calcular orçamentos específicos para cada funeral baseado nos seus componentes únicos
2. **Diferenciação por tipo**: Gere adequadamente os diferentes requisitos de preços para enterros e cremações
3. **Centralização da lógica**: Mantém toda a lógica de cálculo de orçamentos numa única stored procedure
4. **Diagnóstico facilitado**: Inclui instruções PRINT para depuração que ajudam a verificar os cálculos



A implementação é apoiada por índices específicos que melhoram o desempenho das operações do cursor, minimizando o impacto na performance global do sistema.


<img src="screenshots/Cursors.png" alt="Register!" title="AnImage" width="400"/>

---
### Aplicação dos Índices

#### Implementação
Em relação aos indices, implementámos vários tipos de modo a otimizar o desempenho da base de dados. Estes índices estão organizados em categorias específicas no ficheiro `07_Indexes.sql`

1. Índices para acelerar JOINS e WHERES
```sql
CREATE INDEX idx_funeral_num_process ON Funeral(num_process);
CREATE INDEX idx_process_num_process ON Process(num_process);
CREATE INDEX idx_funeral_church_id ON Funeral(church_id);
CREATE INDEX idx_funeral_deceased_bi ON Funeral(deceased_bi);
CREATE INDEX idx_burial_funeral_id ON Burial(funeral_id);
CREATE INDEX idx_cremation_funeral_id ON Cremation(funeral_id);
CREATE INDEX idx_flowers_process_num ON Flowers(process_num);
```
São Fundamentais para acelerar a execução do cursor funeral cursor na stored procedure `sp_UpdateFuneralBudgets`, pois otimizam as junções entre as tabelas relacionadas a funerais.


2. Índices para colunas frequentemente acedidas
```sql
CREATE INDEX idx_users_username ON Users(username);
CREATE INDEX idx_users_email ON Users(mail);
CREATE INDEX idx_users_id ON Users(id);
CREATE INDEX idx_person_bi ON Person(bi);
```
Estes índices melhoram o desempenho das operações de autenticação e pesquisa de utilizadores e pessoas, que são frequentemente consultadas nas stored procedures `AuthenticateUser` e `RegisterUser`.


3. Índices para análises e relatórios

```sql
CREATE INDEX idx_process_start_date ON Process(start_date);
CREATE INDEX idx_products_stock ON Products(stock);
CREATE INDEX idx_process_type_of_payment ON Process(type_of_payment);
CREATE INDEX idx_funeral_funeral_date ON Funeral(funeral_date);
```
Estes índices facilitam a geração de relatórios e análises temporais sobre processos, pagamentos e gestão de stock.

4.  Índices compostos para consultas específicas
```sql
CREATE INDEX idx_person_bi_name ON Person(bi, name);
CREATE INDEX idx_funeral_church__deceased ON Funeral(church_id, deceased_bi);
CREATE INDEX idx_produts_price_stock ON Products(price, stock);
```
Os índices compostos são particularmente úteis para as views complexas como `vw_LoadProcess` e `vw_AllProducts`, que utilizam múltiplas condições de junção.


5. Índices únicos para integridade de dados
```sql
CREATE UNIQUE INDEX idx_users_unique_username ON Users(username);
CREATE UNIQUE INDEX idx_users_unique_email ON Users(mail);
```
Estes índices garantem a unicidade dos nomes de utilizador e e-mails, reforçando a integridade dos dados fundamentais do sistema.

---

## Normalização/Normalization

Para minimizar a duplicação de dados e otimizar o uso de espaço, aplicámos a normalização até à 3FN. Com isto asseguramos que cada tabela possuí uma primary key, garantindo que cada registo é único. Utilizámos foreign keys para referenciar outras tabelas sem necessidade de duplicar informação. Os dados foram distribuídos por várias tabelas de forma a evitar a repetição e a garantir a integridade dos mesmos.


#### Primeira Forma Normal (1FN)

**Critério:** Todos os atributos nas tabelas são atómicos. Não há listas, arrays ou campos compostos.

- Todas as tabelas utilizam **atributos atómicos**, sem listas ou estruturas compostas.
- Exemplo: `Flowers`, `Coffin`, `Urn` são entidades separadas, não agrupadas em campos compostos dentro de `Products`.

**Conclusão**: Todas as tabelas estão em 1FN.


#### Segunda Forma Normal (2FN)

**Critério:**  As tabelas não mostram dependências parciais. Por exemplo, Process tem client_id, e os dados do cliente estão noutra tabela (Client, Representative, Person), o que evita dependência parcial de atributos.

- Tabelas com chaves compostas como `Have(priest_bi, church_id)` foram analisadas.
- Todos os atributos dependem da chave **por completo**.
- Tabelas como `Client`, `Representative`, `Deceased` têm **chaves simples**, portanto **não se aplicam dependências parciais**.

**Conclusão**: Todas as tabelas estão em 2FN.

#### Terceira Forma Normal (3FN)

**Critério:** Não existem dependências transitivas óbvias. Ex.: os dados da pessoa falecida estão em Deceased e os seus dados pessoais estão em Person, evitando redundância e facilitando atualizações.

- Os atributos `name`, `contact`, `title` estão bem separados entre `Person`, `Representative` e `Priest`.
- Em `Process`, `client_id` é uma FK para `Client`, e os detalhes do cliente estão fora da tabela (sem dependência transitiva).
- O modelo evita repetição de atributos como `price`, `location`, que pertencem às tabelas especializadas como `Cemetery` e `Crematory`.

**Conclusão**: A estrutura está livre de dependências transitivas e cumpre a 3FN.


#### Conclusões finais

A base de dados encontra-se **completamente normalizada até à 3FN**. Foi evitada qualquer violação à integridade ou presença de dados redundantes.

---

## Dados iniciais da dabase de dados/Database init data

Os ficheiros devem ser executados pela ordem que estão numerados para que não haja erros devido a dependências.

[01_ddl.sql](db/01_ddl.sql "DDL File")

[07_db_init.sql](sql/02_db_init.sql "DB init")

[02_views.sql](db/03_views.sql "Views File")

[03_sp.sql](sql/04_sp.sql "Stored Procedures File")

[04_udf.sql](sql/05_udf.sql "UDFs File")

[05_triggers.sql](sql/06_triggers.sql "Triggers File")

[06_indexes.sql](sql/07_indexes.sql "Indexes File")

---