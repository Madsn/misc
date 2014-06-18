using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private SqlCommand getQueryCommand(String callerId, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT TOP 10 dbo.tbl_svm_call_events.fld_timestamp, dbo.tbl_svm_calls.fld_callerid, dbo.tbl_svm_consoles.fld_name AS AcdExt, " +                        "dbo.tbl_svm_users.fld_name AS Username " +
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
                "connection timeout=30";
            cnn = new SqlConnection(connectionString);
            return cnn;
        }

        private void onButtonClick(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn = getSqlConn();
            SqlCommand cmd = getQueryCommand(phoneInputBox.Text, cnn);
            try
            {
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dataGrid.Visibility = Visibility.Visible;
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGrid.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        dataGrid.Visibility = Visibility.Hidden;
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                label1.Content = ex.Message;
            }
        }
    }
}
