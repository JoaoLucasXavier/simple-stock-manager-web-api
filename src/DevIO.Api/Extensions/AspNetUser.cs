using System;
using System.Collections.Generic;
using System.Security.Claims;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DevIO.Api.Extensions
{
    // Implementa o 'IUser' da camada de Bussiness
    public class AspNetUser : IUser
    {
        // IHttpContextAccessor: Interface do ASP NET CORE que permite o acesso ao HTTP CONTEXT de qualquer lugar
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        // Retorna o username do usuário
        public string Name => _accessor.HttpContext.User.Identity.Name;

        // Retorna o id do usuário através da extension 'ClaimsPrincipalExtensions'
        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        // Retorna o email do usuário através da extension 'ClaimsPrincipalExtensions'
        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        // Retorna se o usuário está autenticado
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        // Retorna um boolean de acordo como pertencimento do usuário a role
        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        // Retorna as claims do usuário
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }

    // Extension que obtém dados do usuário através de sua claim
    public static class ClaimsPrincipalExtensions
    {
        // method que obtém da claim do usuário seu id
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        // method que obtém da claim do usuário seu email
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
