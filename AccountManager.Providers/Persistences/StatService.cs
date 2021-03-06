﻿using AccountManager.Providers.Models;
using AccountManager.Providers.Persistences.Models;
using AccountManager.Providers.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountManager.Providers.Persistences
{
    public class StatServices
    {
        private readonly Account _account;
        private readonly IEnumerable<OperationCategoryDb> _operationCategories;

        public StatServices(Account account, IEnumerable<OperationCategoryDb> operationCategories)
        {
            _account = account;
            _operationCategories = operationCategories;
        }

        public decimal GetSold()
        {
            return _account.Sold;
        }

        public decimal GetSpents(DateRange dateRange)
        {
            return GetAmountByFilter(dateRange, isCredit => !isCredit);
        }

        public decimal GetReceipts(DateRange dateRange)
        {
            return GetAmountByFilter(dateRange, isCredit => isCredit);
        }

        

        public object GetLeft()
        {
            var sold = GetSold();
            var spent = GetSpents(DateRange.Current);

            return (int)Math.Round((100 * spent) / sold);
        }

        public IEnumerable<(string date, decimal value)> GetAnnualSpent(int year)
        {
            return GetAnnualAmountByFilter(year, isCredit => !isCredit);
        }

        public IEnumerable<(string date, decimal value)> GetAnnualReceipts(int year)
        {
            return GetAnnualAmountByFilter(year, isCredit => isCredit);
        }

        public object GetLastOperations()
        {
            return _operationCategories.OrderByDescending(x => x.Date)
                .Select(x => new { isCredit = x.IsCredit, date = x.Date.ToString("yyyy-MM-dd"), libelle = x.Libelle, amount = x.Amount })
                .Take(9).ToList();
        }

        public IEnumerable<object> GetByCategories(DateRange dateRange)
        {
            var unkownCategories = new List<object>();
            foreach (var category in _operationCategories.Select(x => x.Name).Distinct().OrderBy(x => x))
            {
                if (!_operationCategories
                    .Any(x => DateRange.Parse(x.Date) == dateRange && x.Name == category))
                {
                    unkownCategories.Add(new { category = category, amount = 0 });
                }
            }

            unkownCategories.AddRange(_operationCategories
                .Where(x => DateRange.Parse(x.Date) == dateRange)
                .GroupBy(x => x.Name, value => value.Amount)
                .Select(x => new { category = x.Key, amount = x.Sum() }).OrderBy(x => x.category));

            return unkownCategories;
        }

        public object GetByDateAndCategories(int year)
        {
            return YearRunner(year, (dateRange, ct) =>
            {
               
                var sum = _operationCategories
                    .Where(x => DateRange.Parse(x.Date) == dateRange && x.Name == ct)
                    .Sum(x => x.Amount);

                return (dateRange, ct, sum);
            }).GroupBy(key => 
                key.dr.Start,
                value => value).Select(x => new { date = x.Key.ToString("yyyy-MM"), categories = x.GroupBy(y => {
                    (DateRange dr, string cat, object o) = y;
                    return cat;
                }, value=> value).Select(z => z) });
        }

        private decimal GetAmountByFilter(DateRange dateRange, Predicate<bool> filter)
        {
            return _operationCategories
                .Where(x => DateRange.Parse(x.Date) == dateRange && filter(x.IsCredit))
                .Sum(x => x.Amount);
        }

        private IEnumerable<(string date, decimal value)> GetAnnualAmountByFilter(int year, Predicate<bool> filter)
        {
            return YearRunner(year, dateRange =>
            {
                var sum = _operationCategories
                    .Where(x => DateRange.Parse(x.Date) == dateRange && filter(x.IsCredit))
                    .Sum(x => x.Amount);

                return (dateRange, sum);
            }).GroupBy(key => key.dr.Start, value => value.value).Select(x => (x.Key.ToString("yyyy-MM"), x.Sum()));
        }

        private IEnumerable<(DateRange dr, T value)> YearRunner<T>(int year, Func<DateRange, (DateRange dr, T value)> func)
        {
            for (int i = 1; i < 13; i++)
            {
                var dr = new DateRange(new DateTime(year, i, 1));
                yield return func(dr);
            }
        }

        private IEnumerable<(DateRange dr, string, T value)> YearRunner<T>(int year, Func<DateRange,string, (DateRange dr, string cat, T value)> func)
        {
            var cats = _operationCategories.Select(x => x.Name).Distinct();
            for (int i = 1; i < 13; i++)
            {
                var dr = new DateRange(new DateTime(year, i, 1));
                foreach (var category in cats)
                {
                    yield return func(dr, category);
                }
            }
        }
    }
}
