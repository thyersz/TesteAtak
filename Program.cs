using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TesteAtak.Settings;
using TesteAtak.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços necessários ao container
builder.Services.AddControllersWithViews();

// Configura o banco de dados em memória
builder.Services.AddDbContext<TesteAtak.Data.ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TesteAtakDb"));

// Configurações do JWT (Token de Autenticação)
var jwtSettings = new JwtSettings 
{
    Secret = "your-super-secret-key-with-at-least-32-characters",
    ExpirationHours = 24,
    Issuer = "TesteAtak",
    Audience = "TesteAtak"
};
builder.Services.AddSingleton(jwtSettings);

// Configura a autenticação usando JWT e Cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";      // Página de login
    options.LogoutPath = "/Account/Logout";    // Página de logout
    options.AccessDeniedPath = "/Account/Login"; // Redirecionamento quando acesso é negado
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configuração da sessão do usuário
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);    // Tempo limite da sessão: 30 minutos
    options.Cookie.HttpOnly = true;                     // Cookie acessível apenas pelo servidor
    options.Cookie.IsEssential = true;                 // Cookie essencial para o funcionamento
});

// Registro dos serviços na injeção de dependência
builder.Services.AddScoped<IAuthService, AuthService>();                // Serviço de autenticação
builder.Services.AddScoped<IDataGeneratorService, DataGeneratorService>();  // Serviço de geração de dados
builder.Services.AddScoped<IEmailService, EmailService>();                  // Serviço de email

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Cria dados iniciais no banco de dados
using (var scope = app.Services.CreateScope())
{
    // Obtém as instâncias dos serviços necessários
    var context = scope.ServiceProvider.GetRequiredService<TesteAtak.Data.ApplicationDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    
    // Se não houver usuários cadastrados, cria os usuários padrão
    if (!context.Usuarios.Any())
    {
        context.Usuarios.AddRange(
            new TesteAtak.Models.Usuario 
            { 
                Nome = "João Silva",                    // Nome do primeiro usuário
                Email = "joao@email.com",              // Email para login
                Telefone = "(11) 98765-4321",          // Telefone de contato
                PasswordHash = authService.HashPassword("123456")  // Senha: 123456
            },
            new TesteAtak.Models.Usuario 
            { 
                Nome = "Maria Santos", 
                Email = "maria@email.com", 
                Telefone = "(11) 91234-5678",
                PasswordHash = authService.HashPassword("123456")
            }
        );
        context.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();