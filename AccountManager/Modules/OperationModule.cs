using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.Models;
using AccountManager.Providers.Persistences;
using AccountManager.Providers.Persistences.Models;
using Nancy;
using Nancy.ModelBinding;

namespace AccountManager.Modules
{
    public class OperationModule : NancyModule
    {
        private readonly OperationRepository _operationRepository;
        public OperationModule(IRepository<OperationDb> operationRepository, IRepository<CategoryDb> categoryRepository) : base("operation")
        {
            _operationRepository = (OperationRepository)operationRepository;

            Get["/categorized"] = _ =>
            {
                var model = _operationRepository.GetAllCategorized().OrderByDescending(x => x.Date);
                return View["categorized", model];
            };

            Get["/uncategorized"] = _ =>
            {
                var model = categoryRepository.GetAll();
                return View["uncategorized", model];
            };

            Get["/getUncategorized"] = _ =>
            {
                var model = _operationRepository.GetAllUncategorized().OrderByDescending(x => x.Date);
                return Response.AsJson(model);
            };

            Post["/addCategory"] = _ =>
            {
                var model = this.Bind<AddCategoryViewModel>();
                _operationRepository.AddCategory(model.OperationId, model.CategoryId);

                return Response.AsJson(true);
            };
        }
    }
}
