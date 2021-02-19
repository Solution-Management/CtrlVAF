using MFiles.VAF.Common;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Models
{
    public interface IEventHandlerCommand<T>: IEventHandlerCommand
    {
        EventHandlerEnvironment Env { get; set; }
        T Configuration { get; set; }
    }

    public interface IEventHandlerCommand
    {

    }
}
