using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Shared.Client.Interfaces
{
    public interface IComponentFactory
    {
        T GetComponent<T>(object key);
    }
}
