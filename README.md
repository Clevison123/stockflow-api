# StockFlow

Sistema de gestão para distribuidoras de produtos eletrônicos.

---

## Sobre o Projeto

O StockFlow foi desenvolvido para controlar todo o fluxo operacional de uma distribuidora de eletrônicos, desde o recebimento de mercadorias vindas de fornecedores internacionais até a venda, entrega e rastreabilidade dos produtos.

O sistema foi projetado para resolver problemas comuns encontrados em operações de estoque e logística, como divergências de inventário, falta de auditoria e ausência de rastreabilidade dos produtos.

---

## Problemas Resolvidos

* Divergências de estoque
* Produtos sem rastreabilidade
* Falta de auditoria
* Inventários demorados
* Dificuldade para localizar responsáveis por alterações
* Controle manual de recebimentos
* Controle limitado de serial numbers
* Baixa visibilidade das movimentações

---

## Principais Funcionalidades

### Autenticação e Segurança

* Login com JWT
* Refresh Tokens
* Controle de permissões por perfil
* Auditoria de acessos

### Gestão de Produtos

* Cadastro de produtos
* Categorias
* Controle de garantia
* País de origem
* Código de barras

### Controle de Estoque

* Entrada de estoque
* Saída de estoque
* Ajustes
* Inventários
* Histórico de movimentações

### Rastreabilidade

* Controle de Serial Number
* Controle por item individual
* Histórico de movimentações
* Auditoria completa

### Recebimento de Mercadorias

* Controle de fornecedores
* Registro de remessas
* Conferência de recebimento
* Controle de ocorrências

### Vendas e Clientes

* Cadastro de clientes
* Gestão de pedidos
* Histórico de compras

### Logística

* Controle de entregas
* Registro de ocorrências de transporte

### Qualidade

* Controle de produtos defeituosos
* Reclamações de clientes
* Reclamações para fornecedores

---

## Tecnologias

### Backend

* C#
* .NET 8
* ASP.NET Core Web API

### Banco de Dados

* PostgreSQL

### Segurança

* JWT Authentication
* Refresh Tokens

### Persistência

* Entity Framework Core

### Validação

* FluentValidation

### Documentação

* Swagger / OpenAPI

### Controle de Versão

* Git
* GitHub

---

## Arquitetura

O projeto segue os princípios da Clean Architecture.

```txt
src
│
├── Domain
├── Application
├── Infrastructure
└── Presentation
```

### Domain

Entidades, enums e regras de negócio.

### Application

Casos de uso, DTOs, Services, Validators e Exceptions.

### Infrastructure

Banco de dados, autenticação e serviços externos.

### Presentation

Controllers, endpoints e middlewares.

---

## Documentação

A documentação do projeto está disponível na pasta:

```txt
docs/
```

Contendo:

* Requirements
* Business Rules
* Flows
* Architecture
* Roadmap

---

## Status do Projeto

Em desenvolvimento

Atualmente em fase de modelagem do domínio e implementação da infraestrutura base.

---

## Autor

José Clévison
