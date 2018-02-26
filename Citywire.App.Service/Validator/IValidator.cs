using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citywire.App.Service
{
    public interface IValidator<in T>
    {
        bool Validate(T obj);
    }
}
