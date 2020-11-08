using System.Collections.Generic;
using System.ComponentModel;

namespace app.Models
{
    public class Fornecedor : Entity
    {

        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }

        // EF Relations: EF entender que existe uma relação de 1:n de Fornecedor com Produto
        public IEnumerable<Produto> Produtos { get; set; }
    }
}
