using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;

namespace Lab_4
{
    public class ConnectionStringFactory
    {
            public static SqlConnection Create(string path)
        {
            var connection = new SqlConnectionStringBuilder();

            var _path = Path.Combine(path, "App_Data", "DB_BOOKS.mdf");

            connection.AttachDBFilename = _path;
            connection.InitialCatalog = "DB_BOOKS";
            connection.IntegratedSecurity = true;

            return new SqlConnection(connection.ToString());
        }
    }
}