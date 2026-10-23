using ClassLibrary1HRLeaveManagement.Domain.Common;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Presistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Presistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly HRDatabaseContext _context;

        public GenericRepository(HRDatabaseContext context)
        {
            this._context = context;
        }
        public async Task CreateAsync(T entity)
        {
           await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
          await  _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
            if(entity == null)
            {
                throw new Exception($"Entity of type {typeof(T).Name} with id {id} not found.");
            }
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            //_context.Update(entity); //no need to use this method because it will update all the properties of the entity, even if they haven't changed. Instead, we can use the Entry method to set the state of the entity to Modified, which will only update the properties that have changed.
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

      
    }
}
