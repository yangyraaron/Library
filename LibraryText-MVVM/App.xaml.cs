using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace LibraryText_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CompositionContainer _container;

        private void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));

            _container = new CompositionContainer(catalog);

            try
            {
                CompositionBatch batch = new CompositionBatch();
                batch.AddPart(this.MainWindow);

                _container.Compose(batch);
            }
            catch (CompositionException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.MainWindow = new MainWindow();

            Compose();

            this.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_container != null)
                _container.Dispose();
        }
    }
}
