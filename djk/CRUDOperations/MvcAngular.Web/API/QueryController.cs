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
using System.Text;

namespace MvcAngular.Web.API
{
    public class QueryController : ApiController
    {

        public DjkResponse Get([ModelBinder] DjkRequest model)
        {
            DjkResponse response = new DjkResponse();
            model = model ?? new DjkRequest();
            try
            {
                SqlConnection cnn = getSqlConn();
                SqlCommand cmd = getQueryCommand(model, cnn);
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        response.Rows = new List<Row>();
                        while(reader.Read())
                        {
                            Row row = new Row();
                            row.Fld_Timestamp = reader.GetDateTime(0);
                            row.Fld_CallerId = reader.GetString(1);
                            row.AcdExt = reader.GetString(2);
                            row.Username = reader.GetString(3);
                            response.Rows.Add(row);
                        }

                        
                        response.Page = model.PageIndex;
                        response.Records = ((model.PageIndex -1) * model.PageSize) + response.Rows.Count;
                        response.Total = response.Records / model.PageSize + 1; // total pages
                    }
                    else
                    {
                        HttpResponseMessage notFound = new HttpResponseMessage();
                        notFound.StatusCode = HttpStatusCode.NotFound;
                        notFound.Content = new StringContent("Not found", Encoding.UTF8, "text/html");
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                HttpResponseMessage internalError = new HttpResponseMessage();
                internalError.StatusCode = HttpStatusCode.InternalServerError;
                internalError.Content = new StringContent(ex.Message.ToString(), Encoding.UTF8, "text/html");
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

        private SqlCommand getQueryCommand(DjkRequest request, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sortorder = request.Descending ? "DESC" : "ASC";
            string orderby;
            if (request.OrderBy.Equals("fld_timestamp"))
            {
                orderby = "dbo.tbl_svm_call_events.fld_timestamp";
            }
            else if (request.OrderBy.Equals("fld_callerid"))
            {
                orderby = "dbo.tbl_svm_calls.fld_callerid";
            }
            else if (request.OrderBy.Equals("AcdExt"))
            {
                orderby = "dbo.tbl_svm_consoles.fld_name";
            }
            else
            {
                orderby = "dbo.tbl_svm_users.fld_name";
            }
            //string orderby = request.OrderBy; // SQL injection vulnerable
            
            string command = "SELECT dbo.tbl_svm_call_events.fld_timestamp, dbo.tbl_svm_calls.fld_callerid, dbo.tbl_svm_consoles.fld_name AS AcdExt, " +
                        "dbo.tbl_svm_users.fld_name AS Username " +
                        "FROM dbo.tbl_svm_call_events INNER JOIN " +
                        "dbo.tbl_svm_calls ON dbo.tbl_svm_call_events.fld_call = dbo.tbl_svm_calls.id INNER JOIN " +
                        "dbo.tbl_svm_consoles ON dbo.tbl_svm_call_events.fld_console = dbo.tbl_svm_consoles.guid INNER JOIN " +
                        "dbo.tbl_svm_users ON dbo.tbl_svm_call_events.fld_user = dbo.tbl_svm_users.guid " +
                //            "WHERE dbo.tbl_svm_call_events.fld_event = '12' AND dbo.tbl_svm_calls.fld_callerid = @callerid " +
                        "WHERE dbo.tbl_svm_call_events.fld_event = '12' @callerid " +
                        "ORDER BY @orderby @sortorder " +
                        "OFFSET @offset ROWS";
            string cmd1 = command.Replace("@sortorder", sortorder);
            string cmd2 = cmd1.Replace("@orderby", orderby);
            string cmd3 = request.CallerId.Length > 1 ? cmd2.Replace("@callerid", String.Concat("AND dbo.tbl_svm_calls.fld_callerid = ", request.CallerId)) : cmd2.Replace("@callerid", "");
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = cmd3;
            //cmd.Parameters.Add("@callerid", System.Data.SqlDbType.VarChar, 20).Value = callerid;
            cmd.Parameters.Add("@offset", System.Data.SqlDbType.Int).Value = (request.PageIndex - 1) * request.PageSize;
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
