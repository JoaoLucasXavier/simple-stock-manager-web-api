using System;

namespace app.Models
{
    public class Endereco : Entity
    {

        //EF - FK que identifica que esse Endereco pertence a um fornecedor
        public Guid FornecedorId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        // EF Relations: EF entender que existe uma relação de 1:1 Cada endereco possui um fornecedor
        public Fornecedor Fornecedor { get; set; }
    }
}
