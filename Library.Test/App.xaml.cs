using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using Library.Shared.Client.Interfaces;
using System.ComponentModel;

namespace Library.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [Import]
        public IComponentFactory ComonentFactory { get; set; }

        [Import(typeof(Func<WorkLoaderType,IWorkLoader>))]
        public Func<WorkLoaderType, IWorkLoader> GetWorkLoaderFunc { get; set; }

        private CompositionContainer _container;
        private IWorkLoader _workLoader;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Compose())
            {
                this.MainWindow = new MainWindow();

                InitializeResources();
                InitializeStartupWorkLoader();

                MainWindow.Show();
            }
            else
                Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_workLoader != null&&MainWindow!=null)
            {
                MainWindow.Closing -= new CancelEventHandler(
                    (o, ce) => { _workLoader.Close(); });
            }

            if (_container != null)
            {
                _container.Dispose();
            }
        }

        private void InitializeResources()
        {
            var resource = new ResourceDictionary();
            resource.Source = new Uri("Themes/WhistlerBlue.xaml", UriKind.RelativeOrAbsolute);
            this.Resources.MergedDictionaries.Add(resource);
        }

        private void InitializeStartupWorkLoader()
        {
            //_workLoader = ComonentFactory.GetComponent<IWorkLoader>(WorkLoaderType.SignInWorkLoader);
            if (GetWorkLoaderFunc == null)
                return;
            _workLoader = GetWorkLoaderFunc(WorkLoaderType.SignInWorkLoader);
            if (_workLoader == null)
                return;
            _workLoader.ComponentFactory = this.ComonentFactory;
            _workLoader.UIManager = MainWindow as IUIManager;
            _workLoader.Run();

            MainWindow.Closing += new CancelEventHandler(
                (o, ce) => { _workLoader.Close(); });
        }

        private bool Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));

            _container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddPart(this);

            try
            {
                _container.Compose(batch);
            }
            catch(CompositionException ex)
            {
                MessageBox.Show(ex.ToString());
                Shutdown(1);

                return false;
            }

            return true;
        }
    }
}
