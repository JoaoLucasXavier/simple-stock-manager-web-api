using System;
using System.ComponentModel;

namespace app.Models
{
    public class Produto : Entity
    {

        //EF - FK que identifica que esse produto pertence a um fornecedor
        public Guid FornecedorId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        // EF Relations: EF entender que existe uma relação de 1:1 Cada produto possui um fornecedor
        public Fornecedor Fornecedor { get; set; }
    }
}
