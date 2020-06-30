using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract
{
    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        Message Servicing(Message msg);


    }

}
