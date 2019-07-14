﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Validation
{
    internal interface IValidator<TCommand>
    {
        ValidationResult Validate(TCommand command);
    }
}
