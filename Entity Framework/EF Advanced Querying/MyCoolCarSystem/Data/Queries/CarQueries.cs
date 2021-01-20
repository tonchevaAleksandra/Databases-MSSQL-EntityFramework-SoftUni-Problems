using Microsoft.EntityFrameworkCore;
using MyCoolCarSystem.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoolCarSystem.Data.Queries
{
   public class CarQueries
    {

        public static Func<CarDbContext,int, IEnumerable<ResultModel>> ToResult
            = EF.CompileQuery<CarDbContext,int, IEnumerable<ResultModel>>(
                (db,price) => db.Cars
               .Where(c => c.Price > price)
               .Select(c => new ResultModel
               {
                   FullName = c.Model.Make.Name
               }));
    }
}
