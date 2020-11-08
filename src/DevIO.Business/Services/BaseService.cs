using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Business.Notificacoes;
using FluentValidation;
using FluentValidation.Results;

namespace DevIO.Business.Services
{
    // As classes "Services" são classes que possuem métodos que modificam o estado da entidade no banco
    // Para isso vamos implementar métododos que são obrigatórios passar por um "Service" para fazer alguma coisa que modifique uma entidade no db (Criamos os contratos: Interfaces)

    // Classe base de serviços
    public abstract class BaseService
    {

        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        // Método que recebe um validationResult que é uma coleção de erros
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        // Método que recebe a mensagem de erro que vai ser propagada para algum lugar
        protected void Notificar(string mensagem)
        {
            // Propaga o erro até a camada de apresentação | Coloca na lista de notificação mais uma mensagem
            _notificador.Handle(new Notificacao(mensagem));
        }

        // Método generico que recebe a classe de validação (Ex: \DevIO.Business\Models\Validations\FornecedorValidation.cs) e qualquer tipo de entidade da aplicação
        // TV: Classe generica de validação
        // TE: Entidade genérica
        // where TV : AbstractValidator<TE> | Onde TV que é validação é do tipo abstract de TE
        // where TE : Entity || E onde TE precisa ser do tipo Entity (Uma entidade/model de negócio)
        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);
            if (validator.IsValid) return true;
            Notificar(validator);
            return false;
        }
    }
}
