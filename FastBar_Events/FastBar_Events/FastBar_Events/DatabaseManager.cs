﻿using FastBar_Events.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FastBar_Events
{
    public static class DatabaseManager
    {
        static SQLiteConnection Database;

        public static void Initialize()
        {
            //Initialize the database and create tables.
            Database = DependencyService.Get<ISQLite>().GetConnection();
            Database.CreateTable(typeof(User));
            Database.CreateTable(typeof(Event));
        }

        public static User LoggedInUser
        {
            get
            {
                return Database.Table<User>().FirstOrDefault();
                
            }
            set
            {
                //DeleteAll is not commonly used, but makes sense for this scenario
                Database.DeleteAll<User>();
                //SQLiteCommand cmd = Database.CreateCommand("INSERT INTO User(Email, CipheredAccessToken) VALUES(?, ?);", value.Email, value.CipheredAccessToken );
                //cmd.ExecuteQuery<User>();
                Database.Insert(value);
                //if (value != null)
                //    Database.Insert(value);
                //Again, event details only apply to current user, so they should be
                //deleted when the user changes. If developed in the future to allow more than
                //one logged in user, delete only rows that contain a user not logged in.
                Database.DeleteAll<Event>();
            }
        }

        //Updates events stored in the database with the currently logged in user's token.
        public static void UpdateEvents(IEnumerable<Event> events)
        {
            if (LoggedInUser == null)
                throw new InvalidOperationException("Can't have events without a user.");
            var userId = LoggedInUser.Id;
            foreach (var ev in events)
            {
                ev.UserId = userId;
                Database.InsertOrReplace(ev);
            }
        }

        public static IEnumerable<Event> RetrieveEvents()
        {
            return Database.Query<Event>("select * from Event order by DateTimeStartUtc asc");
        }

        //Tries to update events from logged in user.
        //Returns false if access token is no longer valid.
        public static async Task<bool> TryStoreEventsFromLoggedInUser()
        {
            var user = LoggedInUser;
            if (user == null)
                return false;
            IEnumerable<Event> events = null;
            try
            {
                events = await APIManager.GetEvents(user.Token);
            }
            catch (System.Net.Http.HttpRequestException) { }

            if (events == null)
                return false;
            UpdateEvents(events);
            return true;
        }

        #region Escape Helpers
        //Apparently SQLite has an issue with special characters and doesn't automatically
        //escape strings when passed as part of class fields, so I am reusing code I used
        //in a previous class to manually escape strings that are inserted into databases.
        public static string Escape(this string str, params char[] ignore)
        {
            if (str == null) return null;
            string result = str;
            for (int i = 0; i < EscapableChars.Length; i++)
            {
                if (!ignore.Contains(EscapableChars[i]))
                    result = result.Replace(EscapableChars[i].ToString(), EscapableChars[0].ToString() + EscapeIndices[i].ToString());
            }
            return result;
        }

        public static string UnEscape(this string str)
        {
            if (str == null)
                return null;
            StringBuilder result = new StringBuilder();
            bool escaping = false;
            foreach (var chr in str)
            {
                if (escaping)
                {
                    int i = EscapeIndices.IndexOf(chr);
                    result.Append(EscapableChars[i]);
                    escaping = false;
                }
                else
                {
                    if (chr == EscapableChars[0])
                        escaping = true;
                    else
                        result.Append(chr);
                }
            }
            //for (int i = 0; i < EscapableChars.Length; i++)
            //{
            //    result = result.Replace(EscapableChars[0].ToString() + EscapeIndices[i].ToString(), EscapableChars[i].ToString());
            //}
            return result.ToString();
        }

        public static string EscapableChars = "/!~'_%^|()[]{}`@#?\\\0\"";
        public static string EscapeIndices = "0123456789abcdefghijklmnopqrstuvwxyz";
        #endregion
    }
}
