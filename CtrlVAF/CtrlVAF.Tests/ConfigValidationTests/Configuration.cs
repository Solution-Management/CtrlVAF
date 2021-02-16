using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.ConfigValidationTests
{
    class Configuration
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public Child_Configuration ChildConfig { get; set; }
    }

    class Child_Configuration
    {
        public string Name { get; set; }
    }
}
