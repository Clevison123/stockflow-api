# Regras de Negócio — StockFlow

---

# Estoque

## RN-001

O estoque não pode ficar negativo.

## RN-002

Toda entrada de estoque deve gerar uma movimentação.

## RN-003

Toda saída de estoque deve gerar uma movimentação.

## RN-004

Toda movimentação deve possuir responsável.

## RN-005

Movimentações não podem ser excluídas.

## RN-006

Correções devem ser realizadas através de estorno ou ajuste.

## RN-007

Ajustes de estoque devem ser aprovados por um gerente ou superior.

## RN-008

Inventários devem registrar divergências encontradas.

---

# Produtos

## RN-009

Produtos com Serial Number devem possuir identificação única.

## RN-010

Não pode existir mais de um ProductItem com o mesmo Serial Number.

## RN-011

Produtos defeituosos não podem ser vendidos.

## RN-012

Produtos em quarentena não podem ser movimentados.

## RN-013

Produtos devem pertencer a uma categoria válida.

## RN-014

Produtos devem possuir fornecedor associado.

---

# Recebimento de Mercadorias

## RN-015

Toda remessa deve possuir fornecedor associado.

## RN-016

Toda remessa deve possuir número identificador.

## RN-017

Toda remessa deve passar por conferência.

## RN-018

Divergências encontradas durante a conferência devem gerar ocorrência.

## RN-019

Produtos recebidos devem passar por inspeção quando aplicável.

## RN-020

Produtos reprovados na inspeção não podem entrar em estoque disponível.

## RN-021

Produtos aprovados devem gerar movimentação de entrada.

---

# Pedidos

## RN-022

Pedidos devem possuir cliente associado.

## RN-023

Pedidos devem possuir ao menos um item.

## RN-024

Não é permitido faturar pedido sem estoque disponível.

## RN-025

Pedido cancelado não pode ser expedido.

## RN-026

Pedido entregue não pode retornar para status anterior.

---

# Entregas

## RN-027

Toda entrega deve possuir pedido associado.

## RN-028

Toda entrega deve possuir endereço válido.

## RN-029

Problemas durante a entrega devem gerar DeliveryIssue.

## RN-030

Entregas concluídas devem registrar data e responsável.

---

# Qualidade

## RN-031

Problemas identificados durante inspeções devem gerar QualityIssue.

## RN-032

Reclamações de clientes devem gerar CustomerClaim.

## RN-033

Problemas relacionados ao fornecedor devem gerar SupplierClaim.

## RN-034

Itens defeituosos devem ser segregados do estoque disponível.

---

# Usuários e Segurança

## RN-035

Usuários inativos não podem acessar o sistema.

## RN-036

Toda ação deve estar associada a um usuário autenticado.

## RN-037

Permissões devem respeitar o perfil do usuário.

## RN-038

Senhas devem ser armazenadas criptografadas.

## RN-039

Tokens expirados não podem ser reutilizados.

---

# Auditoria

## RN-040

Toda alteração relevante deve gerar registro de auditoria.

## RN-041

Auditorias não podem ser removidas.

## RN-042

Auditorias devem armazenar usuário, ação, data e entidade afetada.

## RN-043

Auditorias devem permitir rastrear alterações realizadas no sistema.

---

# Clientes

## RN-044

Clientes devem possuir identificação única.

## RN-045

Clientes inativos não podem realizar novos pedidos.

## RN-046

Todo pedido deve manter vínculo com seu cliente original.

---

# Garantia

## RN-047

Itens com garantia devem possuir data de vencimento registrada.

## RN-048

Reclamações em garantia devem estar vinculadas ao item vendido.

## RN-049

A garantia deve ser rastreável através do Serial Number.
