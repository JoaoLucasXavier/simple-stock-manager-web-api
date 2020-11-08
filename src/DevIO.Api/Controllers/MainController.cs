using System;
using System.Linq;
using DevIO.Business.Interfaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

/*
-> Classe responsável por implementar uma "Custom Response" para as classes filhas usando o "fluentvalidation" da camada de
    Bussiness
-> Vamos validar:
    - Validação de notificações de erros
    - Validação de modelstate (Preenchimento correto das viewmodels)
    - Validação da operação de negócios
*/

namespace DevIO.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        // Declaramos um tipo para 'INotificador'
        private readonly INotificador _notificador;

        // IUser: Interface que dar acesso ao usuário logado no sistema (obs: Essa classe é populada através da extension 'AspNetUser')
        public readonly IUser _appUser;

        // Propriedades que recebe dados do usuário logado
        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }

        // Injeção de dependência para 'INotificador'
        protected MainController(
            INotificador notificador,
            IUser appUser
        )
        {
            _notificador = notificador;
            _appUser = appUser;
            // Retornamos o id e username caso o usuário esteja autênticado
            if (_appUser.IsAuthenticated())
            {
                UsuarioId = _appUser.GetUserId();
                UsuarioAutenticado = true;
            }
        }

        // Método que retorna um true quando a operação for válida
        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        // "Custom Response" de notificações da camada de 'Business' para todas as controllers
        protected ActionResult CustomResponse(object result = null)
        {
            // Retorno caso não existe notificações (On()com objeto anônimo)
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            // Retorna caso existe notificações (BadRequest() com objeto anônimo)
            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        // "Custom Response" de notificações obtidas da 'modelState' para todas as controllers
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse(); // Retorna a outra "CustomResponse()"
        }

        // Trabalha os erros recebidos na 'modelState'
        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                // Garantimos que obtemos os erros também de 'Exceptions'
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        // Método responsável por notificar erros obtidos do Notificador 'Camadas Bussiness'
        protected void NotificarErro(string mensagem)
        {
            // Cria uma nova notificação com sua mensagem.
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
