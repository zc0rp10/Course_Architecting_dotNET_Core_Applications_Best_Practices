using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;

public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoryListQuery, List<CategoryListVm>>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Category> _categoryRepository;

    public GetCategoriesListQueryHandler(IMapper mapper, IAsyncRepository<Category> categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryListVm>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var allCategories =
            (await _categoryRepository.ListAllAsync())
            .OrderBy(c => c.Name);

        return _mapper.Map<List<CategoryListVm>>(allCategories);
    }
}