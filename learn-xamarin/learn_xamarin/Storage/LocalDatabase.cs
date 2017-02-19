using learn_xamarin.Model;
using learn_xamarin.Services;
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

        public void Insert(Expenditure e)
        {
            _sqliteConnection.Insert(e);
        }

        private void InitiateSchemaIfNeeded() // todo tmp. 
        {
            RecreateTable<Expenditure>();
            RecreateTable<UnsynchronizedItem>();
        }

        private void RecreateTable<T>()
        {
            if (CheckIfTableExists(nameof(T)))
            {
                _sqliteConnection.DropTable<T>();
            }
            _sqliteConnection.CreateTable<T>();
        }

        private bool CheckIfTableExists(string name)
        {
            var q = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{name}';";
            var qResult = _sqliteConnection.ExecuteScalar<string>(q);
            return qResult != null;
        }

        public Expenditure[] GetAllExpenditures()
        {
            return _sqliteConnection.Query<Expenditure>($"select * from {nameof(Expenditure)}").ToArray();
        }

        public UnsynchronizedItem[] GetAllUnsynchronizedItems()
        {
            return _sqliteConnection.Query<UnsynchronizedItem>($"select * from {nameof(UnsynchronizedItem)}").ToArray();
        }

        public void ClearUnsynchronizedItems()
        {
            _sqliteConnection.DeleteAll<UnsynchronizedItem>();
        }

        public void Insert(UnsynchronizedItem unsynchronizedItem)
        {
            _sqliteConnection.Insert(unsynchronizedItem);
        }
    }
}