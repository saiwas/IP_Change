using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IP地址切换器.BusinessObjectLayer
{
    class IPAddress
    {
        public string Way
        {
            set;
            get;
        }
        public string Mac
        { 
            get;
            set;
        }
        public string Ip
        {
            get;
            set;
        }
        public string Subnetmask
        {
            get;
            set;
        }
        public string Gateway
        {
            get;
            set;
        }
        public string Dns1
        {
            get;
            set;
        }
        public string Dns2
        {
            get;
            set;
        }
    }
}
