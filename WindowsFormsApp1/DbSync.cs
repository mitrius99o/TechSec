using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.Linq;

namespace WindowsFormsApp1
{
    public class DbSync
    {
        public static DataContext db;

        public static IQueryable<Abiturient> selectedGroup;
        public static void SetDbContext(string connString)
        {
            db = new DataContext(connString);
        }
    }
}
