using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests
{
    public class Configuration
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public Child_Configuration ChildConfig { get; set; }
    }

    public class Child_Configuration
    {
        public string Name { get; set; }
        public GrandChild_Configuration GrandChildConfig { get; set; }
    }

    public class GrandChild_Configuration
    {
        public string Name { get; set; }
    }
}
