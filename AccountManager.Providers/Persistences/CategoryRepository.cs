using AccountManager.Providers.Persistences.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using AccountManager.Providers.Persistences.Providers;

namespace AccountManager.Providers.Persistences
{
    public class CategoryRepository : IRepository<CategoryDb>
    {
        private readonly IProvider _provider;


        public CategoryRepository(IProvider provider)
        {
            _provider = provider;
        }

        public void Add(CategoryDb value)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = "insert into category (id, name)" +
                    $"values('{value.Id}','{value.Name}')";

                var  cmd = new SQLiteCommand(sql, cnx);
                cmd.ExecuteNonQuery();
            }
                
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string id)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select id from category where id = '{id}'";

                SQLiteCommand cmd = new SQLiteCommand(sql, cnx);

                SQLiteDataReader reader = cmd.ExecuteReader();

                return reader.Read();
            }
        }

        public CategoryDb Get(string id)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from category where id = '{id}'";

                SQLiteCommand cmd = new SQLiteCommand(sql, cnx);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    var r = reader["amount"];
                    return new CategoryDb(
                        reader["id"].ToString(), 
                        reader["name"].ToString());
                }

                return null;
            }
        }

        public IEnumerable<CategoryDb> GetAll()
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from category";

                SQLiteCommand cmd = new SQLiteCommand(sql, cnx);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    yield return new CategoryDb(
                        reader["id"].ToString(),
                        reader["name"].ToString());
                }
            }
        }
    }
}
