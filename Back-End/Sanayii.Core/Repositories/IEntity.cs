using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Repositories
{
    public interface IEntity<Entity>
    {
        public List<Entity> GetAll();
        public Entity GetById<T>(T id);

        public void Add(Entity entity);

        public void Edit(Entity entity);

        public void Delete<T>(T id);
    }
}
