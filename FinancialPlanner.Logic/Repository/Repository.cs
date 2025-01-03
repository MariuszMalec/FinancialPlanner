﻿using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return entities.AsQueryable();
        }

        public async Task<T> GetById(string id)
        {
            var entity = await entities.SingleOrDefaultAsync(s => s.Id == id);
            return entity;
        }
        public async Task<T> Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);//TODO aby dodac uzytkownika z rola istniejaca trzeba go wykasowac z bazy!
            entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {

            entity.CreatedAt = DateTime.UtcNow;

            // Untrack previous entity version
            var trackedEntity = _context.Set<T>()
                .SingleOrDefaultAsync(e => e.Id == entity.Id);
            _context.Entry<T>(await trackedEntity).State = EntityState.Detached;

            // Track new version
            _context.Set<T>().Attach(entity);
            _context.Entry<T>(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            //if (entity == null)
            //{
            //    throw new ArgumentNullException("entity");
            //}
            //entities.Update(entity);
            //await _context.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
