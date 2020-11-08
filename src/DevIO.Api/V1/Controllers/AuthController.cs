using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : MainController
    {
        // UserManager: Class do identity responsável por criar o usuário e fazer outras manipulações
        private readonly UserManager<IdentityUser> _userManager;

        // SignInManager: Class do identity responsável por fazer a autenticação do usuário
        private readonly SignInManager<IdentityUser> _signInManager;

        // Recebemos as configurações do JWT
        private readonly AppSettings _appSettings;

        private readonly ILogger _logger;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<AppSettings> appSettings,
            ILogger<AuthController> logger,
            INotificador notificador,
            IUser user
        ) : base(notificador, user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            // Cria obj identity user
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };
            // Criar usuário
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            // Se o resultado for bem sucedido fazemos o login do usuário
            if (result.Succeeded)
            {
                // Logging que informa estado de login do usuário
                _logger.LogInformation("Usuario "+ user.Email +" logado com sucesso");
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GerarJwt(user.Email));
            }
            // Notifica cada erro encontrado caso o resultado não for bem sucedido
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }
            return CustomResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            // Efetua login com email e senha
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            // Se o resultado for bem sucedido
            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(loginUser.Email));
            }
            // Se fez mais de 5 tentativas de login e foi travado | PasswordSignInAsync(bool lockoutOnFailure)
            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas.");
                return CustomResponse(loginUser);
            }
            // Caso não tenha entrado no login por algum motivo e não esteja travado
            NotificarErro("Usuário ou Senha incorretos.");
            return CustomResponse(loginUser);
        }

        private async Task<LoginResponseViewModel> GerarJwt(string email)
        {
            /* ADICIONAR CLAIMS AO TOKE JWT */
            // Obtemos o user, claims e roles do usuário baseado no 'email' recebido no parâmetro
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            // Passamos as claims do token JWT
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            // Adiciona as ROLES na coleção de CLAIMS | OBS: Permitido pois roles e claims são parecidos
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }
            // Converte as CLAIMS de: System.Collections.Generic.IList<Claim> para: identityClaims
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            /* GERAR TOKEN JWT */
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims, // Passamos as CLAIMS para o token
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            var encodedToken = tokenHandler.WriteToken(token);

            /* MECANISMO PARA RETORNAR INFORMAÇÕES ALÉM DO JWT TOKEN
                ADICIONAMOS INFORMAÇÕES DE USUÁRIO COMO: EMAIL ETC PARA O CLIENT QUE VAI CONSUMIR A API */
            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken, // Token
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds, //Exiração do token
                // Informações do usuário com suas respectivas CLAIMS
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };
            return response;
        }

        // Método que prover data num formato 'ToUnixEpochDate' para gerar algumas claims JWT no método GerarJwt()
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


        /* ---------------------------- */
        /* ABAIXO EXEMPLOS PARA ESTUDO */
        /* -------------------------- */

        // Método que gera um token JWT para actions de Login(), Registrar()
        // Para usar o método basta retornar esse método como parâmetro | Ex: return CustomResponse(GerarJwtExemplo());
        private string GerarJwtExemplo()
        {
            // Gerar token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        // Exemplo iteração com usuário sem uso da extension 'Interfaces\IUser.cs'
        private void UsuarioLogado()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
            }
        }

        // Exemplo iteração com usuário COM O USO DA EXTENSION 'Interfaces\IUser.cs'
        private void UsuarioLogadoExension()
        {
            if (UsuarioAutenticado)
            {
                var userId = _appUser.GetUserId();
                var userName = _appUser.Name;
                var userEmail = _appUser.GetUserEmail();
            }
        }
    }
}
