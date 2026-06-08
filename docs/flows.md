# Fluxo — Recebimento de Mercadoria

## Ator Principal

* Estoquista
* Supervisor de Galpão

## Pré-condições

* Fornecedor cadastrado
* Produtos cadastrados
* Remessa (InboundShipment) registrada
* Container identificado

## Passos

### Etapa 1 — Recebimento no Porto

1. Mercadoria chega ao porto.
2. Conferente verifica número do container.
3. Conferente verifica documentação da remessa.
4. Conferente compara produtos recebidos com a documentação enviada pelo fornecedor.
5. Conferente verifica:

   * quantidade;
   * modelos;
   * cores;
   * tamanhos;
   * lotes (quando aplicável).
6. Caso existam divergências, o sistema registra uma ocorrência.
7. Caso a remessa seja aprovada, o sistema altera o status para **ApprovedAtPort**.

### Etapa 2 — Transporte até o Galpão

8. Mercadoria é liberada para transporte.
9. Sistema registra data de saída do porto.
10. Sistema altera o status para **InTransitToWarehouse**.

### Etapa 3 — Recebimento no Galpão

11. Mercadoria chega ao galpão.
12. Estoquista realiza nova conferência.
13. Sistema compara a conferência do galpão com a conferência realizada no porto.
14. Caso existam divergências, o sistema registra uma ocorrência.
15. Sistema altera o status para **WarehouseReceiving**.

### Etapa 4 — Inspeção de Qualidade

16. Produtos são inspecionados.
17. São realizados testes de funcionamento quando aplicável.
18. Produtos defeituosos geram um QualityIssue.
19. Produtos com problemas podem gerar um SupplierClaim.
20. Produtos aprovados seguem para armazenamento.

### Etapa 5 — Entrada em Estoque

21. Sistema registra os ProductItems.
22. Sistema registra Serial Numbers quando aplicável.
23. Sistema cria movimentação de entrada.
24. Sistema atualiza o estoque.
25. Sistema registra auditoria.
26. Sistema altera o status da remessa para **Completed**.

## Pós-condições

* Produtos disponíveis para venda.
* Estoque atualizado.
* Movimentações registradas.
* Auditoria registrada.
* Produtos defeituosos segregados para análise.
