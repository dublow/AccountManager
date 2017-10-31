using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using AccountManager.Providers.Persistences.Providers;
using AccountManager.Providers.Persistences;
using System.Configuration;
using AccountManager.Providers.Persistences.Models;
using AccountManager.Providers.Readers;
using AccountManager.Providers;
using AccountManager.Providers.Models;

namespace AccountManager
{
    public class DefaultBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            var providers = new SqliteProvider(ConfigurationManager.ConnectionStrings["accountmanager"].ConnectionString);
            var db = new SqliteDb(providers);
            var operationRepository = new OperationRepository(providers);
            var categoryRepository = new CategoryRepository(providers);
            var reader = new OperationReader(ConfigurationManager.AppSettings["filename"]);

            container.Register<IRepository<OperationDb>>(operationRepository);
            container.Register<IRepository<CategoryDb>>(categoryRepository);
            container.Register<IReader<AccountOperation>>(reader);

            db.CreateDb(ConfigurationManager.AppSettings["dbname"]);

            var r = reader.Read();

            r.Operations
                .Select(op => new OperationDb(op.Id, op.Date, op.Libelle, op.Amount, op.IsCredit))
                .ToList()
                .ForEach(op => operationRepository.Add(op));

            container.Register(r.Account);
        }
    }
}
