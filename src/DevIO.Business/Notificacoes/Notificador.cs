using System.Collections.Generic;
using System.Linq;
using DevIO.Business.Interfaces;

namespace DevIO.Business.Notificacoes
{
    public class Notificador : INotificador
    {

        // _notificacoes: Lista de notificação global que vai existir durante todo o request
        // Será como uma pilha que vamos colocando notificações aqui dentro
        private List<Notificacao> _notificacoes;

        // Ao dar um new em "Notificador" cria a lista de notificações
        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        // Handle: Adiciona a noticação na lista: _notificacoes
        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        // Retorna a lista: _notificacoes
        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        // Verifica se existe notificação
        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
