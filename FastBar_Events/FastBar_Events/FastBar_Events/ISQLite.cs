using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBar_Events
{
    public interface ISQLite
    {
        //An interface for handling SQLite initialization across platforms.
        SQLiteConnection GetConnection();
    }
}
