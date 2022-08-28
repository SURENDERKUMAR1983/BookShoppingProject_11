using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.DataAccess.Repository.IRepository
{
  public  interface ISP_Cal :IDisposable
    {
        T Single<T>(String procedureName, DynamicParameters param= null);
        T OneRecord<T>(String procedureName, DynamicParameters param = null);
        void Execute(String procedureName, DynamicParameters param = null);
        IEnumerable<T> List<T>(String procedureName, DynamicParameters param = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(String procedureNmae, DynamicParameters param = null);

    }
}
