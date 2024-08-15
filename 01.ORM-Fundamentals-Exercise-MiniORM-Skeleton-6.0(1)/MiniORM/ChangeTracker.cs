
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MiniORM
{
    public class ChangeTracker<T>
        where T : class, new()
    {
        private readonly List<T> _allEntities;
        private readonly List<T> _added;
        private readonly List<T> _removed;

   
        public ChangeTracker(IEnumerable<T> entities)
        {
            if (entities is null) throw new ArgumentNullException(nameof(entities));
            this._added = new List<T>();
            this._removed = new List<T>();
            this._allEntities = CloneEntities(entities).ToList();
        }
        public IReadOnlyCollection<T> Added => this._added.AsReadOnly();
        public IReadOnlyCollection<T> Removed => this._removed.AsReadOnly();
        public IReadOnlyCollection<T> AllEntities => this._allEntities.AsReadOnly();

        private static IEnumerable<T>? CloneEntities(IEnumerable<T> entities)
        {
          
            var cloneEntities = new List<T>();
            var properties = typeof(T).GetAllowedSqlProperties();
                
            foreach ( var entity in entities)
            {
                var copy = new T();

                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    property.SetValue(copy, value);
                }

                cloneEntities.Add(copy);
            }
            return cloneEntities;
        }

        public void Add(T item) => this._added.Add(item);

        public void Remove(T item ) => this._removed.Remove(item);


        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet())
        {
            var modifiedEntities = new List<T>();
            var primaryKeys = typeof(T).GetAllowedSqlProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (var proxyEntity in AllEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity). ToArray();

                var entity = dbSet.Entities.Single(entity => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

            var IsModified = IsModified(entity, proxyEntity);
                if (IsModified) 
                {
                        modifiedEntities.Add(entity);
                }
            }

            return modifiedEntities;
        }



        public static bool IsModified(T entity, T proxyEntity) 
        {
            var monitoredProperties = typeof(T).GetAllowedSqlProperties();

            var modifiedProperties = monitoredProperties
                .Where(pi => !Equals(pi.GetValue(entity), pi.GetValue(proxyEntity)))
                .ToArray();
            var isModified = modifiedProperties.Any();
            return isModified;
        }

        public static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T entity) 
        {
            return primaryKeys.Select(pi => pi.GetValue(entity));
        }
    }
}

  