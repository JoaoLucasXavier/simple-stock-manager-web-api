using System.Collections.Generic;
using DevIO.Business.Notificacoes;

namespace DevIO.Business.Interfaces
{
    public interface INotificador
    {
        // Valida se tem notificação, retornando true ou false
        bool TemNotificacao();
        // Retorna uma lista de notificações
        List<Notificacao> ObterNotificacoes();
        // Manipúla uma notificação quando ela for lançada
        void Handle(Notificacao notificacao);
    }
}
