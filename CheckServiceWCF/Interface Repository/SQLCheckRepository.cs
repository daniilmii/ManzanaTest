using CheckServiceWCF.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckServiceWCF.Interface_Repository
{
    public class SQLCheckRepository : IRepository<CheckEntity>
    {
        string connectionString;
        public SQLCheckRepository() 
        {
            connectionString = Configurations.CurrentConfig.ConnectionString;
        }
      

        public IEnumerable<CheckEntity> GetLastNChecks(int n)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return GetChequesPackProcedure(n, db);
            }
        }

        public void SaveCheck(CheckEntity item)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                SaveChequeProcedure(item, db);
            }
        }
        
        public void SaveChequeProcedure(CheckEntity item, IDbConnection connection ) 
        {
            var procedure = "[save_cheque]";
            var values = new { cheque_id = item.Id, cheque_number = item.Number, summ = item.Summ, discount = item.Discount, articles = String.Join(";",item.Articles) };
            var results = connection.Query(procedure, values, commandType: CommandType.StoredProcedure).ToList();
            
        }
        public List<CheckEntity> GetChequesPackProcedure(int pack_size, IDbConnection connection)
        {
            var procedure = "[get_cheques_pack]";
            var values = new { pack_size = pack_size };
            return connection.Query<CheckEntity>(procedure, values, commandType: CommandType.StoredProcedure).ToList();

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}