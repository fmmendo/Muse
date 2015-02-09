using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Telerik.Windows.Controls;
using Windows.ApplicationModel.Background;
using System.Diagnostics;
using MuseBackgroundTask;

namespace Muse.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void RegisterBackgroundTask()
        {
            try
            {
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity || status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity)
                {
                    bool isRegistered = BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == "News push notification");
                    if (!isRegistered)
                    {
                        BackgroundTaskBuilder builder = new BackgroundTaskBuilder
                        {
                            Name = "News push notification",
                            TaskEntryPoint = typeof(NewsNotifierBackgroundTask).FullName
                        };
                        builder.SetTrigger(new TimeTrigger(360, false));
                        //builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                        BackgroundTaskRegistration task = builder.Register();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("The access has already been granted");
            }
        }


        /// <summary>
        /// Navigates to about page.
        /// </summary>
        private void GoToAbout(object sender, Telerik.Windows.Controls.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }

        private void RadDataBoundListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lb = sender as RadDataBoundListBox;
            if (lb == null) return;
            if (lb.RealizedItems.Count() <= 0) return;

            App.MuseService.CurrentItemIndex = lb.RealizedItems.ToList().IndexOf(lb.SelectedItem);
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            NavigationService.Navigate(new Uri("/NewsPage.xaml", UriKind.Relative));
        }

        private void RadDataBoundListBox_Tap_Tour(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lb = sender as RadDataBoundListBox;
            if (lb == null) return;
            if (lb.RealizedItems.Count() <= 0) return;

            App.MuseService.CurrentItemIndex = lb.RealizedItems.ToList().IndexOf(lb.SelectedItem);
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Tour);
            NavigationService.Navigate(new Uri("/TourPage.xaml", UriKind.Relative));
        }

        private void ListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb == null) return;
            if (lb.Items.Count <= 0) return;

            App.MuseService.CurrentItemIndex = lb.SelectedIndex;//lb.Items.IndexOf(lb.SelectedItem);
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            NavigationService.Navigate(new Uri("/PhotoPage.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            RegisterBackgroundTask();
        }
    }
}
