using Microsoft.Data.SqlClient;
using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Utils
{
    public static class QueryUtils
    {
        public static int startRecordDecrease = 1;

        public static string cutStringNull(string query)
        {
            if (query == null)
            {
                return query;
            }
            //string res = query.Replace("'null'", "null");
            string res = query.Replace("''", "null");
            return res;
        }

        public static string NumberNull(string val)
        {
            if (String.IsNullOrEmpty(val))
            {
                return "null";
            }

            return val.ToString() ;
        }

        public static string getParamIn(string freFix, string val)
        {
            freFix = "@" + freFix;
            int index_ = 0;
            if (String.IsNullOrEmpty(val))
            {
                return freFix + index_;
            }
            List<string> resList = new List<string>();
            foreach(string v in val.Split(","))
            {
                resList.Add(freFix + (index_++));
            }
            string resStr = String.Join(",", resList);
            return resStr;
        }
        public static void addParamIn(DbCommand command, string freFix, string val)
        {
            int index_ = 0;
            if (String.IsNullOrEmpty(val))
            {
                string name = freFix + index_;
                QueryUtils.addParam(command, name, val);
                return;
            }
            List<string> resList = new List<string>();
            foreach (string v in val.Split(","))
            {
                string name = freFix + (index_++);
                QueryUtils.addParam(command, name, v);
            }

        }

        public static void addParamIn(List<SqlParameter> sqlParameters, string freFix, string val)
        {
            int index_ = 0;
            if (String.IsNullOrEmpty(val))
            {
                string name = freFix + index_;
                sqlParameters.Add(QueryUtils.addSqlParameter(name, val));
                return;
            }
            List<string> resList = new List<string>();
            foreach (string v in val.Split(","))
            {
                string name = freFix + (index_++);
                sqlParameters.Add(QueryUtils.addSqlParameter(name, v));
            }

        }
        public static string padLeft(string str, char pad, int length)
        {
            return str.PadLeft(length, pad);
        }


        public static string getValueAsString(IDataRecord record, string columnName)
        {
            if (record.GetValue(record.GetOrdinal(columnName)) == null) {
                return "";
            }
            return record.GetValue(record.GetOrdinal(columnName)).ToString();
        }
        public static Decimal? getValueAsDecimal(IDataRecord record, string columnName)
        {
            var val = record.GetValue(record.GetOrdinal(columnName));
            return val == null || String.IsNullOrEmpty(val.ToString()) ? null : Convert.ToDecimal(val.ToString());

        }

        public static Decimal getValueAsDecimalRequired(IDataRecord record, string columnName)
        {
            var val = record.GetValue(record.GetOrdinal(columnName));
            return Convert.ToDecimal(val.ToString());

        }

        public static DateTime? getValueAsDateTime(IDataRecord record, string columnName)
        {
            var val = record.GetValue(record.GetOrdinal(columnName));
            return val == null || String.IsNullOrEmpty(val.ToString()) ? null : Convert.ToDateTime(val.ToString());

        }

        public static DateTime getValueAsDateTimeRequired(IDataRecord record, string columnName)
        {
            var val = record.GetValue(record.GetOrdinal(columnName));
            return Convert.ToDateTime(val.ToString());

        }



        // For Native Entity
        public static void addParamLike(DbCommand command, string name, object value)
        {
            createAddParam(command, name, value, true, null);
        }

        public static void addParam(DbCommand command, string name, object value)
        {
            createAddParam(command, name, value, false, null);
        }
        public static void addParamWithDbType(DbCommand command, string name, object value, DbType dbType)
        {
            createAddParam(command, name, value, false, dbType);
        }

        private static void createAddParam(DbCommand command, string name, object value, bool isLike, DbType? dbType)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@" + name;
            if (isLike)
            {
                parameter.Value = "%" + value + "%";
            }
            else
            {
                if (value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }
            }
            if (dbType != null)
            {
                parameter.DbType = (DbType)((dbType == null ? DbType.String : dbType));
            }
            command.Parameters.Add(parameter);
        }





        // For Native Dynamic

        public static SqlParameter addSqlParameterLike(string name, object value)
        {
            return createAddSqlParameter(name, value, true, null);
        }

        public static SqlParameter addSqlParameter(string name, object value)
        {
            return createAddSqlParameter(name, value, false, null);
        }
        public static SqlParameter addSqlParameterWithDbType(string name, object value, DbType dbType )
        {
            return createAddSqlParameter(name, value, false, dbType);
        }

        private static SqlParameter createAddSqlParameter(string name, object value, bool isLike, DbType? dbType)
        {
            if (isLike)
            {
                return new SqlParameter("@" + name, "%" + value + "%");
            }
            else
            {
                if (value == null)
                {
                    value = DBNull.Value;
                }
                SqlParameter param = new SqlParameter("@" + name, value);
                param.IsNullable = true;
                if (dbType != null)
                {
                    param.DbType = (DbType) ((dbType ==null ? DbType.String : dbType));
                }
                return param;
                //return new SqlParameter("@" + name, value);
            }
            
        }

        public static void setSqlPaging(StringBuilder queryBuilder, DbCommand command, int length, int pageNo)
        {

            queryBuilder.AppendFormat(" OFFSET @Length  * (@PageNo - 1) ROWS ");
            queryBuilder.AppendFormat(" FETCH NEXT @Length ROWS ONLY ");
            QueryUtils.addParam(command, "Length", length);
            QueryUtils.addParam(command, "PageNo", pageNo);
        }

        public static void setSqlPaging(StringBuilder queryBuilder, List<SqlParameter> sqlParameters, int length, int pageNo)
        {

            queryBuilder.AppendFormat(" OFFSET @Length  * (@PageNo - 1) ROWS ");
            queryBuilder.AppendFormat(" FETCH NEXT @Length ROWS ONLY ");
            sqlParameters.Add(QueryUtils.addSqlParameter("Length", length));// Add New
            sqlParameters.Add(QueryUtils.addSqlParameter("PageNo", pageNo));// Add New
        }


    }
}
