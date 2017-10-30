using AccountManager.Providers.Persistences.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using AccountManager.Providers.Persistences.Providers;

namespace AccountManager.Providers.Persistences
{
    public class OperationRepository : IRepository<OperationDb>
    {
        private readonly IProvider _provider;

        public OperationRepository(IProvider provider)
        {
            _provider = provider;
        }

        public void Add(OperationDb value)
        {
            var isCreditAsInt = value.IsCredit ? 1 : 0;
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = "insert into operation (id, date, libelle, amount, isCredit)" +
                    $"select '{value.Id}','{value.Date}', '{value.Libelle}','{value.Amount}','{isCreditAsInt}' " +
                    $"where not exists (select id from operation where id = '{value.Id}')";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Edit(OperationDb value)
        {

        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string id)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select id from operation where id = '{id}'";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    } 
                }  
            }
        }

        public OperationDb Get(string id)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from operation where id = '{id}'";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = reader["amount"];
                            return new OperationDb(
                                reader["id"].ToString(),
                                DateTime.Parse(reader["date"].ToString()),
                                reader["libelle"].ToString(),
                                float.Parse(r.ToString(),
                                System.Globalization.NumberStyles.Any), int.Parse(reader["isCredit"].ToString()) == 1);
                        }

                        return null;
                    }    
                }    
            }
        }

        public IEnumerable<OperationDb> GetAll()
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from operation";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = reader["amount"];
                            yield return new OperationDb(
                                reader["id"].ToString(),
                                DateTime.Parse(reader["date"].ToString()),
                                reader["libelle"].ToString(),
                                float.Parse(r.ToString(),
                                System.Globalization.NumberStyles.Any), int.Parse(reader["isCredit"].ToString()) == 1);
                        }
                    } 
                } 
            }
        }

        public IEnumerable<OperationDb> GetAllUncategorized()
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select * from operation  as o where not exists (select * from operationCategory as oc where oc.operationId = o.id)";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var r = reader["amount"];
                        yield return new OperationDb(
                            reader["id"].ToString(),
                            DateTime.Parse(reader["date"].ToString()),
                            reader["libelle"].ToString(),
                            float.Parse(r.ToString(),
                            System.Globalization.NumberStyles.Any), int.Parse(reader["isCredit"].ToString()) == 1);
                    }
                }
            }
        }

        public IEnumerable<OperationCategoryDb> GetAllCategorized()
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select oc.operationId, o.date, o.libelle, o.amount, o.isCredit, oc.categoryId, c.name from operationCategory as oc " +
                    $"inner join operation as o on oc.operationId = o.id " +
                    $"inner join category as c on oc.categoryId = c.id";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new OperationCategoryDb(
                                reader["operationId"].ToString(),
                                DateTime.Parse(reader["date"].ToString()),
                                reader["libelle"].ToString(),
                                float.Parse(reader["amount"].ToString(), System.Globalization.NumberStyles.Any),
                                int.Parse(reader["isCredit"].ToString()) == 1,
                                reader["categoryId"].ToString(),
                                reader["name"].ToString()
                                );
                        }
                    }
                }
            }
        }

        public OperationCategoryDb GetCategorized(string operationId)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = $"select oc.operationId, o.date, o.libelle, o.amount, o.isCredit, oc.categoryId, c.name from operationCategory as oc " +
                    $"inner join operation as o on oc.operationId = o.id " +
                    $"inner join category as c on oc.categoryId = c.id " +
                    $"where o.id = '{operationId}'";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             return new OperationCategoryDb(
                                reader["operationId"].ToString(),
                                DateTime.Parse(reader["date"].ToString()),
                                reader["libelle"].ToString(),
                                float.Parse(reader["amount"].ToString(), System.Globalization.NumberStyles.Any),
                                int.Parse(reader["isCredit"].ToString()) == 1,
                                reader["categoryId"].ToString(),
                                reader["name"].ToString()
                                );
                        }
                    }
                }

                return null;
            }
        }

        public void AddCategory(string operationId, string categoryId)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = "insert into operationCategory (operationId, categoryId)" +
                    $"select '{operationId}','{categoryId}' " +
                    $"where not exists (select operationId from operationCategory where operationId = '{operationId}')";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }   
            }
        }

        public void EditCategory(string operationId, string categoryId)
        {
            using (var cnx = (SQLiteConnection)_provider.Create())
            {
                string sql = 
                    $"update operationCategory set categoryId = '{categoryId}' " +
                    $"where operationId = '{operationId}'";

                using (var cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
