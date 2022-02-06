using System.Data.SqlClient;

namespace NEW_Mfile_Project.Models
{
    public class DBConnect
    {
        public static SqlConnection myCon = null;

        public DBConnect()
        {
            //myCon = new SqlConnection(@"Data Source=.;Initial Catalog=M-FilesWeb;Integrated Security=True");

            myCon = new SqlConnection(@"Data Source=NADEEM\SQLEXPRESS;Initial Catalog=M-FilesWeb;Integrated Security=True");

        }
    }
}