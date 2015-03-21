using MuseRT.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;

namespace MuseRT.Services
{
    public class NavigationServices
    {
        static public void NavigateToPage(string pageName, object parameter = null)
        {
            try
            {
                string pageTypeName = String.Format("{0}.{1}", typeof(HubPage).Namespace, pageName);
                Type pageType = Type.GetType(pageTypeName);
                App.RootFrame.Navigate(pageType, parameter);
            }
            catch (Exception ex)
            {
                Logger.WriteError("NavigationServices.NavigateToPage", ex);
            }
        }

        static public async void NavigateTo(Uri uri)
        {
            try
            {
                await Launcher.LaunchUriAsync(uri);
            }
            catch (Exception ex)
            {
                Logger.WriteError("NavigationServices.NavigateTo", ex);
            }
        }
    }
}
