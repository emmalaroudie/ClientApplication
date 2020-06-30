using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Job
{
    class ResultReturner
    {
        public Message ReturnResult(Message msg)
        {
            msg.info = "Demande de la réponse du décryptage reçue ";
            return msg;
        }
    }
}
