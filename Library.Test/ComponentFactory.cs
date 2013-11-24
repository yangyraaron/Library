using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Shared.Client.Interfaces;
using Library.Test.Attributes;
using System.ComponentModel.Composition;
using Library.Shared;

namespace Library.Test
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IComponentFactory))]
    public class ComponentFactory:IComponentFactory
    {
        [ImportMany(typeof(IView))]
        public IEnumerable<Lazy<IView,IMetadata<ViewType>>> Views { get; set; }

         [ImportMany(typeof(IPresenter))]
        public IEnumerable<Lazy<IPresenter, IMetadata<PresenterType>>> Presenters { get; set; }

         [ImportMany(typeof(IWorkLoader))]
         public IEnumerable<Lazy<IWorkLoader, IMetadata<WorkLoaderType>>> WorkLoaders { get; set; }

        [Export(typeof(Func<ViewType,IView>))]
        public IView GetView(ViewType viewType)
        {
            Lazy<IView, IMetadata<ViewType>> value = null;
            if (Views.Any())
                value =  Views.SingleOrDefault(x => { return x.Metadata.Key == viewType; });

            if (value == null)
                return null;
            return value.Value;
        }

         [Export(typeof(Func<PresenterType,IPresenter>))]
        public IPresenter GetPresenter(PresenterType presenterType)
        {
            Lazy<IPresenter, IMetadata<PresenterType>> value = null;
            if(Presenters.Any())
                value = Presenters.SingleOrDefault(x=>{return x.Metadata.Key == presenterType;});

            if (value == null)
                return null;
            return value.Value;
        }

         [Export(typeof(Func<WorkLoaderType,IWorkLoader>))]
        public IWorkLoader GetWorkLoader(WorkLoaderType workLoaderType)
        {
            Lazy<IWorkLoader, IMetadata<WorkLoaderType>> value = null;
            if (WorkLoaders.Any())
                value = WorkLoaders.SingleOrDefault(x => { return x.Metadata.Key == workLoaderType; });

            if (value == null)
                return null;
            return value.Value;
        }
    
      
    
        #region IComponentFactory Members

        public T  GetComponent<T>(object key)
        {
 	        if(key is ViewType)
            {
                ViewType viewType =ValueHelper.ConvertToEnum<ViewType,object>(key);
                return (T)GetView(viewType);
            }
            else if(key is PresenterType)
            {
                PresenterType presenterType = ValueHelper.ConvertToEnum<PresenterType,object>(key);
                return (T)GetPresenter(presenterType);
            }
            else
            {
                return (T)GetWorkLoader(ValueHelper.ConvertToEnum<WorkLoaderType, object>(key));
            }

        }

        #endregion
}
}
