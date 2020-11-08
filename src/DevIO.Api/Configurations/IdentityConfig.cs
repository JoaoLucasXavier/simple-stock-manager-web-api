using System.Text;
using DevIO.Api.Data;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;




namespace DevIO.Api.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Configuração do context do identity
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration
                .GetConnectionString("DefaultConnection")));

            // Adicionar o identity na aplicação
            // IdentityUser: Classe default de user do identity
            // IdentityRole: Classe default de role do identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders();

            /* JWT */
            // Obtém e configura no ASP NET CORE a sessão de configuração 'AppSettings' do 'appsettings.json'
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            // Obtemos as informações DE configuração para a implementação do JWT
            var appSettings = appSettingsSection.Get<AppSettings>();
            // Faz o encoding da key secret obtida do 'AppSettings'
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            // Adicina e configura a autenticação JWT
            services.AddAuthentication(x =>
            {
                // DefaultAuthenticateScheme: Faz com que seja serado um token toda vez que for autenticar alguém
                // DefaultChallengeScheme: Verifica se a pessoa está autenticada baseada no token
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                // RequireHttpsMetadata: Https: true | Http: false
                x.RequireHttpsMetadata = true;
                // SaveToken: Se o token deve ser guardado no 'http authentication properties' caso a autenticação seja feita com sucesso
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Valida se quem está emitindo o token a partir da key
                    ValidateIssuerSigningKey = true,
                    // Transforma a key ASCII em uma key criptografada
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // Valida o Issuer/emitente conforme o nome
                    ValidateIssuer = true,
                    // Valida se o token é valido baseado na Audience | Ex: https://localhost
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    // Valida o token baseado o Issues/Emitente
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }
    }
}
