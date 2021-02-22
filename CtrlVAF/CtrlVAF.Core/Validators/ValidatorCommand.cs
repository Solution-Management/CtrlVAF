﻿using CtrlVAF.Models;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validators
{
    public class ValidatorCommand<TConfig>: ICtrlVAFCommand
    {
        public Vault Vault { get; set; }

        public TConfig Configuration { get; set; }
    }

}