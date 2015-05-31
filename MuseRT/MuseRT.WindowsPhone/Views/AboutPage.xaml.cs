using MuseRT.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MuseRT.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
            AboutModel = new AboutViewModel();
            this.DataContext = this;
        }

        public AboutViewModel AboutModel { get; private set; }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=12d59edb-509f-e011-986b-78e7d1fa76f8"));
        }

        private async void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=feedback@fmendo.com&subject=Feedback: Muse");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
    }
}
