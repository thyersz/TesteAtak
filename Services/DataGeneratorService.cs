using Bogus;
using TesteAtak.Models;

namespace TesteAtak.Services
{
    // Interface do serviço gerador de dados aleatórios
    public interface IDataGeneratorService
    {
        List<RandomData> GenerateRandomData(int count);
    }

    // Classe que implementa a geração de dados aleatórios usando a biblioteca Bogus
    public class DataGeneratorService : IDataGeneratorService
    {
        // Método principal que gera uma lista de dados fictícios
        public List<RandomData> GenerateRandomData(int count)
        {
            // Configuração do gerador de dados em português brasileiro
            var faker = new Faker<RandomData>("pt_BR")
                // Informações Pessoais
                .RuleFor(d => d.Nome, f => f.Name.FullName())                                     // Gera um nome completo
                .RuleFor(d => d.Email, (f, d) => f.Internet.Email(d.Nome.ToLower()))             // Cria um email com base no nome
                .RuleFor(d => d.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"))           // Gera um número de celular

                // Informações Profissionais
                .RuleFor(d => d.Empresa, f => f.Company.CompanyName())                           // Gera um nome de empresa
                .RuleFor(d => d.Cargo, f => f.Name.JobTitle())                                   // Define um cargo
                .RuleFor(d => d.Departamento, f => f.Commerce.Department())                       // Define um departamento
                .RuleFor(d => d.Salario, f => Math.Round(f.Random.Decimal(3000, 15000), 2))      // Gera um salário entre R$ 3.000 e R$ 15.000
                .RuleFor(d => d.DataContratacao, f => f.Date.Past(3))                            // Gera uma data nos últimos 3 anos

                // Informações de Endereço
                .RuleFor(d => d.Endereco, f => f.Address.StreetAddress())                        // Gera endereço com rua e número
                .RuleFor(d => d.Cidade, f => f.Address.City())                                   // Gera nome da cidade
                .RuleFor(d => d.Estado, f => f.Address.StateAbbr())                              // Gera a sigla do estado
                .RuleFor(d => d.CEP, f => f.Address.ZipCode("#####-###"));

            return faker.Generate(count);
        }
    }
}
