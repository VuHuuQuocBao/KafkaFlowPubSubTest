using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    [DataContract]
    public class EmailEvents
    {
        [DataMember(Order = 1)]
        public int id {  get; set; }
        [DataMember(Order = 2)]
        public string message { get; set; }

    }
}
