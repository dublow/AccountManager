using AccountManager.Providers.Persistences.Providers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Persistences
{
    public class SqliteDb
    {
        private readonly IProvider _provider;

        public SqliteDb(IProvider provider)
        {
            _provider = provider;
        }

        public void CreateDb(string dbname)
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var dbfile = $"{fileInfo.DirectoryName}\\{dbname}";

            if (!File.Exists(dbfile))
                SQLiteConnection.CreateFile($"{dbname}");

            using (SQLiteConnection cnx = (SQLiteConnection)_provider.Create())
            {
                
                var filename = $"{fileInfo.DirectoryName}\\Scripts\\db.sql";
                var script = File.ReadAllText(filename);
                var cmd = new SQLiteCommand(script, cnx);
                cmd.ExecuteNonQuery();
            }
        }

        public void CreateTable(string tname, string query)
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var lockFile = $"{fileInfo.DirectoryName}\\lock{tname}";

            if (File.Exists(lockFile))
                return;

            using (SQLiteConnection cnx = (SQLiteConnection)_provider.Create())
            {
                var cmd = new SQLiteCommand(query, cnx);
                cmd.ExecuteNonQuery();

                File.Create(lockFile);
            }
                
        }
    }
}
