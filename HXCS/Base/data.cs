using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;


namespace HXCS.Base
{
    class Data
    {
        private SqlConnection con;
        public Data(String path)
        {
            con = new SqlConnection(path);
        }

        public int TransactSelectCount(string cmd)
        {
            return 0;
        }

        public List<KeyValuePair<String, String>> TransactSelectData(string cmd)
        {
            return null;
        }

        public int TransactModify(string cmd)
        {
            return 0;
        }
    }
}
