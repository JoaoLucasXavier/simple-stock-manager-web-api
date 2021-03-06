﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Http;

namespace DevIO.Api.ViewModels
{
    public class ProdutoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        // IFormFile: Tipo necessário para upload de imagens. | Possui propriedades: Nome, extensão, tamanho etc
        [DisplayName("Imagem do Produto")]
        [NotMapped] // O scaffold não consegui mapear esse campo por isso NotMapped
        public string ImagemUpload { get; set; }

        public string Imagem { get; set; }

        // [Moeda(ErrorMessage = "Error message customizado moeda invalida")] //  Fazemos o uso da data annotation "[Moeda]" criada em: \DevIO.Api\Extensions\MoedaAttribute.cs
        // [Moeda] //  Fazemos o uso da data annotation "[Moeda]" criada em: \DevIO.Api\Extensions\MoedaAttribute.cs
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        // [ScaffoldColumn (false)]: Na hora de fazer o scaffold não será levada em consideração essa coluna: DataCadastro
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        [ScaffoldColumn(false)]
        public string NomeFornecedor { get; set; }
    }
}
