using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Test.Attributes
{
    public interface IViewMetadata
    {
         ViewType View { get; }
    }

    public interface IPresenterMetadata
    {
        PresenterType Presenter { get; }
    }

    public interface IWorkLoaderMetadata
    {
        WorkLoaderType WorkLoader{ get; }
    }

    public interface IMetadata<T>
    {
        T Key { get; }
    }
}
