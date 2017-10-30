using System.Data;
using System.Data.SQLite;

namespace AccountManager.Providers.Persistences.Providers
{
    public class SqliteProvider : IProvider
    {
        private readonly string _cnx;

        public SqliteProvider(string cnx)
        {
            _cnx = cnx;
        }

        public IDbConnection Create()
        {
            return new SQLiteConnection(_cnx).OpenAndReturn();
        }
    }
}
