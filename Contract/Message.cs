using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public StatusOperation statusOp { get; set; }
        [DataMember]
        public string info { get; set; }
        [DataMember]
        public object[] data { get; set; }
        [DataMember]
        public string operationName { get; set; }
        [DataMember]
        public string tokenApp { get; set; }
        [DataMember]
        public string tokenUser { get; set; }
        [DataMember]
        public string appVersion { get; set; }
        [DataMember]
        public string operationVersion { get; set; }
    }

    public enum StatusOperation
    {
        [EnumMember]
        Waiting,
        [EnumMember]
        Working,
        [EnumMember]
        Finished
    }
}
