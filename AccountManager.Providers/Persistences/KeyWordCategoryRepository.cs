using AccountManager.Providers.Persistences.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using AccountManager.Providers.Persistences.Providers;

namespace AccountManager.Providers.Persistences
{
    public class KeyWordCategoryRepository : IRepository<KeyWordCategoryDb>
    {
        private readonly IProvider _provider;

        public KeyWordCategoryRepository(IProvider provider)
        {
            _provider = provider;
        }

        public void Add(KeyWordCategoryDb value)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = "insert into keywordscategory (categoryId, keywords)" +
                    $"values('{value.CategoryId}','{value.KeyWord}')";

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
                string sql = $"select id from keywordscategory where categoryid = '{id}'";

                SQLiteCommand cmd = new SQLiteCommand(sql, cnx);

                SQLiteDataReader reader = cmd.ExecuteReader();

                return reader.Read();
            }
        }

        public KeyWordCategoryDb Get(string id)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from keywordscategory where categoryid = '{id}'";

                SQLiteCommand cmd = new SQLiteCommand(sql, cnx);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    return new KeyWordCategoryDb(
                        reader["categoryid"].ToString(), 
                        reader["keywords"].ToString());
                }

                return null;
            }
        }

        public IEnumerable<KeyWordCategoryDb> GetAll()
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from keywordscategory";

                SQLiteCommand cmd = new SQLiteCommand(sql, cnx);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    yield return new KeyWordCategoryDb(
                        reader["categoryid"].ToString(),
                        reader["keywords"].ToString());
                }
            }
        }
    }
}
