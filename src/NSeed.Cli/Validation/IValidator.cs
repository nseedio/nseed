using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Validation
{
    public interface IValidator
    {
        ValidationResult Validate();
    }
}
