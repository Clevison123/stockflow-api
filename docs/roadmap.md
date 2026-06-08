# Roadmap — StockFlow

---

# FASE 1 — Análise e Planejamento

## Negócio

* [x] Objetivo do sistema
* [x] Identificação dos usuários
* [x] Levantamento dos problemas atuais
* [x] Fluxos principais
* [x] Regras de negócio

## Documentação

* [x] README
* [x] Requirements
* [x] Business Rules
* [x] Flows
* [x] Roadmap

---

# FASE 2 — Modelagem do Domínio

## Domain

* [x] BaseEntity
* [x] User
* [x] RefreshToken
* [x] AuditLog

### Catálogo

* [x] Product
* [x] ProductItem
* [x] Category

### Fornecedores

* [x] Supplier
* [x] InboundShipment

### Estoque

* [x] StockMovement

### Clientes

* [x] Customer

### Pedidos

* [x] SalesOrder
* [x] SalesOrderItem

### Logística

* [x] Delivery
* [x] DeliveryIssue

### Qualidade

* [x] QualityIssue
* [x] SupplierClaim
* [x] CustomerClaim

### Enums

* [x] UserRole
* [x] ProductItemStatus
* [x] MovementType
* [x] SalesOrderStatus
* [x] InboundShipmentStatus

---

# FASE 3 — Infraestrutura Base

## Projeto

* [ ] Ajustar estrutura Clean Architecture
* [ ] Organizar namespaces
* [ ] Configurar Dependency Injection

## Banco de Dados

* [ ] Configurar PostgreSQL
* [ ] Configurar Entity Framework Core
* [ ] Configurar Migrations
* [ ] Configurar Seed Inicial

## API

* [ ] Configurar Swagger
* [ ] Configurar Versionamento
* [ ] Configurar Exception Middleware
* [ ] Configurar Logging

---

# FASE 4 — Autenticação e Segurança

## Autenticação

* [ ] Login
* [ ] Logout
* [ ] JWT
* [ ] Refresh Token

## Segurança

* [ ] Controle de Permissões
* [ ] Controle de Roles
* [ ] Password Hashing
* [ ] Recuperação de Senha

## Auditoria

* [ ] Auditoria de Login
* [ ] Auditoria de Alterações

---

# FASE 5 — Catálogo de Produtos

## Categorias

* [ ] CRUD Categorias

## Produtos

* [ ] CRUD Produtos
* [ ] Controle de Garantia
* [ ] Controle de País de Origem
* [ ] Código de Barras
* [ ] Estoque Mínimo

## Product Items

* [ ] Controle de Serial Number
* [ ] Controle de Status
* [ ] Rastreabilidade

---

# FASE 6 — Recebimento de Mercadorias

## Fornecedores

* [ ] CRUD Fornecedores

## Remessas

* [ ] Registro de Remessas
* [ ] Registro de Containers
* [ ] Conferência de Recebimento

## Qualidade

* [ ] Registro de QualityIssue
* [ ] Registro de SupplierClaim

---

# FASE 7 — Controle de Estoque

## Movimentações

* [ ] Entrada
* [ ] Saída
* [ ] Ajuste

## Inventário

* [ ] Inventário Manual
* [ ] Divergências
* [ ] Aprovação de Ajustes

## Regras

* [ ] Bloqueio de Estoque Negativo
* [ ] Auditoria Obrigatória

---

# FASE 8 — Clientes e Vendas

## Clientes

* [ ] CRUD Clientes

## Pedidos

* [ ] Criar Pedido
* [ ] Cancelar Pedido
* [ ] Alterar Status
* [ ] Histórico

## Itens

* [ ] Controle de Itens do Pedido
* [ ] Reserva de Estoque

---

# FASE 9 — Logística

## Entregas

* [ ] Criar Entrega
* [ ] Acompanhar Entrega
* [ ] Confirmar Entrega

## Ocorrências

* [ ] DeliveryIssue
* [ ] Tratamento de Problemas

---

# FASE 10 — Qualidade e Pós-Venda

## Reclamações

* [ ] CustomerClaim
* [ ] SupplierClaim

## Garantias

* [ ] Controle de Garantia
* [ ] Trocas
* [ ] Reposições

---

# FASE 11 — Relatórios e Dashboard

## Dashboard

* [ ] Resumo Geral
* [ ] Produtos Baixo Estoque
* [ ] Pedidos Pendentes
* [ ] Entregas em Andamento

## Relatórios

* [ ] Estoque
* [ ] Movimentações
* [ ] Vendas
* [ ] Entregas
* [ ] Auditoria

## Exportação

* [ ] Excel
* [ ] CSV

---

# FASE 12 — Recursos Avançados

## RH

* [ ] Gestão de Funcionários

## Portal do Cliente

* [ ] Login Cliente
* [ ] Catálogo Online
* [ ] Histórico de Pedidos
* [ ] Acompanhamento de Entregas
* [ ] Abertura de Reclamações

## Futuro

* [ ] Aplicativo Mobile
* [ ] Integração ERP
* [ ] Integração Transportadoras
* [ ] Integração Fiscal
