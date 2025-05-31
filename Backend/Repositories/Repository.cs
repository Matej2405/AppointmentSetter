namespace Backend.Repositories;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;


    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task RemoveAsync(TEntity entity)
    {
        var entityToRemove = await _dbSet.FindAsync(entity);
        if(entityToRemove != null)
        {
            _dbSet.Remove(entityToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        var entitiesToRemove = await _dbSet.Where(e => entities.Contains(e)).ToListAsync();
        if (entitiesToRemove != null)
        {
            _dbSet.RemoveRange(entitiesToRemove);
            await _context.SaveChangesAsync();
        }
    }
}