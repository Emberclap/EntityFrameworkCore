using System.Collections;

namespace MiniORM
{
    public class DbSet<TEntity> : ICollection<TEntity>
        where TEntity : class, new()
    {
        internal ChangeTracker<TEntity> ChangeTracker { get; set; }
        internal IList<TEntity> Entities { get; set; }

        public DbSet(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToList();
            ChangeTracker = new ChangeTracker<TEntity>(entities);
        }

        public int Count => Entities.Count;

        public bool IsReadOnly => Entities.IsReadOnly;

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Item cannot be null");
            }
            this.Entities.Add(entity);
            this.ChangeTracker.Add(entity);
        }

        public void Clear()
        {
            while (this.Entities.Any())
            {
                var entity = this.Entities.First();
                Remove(entity);
            }
        }

        public bool Contains(TEntity item) => Entities.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);
     
        public IEnumerator<TEntity> GetEnumerator() => Entities.GetEnumerator();

        public bool Remove(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item cannot be null");
            }
            var remoedSuccessfully = Entities.Remove(item);

            if(remoedSuccessfully)
            {
                ChangeTracker.Remove(item);
            }
            return remoedSuccessfully;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities.ToArray())
            {
                Remove(entity);
            }
        }
    }
}
