using Dapper;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace LikeKero.Data.Services
{

    public class Repository<T> : IRepository<T>
    {
        private IOptions<ReadConfig> _connStr;
        private IDapperResolver<T> _resolver;


        public Repository(IOptions<ReadConfig> connStr, IDapperResolver<T> resolver
            )
        {
            _resolver = resolver;
            _connStr = connStr;

        }

        public async Task<int> ExecuteNonQueryAsync(T obj, string[] param, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                con.Open();
                IDbTransaction trans = con.BeginTransaction();
                try
                {
                    var dynamicparam = new DynamicParameters();
                    dynamicparam = _resolver.GetParameters(param, obj);
                    var result = await con.ExecuteAsync(spName, dynamicparam, trans, 0, System.Data.CommandType.StoredProcedure);

                    trans.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }


        public async Task<int> ExecuteNonQueryAsyncWithDynamicParam(List<KeyValuePair<string, string>> keyValuePairs, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                con.Open();
                IDbTransaction trans = con.BeginTransaction();
                try
                {

                    DynamicParameters dynamicparam = new DynamicParameters();
                    if (keyValuePairs != null)
                    {
                        foreach (var pair in keyValuePairs)
                        {
                            if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                                dynamicparam.Add("@@" + pair.Key, pair.Value);
                        }
                    }

                    var result = await con.ExecuteAsync(spName, dynamicparam, trans, 0, System.Data.CommandType.StoredProcedure);

                    trans.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }
        public async Task<IEnumerable<T>> GetAllAsync(T obj, string[] param, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                var dynamicparam = new DynamicParameters();
                dynamicparam = _resolver.GetParameters(param, obj);

                return await con.QueryAsync<T>(spName, dynamicparam, null, 0, CommandType.StoredProcedure);

            }
        }

        public async Task<IEnumerable<T>> GetAllAsyncWithDynamicParam(List<KeyValuePair<string, string>> keyValuePairs, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                DynamicParameters dynamicparam = new DynamicParameters();
                if (keyValuePairs != null)
                {
                    foreach (var pair in keyValuePairs)
                    {
                        if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                            dynamicparam.Add("@@" + pair.Key, pair.Value);
                    }
                }

                return await con.QueryAsync<T>(spName, dynamicparam, null, 0, CommandType.StoredProcedure);

            }
        }

        public dynamic GetMultipleAsync(T obj, string[] param, string spName, IEnumerable<dynamic> mapItems = null)
        {
            GridReader results = null;
            dynamic data = new ExpandoObject();
            using (IDbConnection con = this.Connection)
            {
                var dynamicparam = new DynamicParameters();
                dynamicparam = _resolver.GetParameters(param, obj);

                results = con.QueryMultiple(spName, dynamicparam, null, 0, CommandType.StoredProcedure);

                foreach (var item in mapItems)
                {
                    var listItem = results.Read(item.Type);
                    ((IDictionary<string, object>)data).Add(item.PropertyName, listItem);
                }

            }

            return data;
        }


        public dynamic GetMultipleAsyncWithDynamicParam(List<KeyValuePair<string, string>> keyValuePairs, string spName, IEnumerable<dynamic> mapItems = null)
        {
            GridReader results = null;
            dynamic data = new ExpandoObject();
            using (IDbConnection con = this.Connection)
            {
                DynamicParameters dynamicparam = new DynamicParameters();
                if (keyValuePairs != null)
                {
                    foreach (var pair in keyValuePairs)
                    {
                        if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                            dynamicparam.Add("@@" + pair.Key, pair.Value);
                    }
                }

                results = con.QueryMultiple(spName, dynamicparam, null, 0, CommandType.StoredProcedure);

                foreach (var item in mapItems)
                {
                    var listItem = results.Read(item.Type);
                    ((IDictionary<string, object>)data).Add(item.PropertyName, listItem);
                }

            }

            return data;
        }

        public async Task<T> GetAsync(T obj, string[] param, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                var dynamicparam = new DynamicParameters();
                dynamicparam = _resolver.GetParameters(param, obj);

                return await con.QueryFirstOrDefaultAsync<T>(spName, dynamicparam, null, 0, CommandType.StoredProcedure);
            }
        }

        public async Task<T> GetAsyncWithDynamicParam(List<KeyValuePair<string, string>> keyValuePairs, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                DynamicParameters dynamicparam = new DynamicParameters();
                if (keyValuePairs != null)
                {
                    foreach (var pair in keyValuePairs)
                    {
                        if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                            dynamicparam.Add("@@" + pair.Key, pair.Value);
                    }
                }

                return await con.QueryFirstOrDefaultAsync<T>(spName, dynamicparam, null, 0, CommandType.StoredProcedure);
            }
        }

        public async Task<T> GetAsyncWithObjects<T2>(T obj, string[] param, string spName, string TPrimaryKey, string T2PrimaryKey)
        {
            using (IDbConnection con = this.Connection)
            {
                var dynamicparam = new DynamicParameters();
                dynamicparam = _resolver.GetParameters(param, obj);

                dynamic dynamicobject = await con.QueryFirstOrDefaultAsync<dynamic>(spName, dynamicparam, null, 0, CommandType.StoredProcedure);

                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(T), new List<string> { TPrimaryKey });
                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(T2), new List<string> { T2PrimaryKey });

                var dynamicResult = Slapper.AutoMapper.MapDynamic<T>(dynamicobject);

                return dynamicResult;


            }
        }

        public async Task<IEnumerable<T>> GetAllAsyncWithObjects<T2>(T obj, string[] param, string spName, string TPrimaryKey, string T2PrimaryKey)
        {
            using (IDbConnection con = this.Connection)
            {
                var dynamicparam = new DynamicParameters();
                dynamicparam = _resolver.GetParameters(param, obj);

                dynamic dynamicobject = await con.QueryAsync<dynamic>(spName, dynamicparam, null, 0, CommandType.StoredProcedure);

                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(T), new List<string> { TPrimaryKey });
                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(T2), new List<string> { T2PrimaryKey });
                var dynamicResult = (Slapper.AutoMapper.MapDynamic<T>(dynamicobject) as IEnumerable<T>).ToList();
                return dynamicResult;



            }
        }

        public int SaveDataTable(string TableTypeName, DataTable dtRecords, string spName)
        {
            using (IDbConnection con = this.Connection)
            {
                try
                {
                    var dynamicparam = new DynamicParameters();
                    dynamicparam.Add("@" + TableTypeName, dtRecords.AsTableValuedParameter());
                    var result = con.Execute(spName, dynamicparam, null, 0, System.Data.CommandType.StoredProcedure);

                    return 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connStr.Value.ConnectionString);
            }
        }
    }
}
