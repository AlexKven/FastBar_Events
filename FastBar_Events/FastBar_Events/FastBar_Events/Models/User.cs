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
                //return CipheredAccessToken;
                //return CipheredAccessToken == null ? null : APIManager.CipherToken(CipheredAccessToken);
                //if (CipheredAccessToken == null) return null;
                System.Diagnostics.Debug.WriteLine("\n\nr2: " + CipheredAccessToken + "\n\n");
                var db = DatabaseManager.UnEscape(CipheredAccessToken);
                System.Diagnostics.Debug.WriteLine("\n\nr1: " + db + "\n\n");
                var result = APIManager.CipherToken(db);
                System.Diagnostics.Debug.WriteLine("\n\nr0: " + result + "\n\n");
                return result;
            }
            set
            {
                //CipheredAccessToken = value;
                //return;
                //CipheredAccessToken = value == null ? null : APIManager.CipherToken(value);
                //return;
                System.Diagnostics.Debug.WriteLine("\n\nw0: " + value + "\n\n");
                var ciphered = value == null ? null : APIManager.CipherToken(value);
                System.Diagnostics.Debug.WriteLine("\n\nw1: " + ciphered + "\n\n");
                if (value != null && APIManager.CipherToken(ciphered) != value)
                {

                }
                CipheredAccessToken = value == null ? null : DatabaseManager.Escape(ciphered);
                System.Diagnostics.Debug.WriteLine("\n\nw2: " + CipheredAccessToken + "\n\n");
            }
        }
    }
}
