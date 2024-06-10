using RenameFile.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RenameFile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// アプリケーションのエントリーポイント
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vw = new RenameFileView();
            vw.ShowDialog();
        }
    }

}
