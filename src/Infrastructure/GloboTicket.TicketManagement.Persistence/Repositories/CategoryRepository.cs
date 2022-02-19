using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(GloboTicketDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Category>> GetCategoriesWithEventsAsync(bool includePassedEvents)
    {
        if (includePassedEvents is true)
        {
            var categoriesWithAllEvents = await _dbContext.Categories
                .Include(c => c.Events)
                .ToListAsync();
            
            return categoriesWithAllEvents;
        }
        else
        {
            var categoriesWithoutPassedEvents = await _dbContext.Categories
                .Include(c => c.Events
                    .Where(e => e.Date < DateTime.Today))
                .ToListAsync();
            
            return categoriesWithoutPassedEvents;
        }
    }
}