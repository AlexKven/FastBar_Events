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
        //Purpose of this property is easy retrieval of actual token by application.
        [Ignore]
        public string Token
        {
            get
            {
                //Converts the database-friendly token (which is XOR ciphered and escaped) to the raw token sent to the server.
                var db = DatabaseManager.UnEscape(CipheredAccessToken);
                return APIManager.CipherToken(db);
            }
            set
            {
                //Converts the raw token to a database suitable token (it is XOR ciphered and escaped).
                var ciphered = value == null ? null : APIManager.CipherToken(value);
                CipheredAccessToken = value == null ? null : DatabaseManager.Escape(ciphered);
            }
        }
    }
}
