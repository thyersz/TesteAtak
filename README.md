# TesteAtak - Sistema de Gerenciamento de Dados

## 📋 Sobre o Projeto
Sistema web desenvolvido em ASP.NET Core MVC que oferece funcionalidades de gerenciamento de usuários, geração de dados aleatórios e exportação para Excel com envio automático por email.

## ✨ Funcionalidades
- 👥 Cadastro e gerenciamento de usuários
- 🔐 Sistema de login seguro com JWT + Cookies
- 📊 Gerador de dados aleatórios para testes
- 📑 Exportação de dados para Excel
- 📧 Envio automático por email

## 🛠 Tecnologias Utilizadas
- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core (Banco em Memória)
- JWT para autenticação segura
- Bootstrap para interface responsiva
- EPPlus para criação de planilhas Excel
- Bogus para geração de dados realistas
- MailKit para sistema de emails

## 🚀 Como Iniciar
1. Clone o projeto
   ```
   git clone https://github.com/thyersz/TesteAtak.git
   ```

2. Acesse a pasta do projeto
   ```
   cd TesteAtak
   ```

3. Inicie a aplicação
   ```
   dotnet run
   ```

4. Abra no navegador
   ```
   https://localhost:5286
   ```

## 📝 Usuários para Teste
O sistema já vem com dois usuários pré-cadastrados:

- **Usuário 1:**
  - Email: joao@email.com
  - Senha: 123456

- **Usuário 2:**
  - Email: maria@email.com
  - Senha: 123456

## 🔍 Organização das Pastas
```
TesteAtak/
├── Controllers/      # Controladores do MVC
├── Models/          # Modelos de dados
├── Services/        # Serviços do sistema
├── Views/           # Interfaces do usuário
└── wwwroot/        # Arquivos públicos
```

## 📊 Gerador de Dados
- Cria dados brasileiros realistas usando Bogus
- Gera planilhas Excel com formatação profissional
- Envia automaticamente por email
- Tipos de dados gerados:
  - Nomes completos
  - Endereços de email
  - Números de telefone
  - Nomes de empresas
  - Cargos profissionais
  - Departamentos
  - Salários em reais
  - Datas de contratação
  - Endereços completos

