using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FastBar_Events.UWP
{
    class SQLiteConnectionFactory : ISQLite
    {
        public SQLiteConnectionFactory() { }
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EventDatabase.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}
