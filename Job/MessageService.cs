using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Contract;

namespace Job
{
    public class MessageService : IMessageService
    {
        private Authentificator authentificator;
        private Decipherer decipherer;
        private string tokenApp;
        private string tokenUser;

        public Message Servicing(Message msg)
        {
            Console.WriteLine($"Demande entrante : {msg.info}");
            switch (msg.operationName)
            {
                case "authentificate":
                    tokenUser = StartAuthenticate(msg);
                    msg.tokenUser = tokenUser;
                    break;
                case "decipher":
                    StartDecipher(msg);
                    break;
                case "returnResult":
                    //TODO : voir comment gérer l'objet returnResult(attribut privé classe, attribut local d'une méthode?)
                    Console.WriteLine($"{msg.info}");

                    //vérifier dans la base s'il existe un travail en cours
                    // corresppondant au tokenuser et au tokenapp du message.
                    // Renvoyer le status de l'OP.
                    msg.statusOp = StatusOperation.Working;
                    msg.info = "retour de la demande de résultat ";
                    break;
                default:
                    msg.info = "Invalid operation";
                    break;
            }
            Console.WriteLine($"{msg.info}");

            return msg;
        }

        private string StartAuthenticate(Message msg)
        {
            string login = msg.data[0].ToString();
            string password = msg.data[1].ToString();
            //this.tokenApp = msg.data[2].ToString();
            this.tokenApp = msg.tokenApp;
            authentificator = new Authentificator();

            return authentificator.Authenticate(login, password, tokenApp) ;
        }

        private Message StartDecipher(Message msg)
        {
            decipherer = new Decipherer();
            return decipherer.Decipher(msg);
        }
    }
}
