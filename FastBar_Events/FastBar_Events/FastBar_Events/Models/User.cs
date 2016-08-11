using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBar_Events.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
        [MaxLength(1024)]
        public string CipheredAccessToken { get; set; }

        //Not stored in the database directly, but automatically ciphers and
        //escapes token and puts it into CipheredAccessToken, which is stored.
        [Ignore]
        public string Token
        {
            get
            {
                return APIManager.CipherToken(DatabaseManager.UnEscape(CipheredAccessToken));
            }
            set
            {
                CipheredAccessToken = DatabaseManager.Escape(APIManager.CipherToken(value));
            }
        }
    }
}
