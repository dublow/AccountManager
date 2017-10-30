using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using AccountManager.Providers.Persistences;
using AccountManager.Providers.ValueObjects;
using AccountManager.Providers.Models;
using AccountManager.Providers.Persistences.Models;

namespace Helper.Modules
{
    public class HomeModule : NancyModule
    {
        private readonly OperationRepository _operationRepository;
        public HomeModule(IRepository<OperationDb> operationRepository, IRepository<CategoryDb> categoryRepository)
        {
            _operationRepository = (OperationRepository)operationRepository;
            Get["/"] = _ => View["index"];

            Get["/Stat"] = _ =>
            {
                var result = _operationRepository.GetAllCategorized();

                var ss = new StatServices(new Account("Nicolas", 3000, true), result);
                var sold = ss.GetSold();
                var spents = ss.GetSpents(DateRange.Current);
                var receipts = ss.GetReceipts(DateRange.Current);
                var ratio = ss.GetProfitability(DateRange.Current);
                var left = ss.GetLeft();
                var annualSpents = ss.GetAnnualSpent(2017);
                var annualReceipts = ss.GetAnnualReceipts(2017);
                var spentsByCategories = ss.GetByCategories(DateRange.Current);
                var allByDateAndCategories = ss.GetByDateAndCategories(2017);


                var anualSpentsReceipts = from aR in annualReceipts
                    from aS in annualSpents
                    where aR.date == aS.date
                    
                    select new {date = aR.date, spents = aS.value, receipts = aR.value};

                var lastOperations = ss.GetLastOperations();

                return Response.AsJson(new
                {
                    sold,
                    spents,
                    receipts,
                    ratio,
                    left,
                    anualSpentsReceipts,
                    lastOperations,
                    spentsByCategories,
                    allByDateAndCategories
                });
            };
        }
    }
}
