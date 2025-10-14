using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace TesteAtak.Services
{
    // Interface que define os métodos do serviço de email
    public interface IEmailService
    {
        Task SendExcelFileAsync(string excelFilePath, string subject = "Excel Report");
    }

    // Classe responsável pelo envio de emails do sistema
    public class EmailService : IEmailService
    {
        // Configurações da aplicação (armazena dados de conexão do email)
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendExcelFileAsync(string excelFilePath, string subject = "Excel Report")
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("TesteAtak System", _configuration["EmailSettings:FromEmail"]));
            email.To.Add(new MailboxAddress("Sergio Junior", "sergio.junior@atak.com.br"));
            email.Subject = "[TesteAtak] - Dados Gerados";

            var builder = new BodyBuilder();
            builder.TextBody = $@"Prezado Sergio Junior,

Este é um projeto desenvolvido em ASP.NET Core MVC que implementa um sistema de gerenciamento de usuários com autenticação JWT, geração de dados aleatórios e exportação para Excel.

Principais funcionalidades:
- CRUD completo de usuários
- Autenticação com JWT e cookies
- Geração de dados aleatórios com Bogus
- Exportação para Excel usando EPPlus
- Envio automático de relatórios por email

Link do repositório: https://github.com/thyersz/TesteAtak.git

Segue em anexo o arquivo Excel com os dados gerados conforme solicitado.

Atenciosamente,
Sistema TesteAtak";

            // Anexar o arquivo Excel
            using var stream = new FileStream(excelFilePath, FileMode.Open);
            builder.Attachments.Add("report.xlsx", stream);

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:AppPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}