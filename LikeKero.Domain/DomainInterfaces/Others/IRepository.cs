using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
   public  interface IRepository<T>
    {
        #region Public Methods

        //this is for add/edit/delete  it return  integer  if no update return -1 
        Task<int> ExecuteNonQueryAsync(T obj, string[] param, string spName);
        //  T Get<T>(object obj, string  param, string spName);

        //to get single record 
        Task<T> GetAsync(T obj, string[] param, string spName);

        //Get All  records matching criteria with paging  
        Task<IEnumerable<T>> GetAllAsync(T obj, string[] param, string spName);

        dynamic GetMultipleAsync(T obj, string[] param, string spName, IEnumerable<dynamic> mapItems = null);
        #endregion

    }
}
