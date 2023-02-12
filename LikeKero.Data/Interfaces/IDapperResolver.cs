using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Data.Interfaces
{
    public interface IDapperResolver<T>
    {
        DynamicParameters GetParameters(string[] addParams, T entity);
    }
}
