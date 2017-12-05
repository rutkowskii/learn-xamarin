using learn_xamarin.Model;
using SQLite;
using Xamarin.Forms;

namespace learn_xamarin.Storage
{
    public class LocalDatabase : ILocalDatabase
    {
        private readonly string _localFilePath;
        private readonly SQLiteConnection _sqliteConnection;
        private const string FileName = "ExpendituresDb";

        public LocalDatabase()
        {
            _localFilePath = DependencyService.Get<IFileHelper>().GetLocalFilePath(FileName);
            _sqliteConnection = new SQLiteConnection(_localFilePath);

            InitiateSchemaIfNeeded();
        }

        public Expenditure[] GetAllExpenditures()
        {
            return _sqliteConnection.Query<Expenditure>($"select * from {nameof(Expenditure)}").ToArray();
        }

        public UnsynchronizedItem[] GetAllUnsynchronizedItems()
        {
            return _sqliteConnection.Query<UnsynchronizedItem>($"select * from {nameof(UnsynchronizedItem)}").ToArray();
        }

        public ConfigEntry[] GetAllConfigEntries()
        {
            return _sqliteConnection.Query<ConfigEntry>($"select * from {nameof(ConfigEntry)}").ToArray();
        }

        public void UpdateConfig(ConfigEntry newValue)
        {
            _sqliteConnection.Execute($"update {nameof(ConfigEntry)} " +
                                      $"set {nameof(ConfigEntry.Value)} = ?" +
                                      $"where {nameof(ConfigEntry.Key)} = ?", newValue.Value, newValue.Key);

            // todo get rid of this guy
        }

        public void ClearUnsynchronizedItems()
        {
            _sqliteConnection.DeleteAll<UnsynchronizedItem>();
        }

        public void Insert(Expenditure expenditure)
        {
            _sqliteConnection.Insert(expenditure);
        }
        
        public void Insert(UnsynchronizedItem unsynchronizedItem)
        {
            _sqliteConnection.Insert(unsynchronizedItem);
        }

        public void UpdateCategories(Category[] categories)
        {
            _sqliteConnection.BeginTransaction();
            _sqliteConnection.DeleteAll<Category>();
            _sqliteConnection.InsertAll(categories);
            _sqliteConnection.Commit();
        }

        private void InitiateSchemaIfNeeded()
        {
            RecreateTable<Expenditure>();
            RecreateTable<Category>();
            RecreateTable<UnsynchronizedItem>();
            RecreateTable<ConfigEntry>();
        }

        private void RecreateTable<T>()
        {
            if (!CheckIfTableExists(nameof(T)))
            {
                _sqliteConnection.CreateTable<T>();
            }
        }

        private bool CheckIfTableExists(string name)
        {
            var q = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{name}';";
            var qResult = _sqliteConnection.ExecuteScalar<string>(q);
            return qResult != null;
        }

        public Category[] GetAllCategories()
        {
            return _sqliteConnection.Query<Category>($"select * from {nameof(Category)}").ToArray();
        }
    }
}