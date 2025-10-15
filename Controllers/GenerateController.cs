using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using TesteAtak.Services;
using System.Drawing;

namespace TesteAtak.Controllers
{
    // Controlador responsável pela página de geração de dados - Necessário estar logado para acessar
    [Authorize]
    public class GenerateController : Controller
    {
        // Serviços necessários para o funcionamento
        private readonly IDataGeneratorService _dataGeneratorService;  // Serviço que gera os dados aleatórios
        private readonly IEmailService _emailService;                  // Serviço de envio de emails
        private readonly IWebHostEnvironment _webHostEnvironment;      // Serviço para manipulação de arquivos

        public GenerateController(IDataGeneratorService dataGeneratorService, IEmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _dataGeneratorService = dataGeneratorService;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            // Licença não comercial do EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método que gera a planilha Excel e envia por email se solicitado
        [HttpPost]
        public async Task<IActionResult> GenerateExcel(int quantidade = 100, bool sendEmail = false)
        {
            // Validação da quantidade de registros (mínimo 10, máximo 1000)
            if (quantidade < 10 || quantidade > 1000)
            {
                TempData["Error"] = "A quantidade de registros deve ser entre 10 e 1000.";
                return RedirectToAction("Index");
            }

            // Solicita ao serviço a geração dos dados aleatórios
            var dados = _dataGeneratorService.GenerateRandomData(quantidade);

            // Inicia a criação da planilha Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Dados Gerados");

                // Estilo do cabeçalho
                var headerStyle = worksheet.Cells["A1:L1"].Style;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                headerStyle.Font.Color.SetColor(Color.White);
                headerStyle.Font.Bold = true;

                // Definir cabeçalhos
                worksheet.Cells["A1"].Value = "Nome";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Telefone";
                worksheet.Cells["D1"].Value = "Empresa";
                worksheet.Cells["E1"].Value = "Cargo";
                worksheet.Cells["F1"].Value = "Departamento";
                worksheet.Cells["G1"].Value = "Salário";
                worksheet.Cells["H1"].Value = "Data Contratação";
                worksheet.Cells["I1"].Value = "Endereço";
                worksheet.Cells["J1"].Value = "Cidade";
                worksheet.Cells["K1"].Value = "Estado";
                worksheet.Cells["L1"].Value = "CEP";

                // Preencher dados
                int row = 2;
                foreach (var dado in dados)
                {
                    worksheet.Cells[row, 1].Value = dado.Nome;
                    worksheet.Cells[row, 2].Value = dado.Email;
                    worksheet.Cells[row, 3].Value = dado.Telefone;
                    worksheet.Cells[row, 4].Value = dado.Empresa;
                    worksheet.Cells[row, 5].Value = dado.Cargo;
                    worksheet.Cells[row, 6].Value = dado.Departamento;
                    worksheet.Cells[row, 7].Value = dado.Salario;
                    worksheet.Cells[row, 8].Value = dado.DataContratacao;
                    worksheet.Cells[row, 9].Value = dado.Endereco;
                    worksheet.Cells[row, 10].Value = dado.Cidade;
                    worksheet.Cells[row, 11].Value = dado.Estado;
                    worksheet.Cells[row, 12].Value = dado.CEP;
                    row++;
                }

                // Autofit colunas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Formatar coluna de salário
                worksheet.Column(7).Style.Numberformat.Format = "R$ #,##0.00";
                // Formatar coluna de data
                worksheet.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";

                var content = package.GetAsByteArray();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"DadosGerados_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sendEmail)
                {
                    // Salvar o arquivo temporariamente
                    var tempPath = Path.Combine(_webHostEnvironment.WebRootPath, "temp");
                    Directory.CreateDirectory(tempPath);
                    var filePath = Path.Combine(tempPath, fileName);
                    
                    await System.IO.File.WriteAllBytesAsync(filePath, content);
                    
                    await _emailService.SendExcelFileAsync(filePath);
                    // Excluir o arquivo temporário após enviar
                    System.IO.File.Delete(filePath);
                }

                return File(content, contentType, fileName);
            }
        }
    }
}
