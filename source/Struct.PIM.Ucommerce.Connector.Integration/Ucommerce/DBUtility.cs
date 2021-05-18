using System.Data.SqlClient;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce
{
    internal class DBUtility
    {
        /// <summary>
        /// Access to server DB
        /// </summary>
        /// <returns></returns>
        internal static SqlConnection GetOpenConnection(string customConnectionString = null)
        {
            SqlConnection connection;

            if (!string.IsNullOrEmpty(customConnectionString))
            {
                connection = new SqlConnection(customConnectionString);
            }
            else
            {
                connection = new SqlConnection(Settings.Ucommerce.DbConnectionString);
            }

            connection.Open();
            return connection;
        }
    }
}