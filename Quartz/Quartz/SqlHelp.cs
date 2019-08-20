using QuartzSQL;
using System.Data.SqlClient;

namespace QuartzSql
{
    public class SqlHelp
    {
        private static XmlConfigTool XmlTool =null;
        //执行操作
        public static void ExSql(string sql,FileTool LogTool)
        {
            XmlTool = new XmlConfigTool("SqlServerConfig.xml");
            var connectionString = $"Min Pool Size=10;Max Pool Size=500;Connection Timeout=50;Data Source={XmlTool.GetValue("SqlServerIP")};Initial Catalog={XmlTool.GetValue("SqlServerDBName")};Persist Security Info=True;User ID={XmlTool.GetValue("SqlServerUserName")};Password={XmlTool.GetValue("SqlServerUserPwd")}";
            SqlConnection Conn = new SqlConnection(connectionString);
            try
            {
                
                Conn.Open();
                SqlCommand com = new SqlCommand(sql, Conn);
                com.CommandText = sql;
                com.ExecuteNonQuery();
                
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
