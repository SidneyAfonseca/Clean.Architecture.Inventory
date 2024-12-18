## Clean Architecture Inventory Setup
Essas instruções permitirão que qualquer pessoa com acesso ao código poderá executá-lo localmente para fins de desenvolvimento e teste.

### Objetivo
O documento tem como objetivo detalhar tecnicamente a estrutura e o funcionamento do projeto voltado para Arquitetura Limpa. No projeto é possível observar as seguintes características:

1. Aplicação dos padrões de Clean Code;
2. Aplicação do Pattern de TDD;
3. Aplicar o pattern de SOLID através da refatoração do TDD;
4. Aplicar o pattern de CQRS através do MediatR;
5. Aplicar o controle de Injeção de Dependência através da utilização do AutoFac;
6. Aplicar as validações das regras de negócios através do FluentValidation;

### Organização da Solução
#### 1. Clean.Architecture.Inventory.API
Essa camada é a camada de apresentação, responsável por lidar com a interface externa do sistema. A camada também é responsável pela entrada e saída de dados, utilizando o MediatR enviar comandos e consultas para a camada de aplicação.

##### Responsabilidades
* **Endpoints da API:** Define os endpoints HTTP (como GET, POST, PUT, DELETE) para interagir com o sistema.
* **Controle de requisições e respostas:** Recebe as requisições, valida os dados de entrada, e retorna as respostas apropriadas.
* **Interação com a Camada de Aplicação:** Utiliza o MediatR para enviar comandos e consultas para a camada de aplicação.

##### Componentes
* **Controllers:** Os controllers são responsáveis por receber as requisições HTTP, processar os dados de entrada, delegar a execução para a camada de aplicação através do MediatR, e retornar as respostas apropriadas ao cliente.
* **Program:** Este arquivo configura os serviços necessários para a aplicação, como o DbContext, MediatR, injeção de dependências dos repositórios e serviços, configuração de logging, etc.
* **appsettings.json:** Configuração da aplicação, como strings de conexão, parâmetros e variáveis de ambiente.

#### 2. Clean.Architecture.Inventory.Application
Essa camada gerencia a lógica da aplicação, definindo os casos de uso como comandos, queries e interfaces sem depende de detalhes da implementação.

##### Responsabilidades

* **CQRS com MediatR:** Separa operações de leitura(Queries) e escrita(Commands) utilizando o MediaTr para gerenciar o fluxo de mensagens.
* **Validações de negócio:** Utiliza o FluentValidation para validar os comandos e queries antes de serem processados.
* **Definição de interfaces:** Define contratos(Interfaces) para repositórios e serviços que serão implementados nas camadas mais internas.

##### Componentes

* **Commands:** Representam operações de escrita(Criar, Atualizar, Deletar).
* **Queries:** Representam operações de leitura.
* **Handlers:** Processam os comandos e queries, implementando a lógica da aplicação.
* **Validators:** Validam os dados de entrada para comandos e queries.
* **Interfaces:** Define contratos para repositórios e serviços que a camada de aplicação precisa.

#### 3. Clean.Architecture.Inventory.Domain

Camada de domínio, responsável por abrigar as entidades de negócio e as regras fundamentais do sistema.

##### Responsabilidades

* **Entidades de domínio:** Representam os objetos de negócio com suas propriedades e comportamentos.
* **Regras de negócio:** Definem as regras que governam o comportamento das entidades.
* **Enumerações e Valores de Domínio:** Definem tipos específicos usados pelas entidades (TransactionType).

##### Componentes
* **Entidades:** Classes que representam os modelos de dados principais do sistema (Product, InventoryTransaction, ErrorLog).
* **Enums:** Tipos enumerados que definem valores específicos para certas propriedades.

#### 4. Clean.Architecture.Inventory.Infrastructure

Esta camada gerencia a implementação de serviços que interagem com sistemas externos, como logging, envio de emails, etc

##### Responsabilidades
* **Serviços Externos:** Implementa serviços que não fazem parte diretamente das regras de negócio, como logging com Serilog.
* **Implementação de Interfaces:** Implementa as interfaces definidas na camada de aplicação para serviços como logging.

##### Componentes
* **Logging:** Serviços de logging configurados para registrar informações e erros utilizando o SeriaLog.
* **Serviços de Integração:** Implementações para serviços de terceiros, caso necessário.

#### 5. Clean.Architecture.Inventory.Persistence
Esta camada é responsável pela interação direta com o banco de dados, utilizando o Entity Framework Core para mapear e manipular as entidades.

##### Responsabilidades
* **Configuração do DbContext:** Define o contexto do banco de dados e configura as entidades.
* **Implementação de Repositórios:** Implementa as interfaces de repositórios definidas na camada de aplicação, fornecendo métodos para acessar e manipular os dados.
* **Migrações de Banco de Dados:** Gerencia as migrações para criar e atualizar o esquema do banco de dados.

##### Componentes
* **DbContext:** Classe que representa a sessão com o banco de dados e permite consultar e salvar dados.
* **Repositórios:** Classes que implementam os métodos definidos nas interfaces de repositório.
* **Configurações de Mapeamento:** Configurações adicionais para mapear as entidades para as tabelas do banco de dados.

### Sobre o projeto
O projeto tem como objetivo criar uma API para o gerenciamento do controle de estoque de peças. O sistema permite realizar operações básicas de gerenciamento de produtos e transações de estoque, garantindo a integridade dos dados e fornecendo alertas sobre o status do estoque, além de registrar qualquer erro ocorrido no projeto por meio de logs.

### Pré-requisitos
Comandos e instalações necessárias para poder executar o projeto

Verifique se você possui o serviço do MYSQL, caso não possua segue o link para instalar:
* MySQL Installer: [https://dev.mysql.com/downloads/installer/]

Caso já possua instalado, pode começar por aqui:
1. Abra o terminal do projeto e execute o seguinte comando:
   ```
   dotnet tool install --global dotnet-ef
   ```
2. Para executar a migração para o banco de dados local execute:
   * Abra o terminal do projeto novamente e navegue até a pasta persistence
     ```sh
     # O caminho deve estar assim
     ..\Clean.Architecture.Inventory\Clean.Architecture.Inventory.Persistence
     ```
     
   * Logo em seguida atualize o banco de dados com o comando:
   ```
   dotnet ef database update --project Clean.Architecture.Inventory.Persistence.csproj --startup-project ../Clean.Architecture.Inventory.API/Clean.Architecture.Inventory.API.csproj
   ```

### Testes
Aqui vão alguns exemplos do corpo para a API

#### api/products
1. Cadastro de produtos (/api/products)
   ```js
   // Corpo da requisição
   {
     "partNumber": "12345",
     "name": "Motor Elétrico",
     "averageCost": 550.75,
     "quantityInStock": 10
   }
   // Resposta esperada
   {
     "id": 1
   }
2. Obter todos os produtos (/api/products)
   ```js
   // Resposta esperada
   [
     {
       "id": 1,
        "partNumber": "PN-12345",
        "name": "Peça Exemplo A",
        "averageCost": 50.00,
        "quantityInStock": 100,
        "createdAt": "2024-04-25T14:30:00Z",
        "updatedAt": "2024-04-25T14:30:00Z",
        "inventoryTransactions": []
     },
     {
        "id": 2,
        "partNumber": "PN-67890",
        "name": "Peça Exemplo B",
        "averageCost": 75.00,
        "quantityInStock": 50,
        "createdAt": "2024-04-26T10:15:00Z",
        "updatedAt": "2024-04-26T10:15:00Z",
        "inventoryTransactions": []
     }
   ]
   ```
3. Obter produto por ID (/api/products/{id})
   ```js
   // Resposta esperada
   {
     "id": 1,
     "partNumber": "PN-12345",
     "name": "Peça Exemplo A",
     "averageCost": 50.00,
     "quantityInStock": 100,
     "createdAt": "2024-04-25T14:30:00Z",
     "updatedAt": "2024-04-25T14:30:00Z",
     "inventoryTransactions": []
   }

4. Atualizar produto por ID (api/products/{id}) 
   ```js
   // Corpo da requisição
   {
     "id": 1,
     "partNumber": "PN-12345",
     "name": "Peça Exemplo A - Atualizada",
     "averageCost": 55.00,
     "quantityInStock": 90
   }
   ```
5. Deletar produto por ID (/api/products/{id})
#### api/InventoryTransactions
1. Cadastrar nova transação de produto (/api/InventoryTransactions)
   ```js
   // Corpo da requisição
   // 1 = Entrada / 2 = Saída
   {
     "productId": 1,
     "type": 1,
     "quantity": 20
   }
   // Resposta esperada
   {
     "id": 1
   }
2. Obter transação por ID (/api/InventoryTransactions{id})
   ```js
   {
     "id": 1,
     "productId": 1,
     "type": "Entry",
     "quantity": 20,
     "cost": 1000.00,
     "transactionDate": "2024-04-27T16:45:00Z",
     "product": null
   }
   ```
3. Obter transações de hoje (/api/InventoryTransactions/today)
   ```js
   [
     {
       "id": 1,
       "productId": 1,
       "type": "Entry",
       "quantity": 20,
       "cost": 1000.00,
       "transactionDate": "2024-04-27T16:45:00Z",
       "product": null
     },
     {
       "id": 2,
       "productId": 1,
       "type": "Exit",
       "quantity": 10,
       "cost": 500.00,
       "transactionDate": "2024-04-27T17:00:00Z",
       "product": null
      }
   ]
   ```
