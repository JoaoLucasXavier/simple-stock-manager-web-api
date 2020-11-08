namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        // Chave de criptografia do token
        public string Secret { get; set; }
        // Validade do token
        public int ExpiracaoHoras { get; set; }
        // É quem emite: a aplicação
        public string Emissor { get; set; }
        // Quais URLs o token é válido (Audiência)
        public string ValidoEm { get; set; }
    }
}
