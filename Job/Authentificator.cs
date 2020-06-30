using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job
{
    class Authentificator
    {
        public string Authenticate(string login, string password, string tokenApp)
        {
            string tokenUser=null;
            if(login == "login" && password == "password" && tokenApp == "tokenApp")
            {
                 tokenUser = "tokenUser récupéré ";
            }
            else
            {

            }
            
            return tokenUser;
        }
    }
}
