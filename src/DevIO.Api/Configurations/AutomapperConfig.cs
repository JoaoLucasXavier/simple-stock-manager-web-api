using app.Models;
using AutoMapper;
using DevIO.Api.ViewModels;

namespace DevIO.Api.Configurations
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            // Fazemos o mapeamento das ViewModel/DTO e entidades(/DevIO.Business/Models/)
            // OBS: "Conseguimos transformar Fornecedor em um FornecedorViewModel não o contrário." | É um caminho de mão única!
            // ReverseMap (): "Agora Conseguimos transformar um FornecedorViewModel em um Fornecedor." | Com isso temos um mapeamento de mão dupla!
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();
            CreateMap<ProdutoImagemViewModel, Produto>().ReverseMap();
            //  Mapear fornecedor para ProdutoModel DTO com AutoMapper
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
        }
    }
}
