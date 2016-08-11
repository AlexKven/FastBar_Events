using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using FastBar_Events.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteConnectionFactory))]
namespace FastBar_Events.Droid
{
    class SQLiteConnectionFactory : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EventDatabase.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}