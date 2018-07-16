using System;
using FMASolutionsCore.BusinessServices.SQLAppConfigTypes;

namespace FMASolutionsCore.DataServices.DataRepository
{
    abstract public class SQLFactory
    {
        public virtual System.Data.IDbConnection CreateDBConnection(SQLAppConfigTypes type, string connectionString)
        {            
            switch(type)
            {
                case SQLAppConfigTypes.MSSQLServer: return new System.Data.SqlClient.SqlConnection(connectionString);
                default: throw new ArgumentOutOfRangeException(type.ToString());
            }
        }
    }
}