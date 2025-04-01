using System;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Repositories;

public class ComponentsRepository(AppDbContext context) : IComponentsRepository
{
    public async Task<ICollection<Component>> GetComponentsAsync()
    {
        return await context.Components.Take(20).ToListAsync();
    }
}
