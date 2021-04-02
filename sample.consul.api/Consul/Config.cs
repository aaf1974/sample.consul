using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample.consul.api.Consul
{
    public class Config
    {
        public string Uri { get; set; }

        public string Table { get; set; }
        public string ServiceUrl { get; internal set; }
    }
}
