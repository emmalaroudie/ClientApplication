using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNETServer
{
    class MessagingServer
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider.ServiceStart();
        }
             
    }
}
