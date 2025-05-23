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
- Cada processo é criado por um user (funcionário) e representa um serviço fúnebre com toda a informação detalhada;
- O sistema deve permitir que os funcionários registem os dados essenciais do requerente (client). Esses dados devem incluir:
    - Nome, Contacto, BI, Grau de parentesco em relação ao falecido;
- Informações pessoais sobre o falecido devem ser registadas:
    - Nome, Sexo, Data de nascimento, Nacionalidade, Estado civil, Residência e BI;
- Cada processo deve estar associado a um funeral;
- Possibilidade de haver cerimónia relegiosa. Para tal vai ser preciso:
    - Padre;
    - Igreja;
- Gestão de inventário e encomendas de produtos. Dentro dos produtos temos os seguintes tipos:
    - Flores;
    - Urnas;
    - Caixão;
- Floristas são fornecedores de flores;
- O sistema deve permitir registar o local e preço associado ao destino final do corpo;
- Escolha entre cremação ou enterro:
    - Em caso de enterro, apenas precisamos de um caixão e de ir a um cemitério;
    - No caso de cremação, vamos precisar de uma Urna, um caixão e de ir a um crematório;

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
