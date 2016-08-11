using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBar_Events.Models
{
    public class Event
    {
        //Same class used for JSON deserialization and SQLite database operations
        //SQL attributes were determined by my best guess, but ideally I would have
        //designed the database schema based on a list of requirements from my client
        [PrimaryKey]
        public int EventId { get; set; }
        public string EventKey { get; set; }
        public int BarOperatorUserId { get; set; }
        public string Name { get; set; }
        public DateTime DateTimeStartUtc { get; set; }
        public DateTime DateTimeEndUtc { get; set; }
        public string CloudinaryPublicImageId { get; set; }

        [Ignore]
        public DateTime DateTimeStartLocal
        {
            get { return DateTimeStartUtc.ToLocalTime(); }
        }
        [Ignore]
        public DateTime DateTimeEndLocal
        {
            get { return DateTimeEndUtc.ToLocalTime(); }
        }

        //Foreign key to user table. Currently not important, but inclusion in the
        //schema now would allow the app to be expanded to handle multiple users
        //in the future. Currently, UserId is set automatically by DatabaseManager.
        public int UserId { get; set; }
    }
}
