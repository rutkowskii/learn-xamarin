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

        public void Insert(Expenditure e)
        {
            _sqliteConnection.Insert(e);
        }

        private void InitiateSchemaIfNeeded()
        {
            if (CheckIfTableExists(nameof(Expenditure)))
            {
                _sqliteConnection.DropTable<Expenditure>();
            }
            _sqliteConnection.CreateTable<Expenditure>();
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
    }
}