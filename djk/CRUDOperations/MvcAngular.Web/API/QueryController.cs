using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using MvcAngular.Web.Models;
using MvcAngular.Web.Models.Binders;
using MvcAngular.Web.Repository;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace MvcAngular.Web.API
{
    public class QueryController : ApiController
    {
        // GET api/values/45643123
        public HttpResponseMessage Get(String id)
        {
            HttpResponseMessage response;
            try
            {
                SqlConnection cnn = getSqlConn();
                SqlCommand cmd = getQueryCommand(id, cnn);
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var r = Serialize(reader);
                        string json = JsonConvert.SerializeObject(r, Formatting.Indented);
                        response = Request.CreateResponse(HttpStatusCode.OK, json);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
            return response;
        }

        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        private SqlCommand getQueryCommand(String callerId, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT TOP 10 dbo.tbl_svm_call_events.fld_timestamp, dbo.tbl_svm_calls.fld_callerid, dbo.tbl_svm_consoles.fld_name AS AcdExt, " +
                        "dbo.tbl_svm_users.fld_name AS Username " +
                        "FROM dbo.tbl_svm_call_events INNER JOIN " +
                        "dbo.tbl_svm_calls ON dbo.tbl_svm_call_events.fld_call = dbo.tbl_svm_calls.id INNER JOIN " +
                        "dbo.tbl_svm_consoles ON dbo.tbl_svm_call_events.fld_console = dbo.tbl_svm_consoles.guid INNER JOIN " +
                        "dbo.tbl_svm_users ON dbo.tbl_svm_call_events.fld_user = dbo.tbl_svm_users.guid " +
                        "WHERE dbo.tbl_svm_call_events.fld_event = '12' AND dbo.tbl_svm_calls.fld_callerid = @callerid " +
                        "ORDER BY dbo.tbl_svm_call_events.fld_timestamp DESC";
            cmd.Parameters.Add("@callerid", System.Data.SqlDbType.VarChar, 20).Value = callerId;
            return cmd;
        }

        private SqlConnection getSqlConn()
        {
            string connectionString = null;
            SqlConnection cnn;
            connectionString = "server=Odin\\SQLEXPRESS;" +
                "Trusted_Connection=yes;" +
                "database=testdb; " +
                "connection timeout=2";
            /*
             * "server=Odin\\SQLEXPRESS;" +
            "Trusted_Connection=yes;" +
            "database=testdb; " +
            "connection timeout=30";
             * */
            cnn = new SqlConnection(connectionString);
            return cnn;
        }
    }
}
