using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Library.Shared.Client.Interfaces
{
    /// <summary>
    /// this interface defines the enviroment context information
    /// that will used by all the related classes instance at 
    /// runtime
    /// </summary>
    public interface IWorkLoaderContext
    {
        /// <summary>
        /// context infomation container
        /// </summary>
        Hashtable Context { get; }
    }
}
