using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// ClaimsAuthorize [ClaimsAuthorize('AuthorizedName')] | NOME DO ATTRIBUTE QUE USAREMOS NA CONTROLLER PARA AUTORIZAÇÃO BASEADO EM CLAIMS

namespace DevIO.Api.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            // Verifica se o usuário está autenticado
            // Verifica se o usuário possui alguma claim de acordo com a passada via 'Attribute' na controller
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }

    }

    // Class que reescreve a forma default de gerar autorização e autenticação
    // ClaimsAuthorize [ClaimsAuthorize('AuthorizedName')] | NOME DO ATTRIBUTE QUE USAREMOS NA CONTROLLER
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        // Passamos uma 'base class' nova (RequisitoClaimFilter) onde reescrevemos o filter default de autorização de 'IAuthorizationFilter'
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    // Filtro de autorização que valida o usuário
    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Verifica se o usuário está autenticado
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
            // Verifica se o usuário possui autorização baseado na CLAIM
            if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
