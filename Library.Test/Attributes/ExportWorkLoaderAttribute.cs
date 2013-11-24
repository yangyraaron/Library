using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Library.Shared.Client.Interfaces;

namespace Library.Test.Attributes
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public class ExportWorkLoaderAttribute : ExportAttribute
    {
        public ExportWorkLoaderAttribute():base(typeof(IWorkLoader))
        {

        }

        public WorkLoaderType Key { get; set; }
    }
}
