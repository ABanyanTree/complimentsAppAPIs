using Dapper;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LikeKero.Data.Interfaces;

namespace LikeKero.Data.Services
{
    public class DapperResolver<T>: IDapperResolver<T>
    {
        public DynamicParameters GetParameters(string[] addParams, T entity)
        {
            var param = new DynamicParameters();
            PropertyInfo[] Props = typeof(T).GetProperties();
            List<string> lstParas = addParams.ToList();

            var f = (from c in Props
                     where (lstParas.Contains(c.Name))
                     select new { PropName = c.Name, PropVal = c.GetValue(entity, null), PropType = c.PropertyType }).ToList();

            foreach (var singleParam in f)
            {
                if(singleParam.PropVal != null && !string.IsNullOrEmpty(Convert.ToString(singleParam.PropVal)))
                         param.Add(singleParam.PropName, singleParam.PropVal);
            }
            return param;
        }
    }
}
