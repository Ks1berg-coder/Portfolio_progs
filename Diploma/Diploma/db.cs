using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    internal class db
    {
        private static string dbname = @"KOMPUTER";
        public static SqlConnection conn = new SqlConnection($@"Data Source={dbname};Initial Catalog=PGIC;Integrated Security=True");
    }
}
