# Arquitetura do Sistema

## Visão Geral

O StockFlow é um sistema de gestão para distribuidoras de produtos eletrônicos.

O projeto foi desenvolvido utilizando os princípios da Clean Architecture para garantir:

* baixo acoplamento;
* alta coesão;
* facilidade de manutenção;
* escalabilidade;
* testabilidade;
* separação clara de responsabilidades.

---

# Estrutura da Solução

```txt
StockFlow
│
├── src
│   │
│   ├── Domain
│   ├── Application
│   ├── Infrastructure
│   └── Presentation
│
├── docs
│
└── README.md
```

---

# Camadas

## Domain

Camada central do sistema.

Responsável por representar as regras de negócio e os conceitos do domínio da distribuidora.

Não possui dependência de nenhuma outra camada.

### Responsabilidades

* Entidades
* Enums
* Constantes
* Regras de domínio

### Exemplos

```txt
Product
ProductItem
Customer
Supplier
SalesOrder
StockMovement
AuditLog
```

---

## Application

Responsável pelos casos de uso da aplicação.

Coordena as regras de negócio e a comunicação entre as camadas.

### Responsabilidades

* DTOs
* Services
* Interfaces
* Validators
* Exceptions
* Casos de Uso

### Exemplos

```txt
ProductService
SalesOrderService
StockMovementService
AuthService
```

---

## Infrastructure

Responsável pela implementação de recursos externos.

Contém toda a comunicação com banco de dados, autenticação e serviços externos.

### Responsabilidades

* Entity Framework Core
* PostgreSQL
* JWT
* Refresh Tokens
* Exportação Excel
* Integrações futuras

### Exemplos

```txt
AppDbContext
Migrations
JwtService
ExcelExportService
```

---

## Presentation

Responsável pela exposição da API.

Recebe as requisições dos clientes e devolve as respostas.

### Responsabilidades

* Controllers
* Endpoints
* Middlewares
* Responses
* Tratamento Global de Exceções

### Exemplos

```txt
ProductsController
AuthController
UsersController
ReportsController
```

---

# Fluxo de Dependências

```txt
Presentation
        ↓
Application
        ↓
Domain

Infrastructure
        ↓
Application
```

O Domain não depende de nenhuma camada.

O Application depende apenas do Domain.

O Infrastructure implementa contratos definidos pelo Application.

O Presentation utiliza os casos de uso definidos pelo Application.

---

# Princípios Utilizados

## Separação de Responsabilidades

Cada camada possui uma única responsabilidade.

---

## Inversão de Dependência

As regras de negócio não dependem de detalhes de infraestrutura.

---

## Encapsulamento

As regras do negócio permanecem protegidas dentro do domínio.

---

## Escalabilidade

A arquitetura permite adicionar novos módulos sem impactar os módulos existentes.

Exemplos futuros:

* Portal do Cliente
* Aplicativo Mobile
* Integração ERP
* Integração Transportadoras
* Integração Fiscal

---

# Objetivo Arquitetural

Garantir que o sistema possa crescer de um simples controle de estoque para uma plataforma completa de gestão de distribuidoras, mantendo organização, manutenção simples e evolução contínua.
