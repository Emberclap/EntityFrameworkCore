using System.Reflection;

namespace MiniORM
{
    abstract class DbContext
    {
        private readonly DatabaseConnection _connection;
        private readonly Dictionary<Type, PropertyInfo> _dbSetProperties;
        internal static HashSet<Type> AllowedSqlTypes { get; } = new HashSet<Type>
        {
            typeof(string), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(decimal),
            typeof(bool), typeof(DateTime)
        };

        protected DbContext(string connectionString)
        {
            _connection = new DatabaseConnection(connectionString);
            _dbSetProperties = DiscoverDbSets();
            using (new ConnectionManager(_connection))
            {
                InitializeDbSets();
            }
            MapAllRelations();
        }

        public void SaveChanges()
        {
            var dbSets = _dbSetProperties
                .Select(pi => pi.Value.GetValue(this))
                .ToList();

            foreach(IEnumerable<object> dbSet in dbSets)
            {
                var invalidEntites = dbSet
                    .Where(entity => !IsObjectValid(entity))
                    .ToArray();
            }
            if (invalidEntites.Any())
            {
                throw new InvalidOperationException($"{invalidEntites.Length} Invalid Entities Found in {dbSets.GetType().Name}")
            }
        }
    }
}
