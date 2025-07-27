namespace Sanayii.Repository
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
