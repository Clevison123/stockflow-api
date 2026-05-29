# Fluxos do Sistema

---

# Fluxo — Login

1. Usuário informa email e senha
2. Sistema valida credenciais
3. JWT é gerado
4. Usuário recebe acesso ao sistema

---

# Fluxo — Cadastro de Produto

1. Usuário acessa módulo de produtos
2. Sistema valida permissões
3. Usuário informa:
   - nome
   - SKU
   - categoria
   - país origem
   - garantia
4. Sistema valida dados
5. Produto é salvo
6. Auditoria é registrada

---

# Fluxo — Entrada de Estoque

1. Usuário seleciona produto
2. Usuário informa quantidade
3. Sistema valida produto
4. Estoque é atualizado
5. Movimentação é criada
6. Auditoria é registrada

---

# Fluxo — Saída de Estoque

1. Usuário seleciona produto
2. Usuário informa quantidade
3. Sistema verifica estoque disponível
4. Sistema impede estoque negativo
5. Estoque é atualizado
6. Movimentação é criada
7. Auditoria é registrada

---

# Fluxo — Ajuste de Estoque

1. Usuário solicita ajuste
2. Sistema envia para aprovação
3. Gerente aprova ou rejeita
4. Estoque é atualizado
5. Auditoria é registrada

---

# Fluxo — Devolução Produto Defeituoso

1. Produto retorna ao estoque
2. Usuário registra devolução
3. Sistema identifica serial number
4. Produto é marcado como defeituoso
5. Auditoria é registrada

---

# Fluxo — Inventário

1. Usuário inicia inventário
2. Produtos são conferidos
3. Sistema compara quantidade física
4. Divergências são registradas
5. Ajustes podem ser solicitados