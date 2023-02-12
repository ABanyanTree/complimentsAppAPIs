using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces
{
    public interface IServiceBase<T>
    {
        Task<T> GetAsync(T obj);
        Task<int> AddEditAsync(T obj);
        Task<int> DeleteAsync(T obj);
        Task<IEnumerable<T>> GetAllAsync(T obj);
         
    }
}
