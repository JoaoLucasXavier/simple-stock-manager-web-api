namespace DevIO.Business.Notificacoes
{
    public class Notificacao
    {

        // Passamos a mensagem de notificação via constructor
        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;
        }

        public string Mensagem { get; }
    }
}
