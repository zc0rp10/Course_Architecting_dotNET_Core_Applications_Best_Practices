using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;

public class GetCategoryListQuery : IRequest<List<CategoryListVm>>
{
}