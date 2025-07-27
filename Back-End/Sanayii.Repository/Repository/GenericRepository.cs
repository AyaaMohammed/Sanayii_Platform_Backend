using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using Sanayii.Core.Interfaces;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository
{
    public class GenericRepository<TEntity> : IEntity<TEntity> where TEntity : class
    {
        public SanayiiContext db;

        public GenericRepository(SanayiiContext db)
        {
            this.db = db;
        }

        public List<TEntity> GetAll()
        {
            List<TEntity> allEntity = db.Set<TEntity>().ToList();
            return allEntity;
        }

        public TEntity GetById<T>(T id)
        {
            TEntity entity = db.Set<TEntity>().Find(id);
            return entity;
        }

        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }

        public void Edit(TEntity entity)
        {
            db.Update(entity);
        }

        public void Delete<T>(T id)
        {
            TEntity entity = GetById(id);
            db.Set<TEntity>().Remove(entity);
        }
    }
}
