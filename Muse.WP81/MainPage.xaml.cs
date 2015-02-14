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
using System.Windows.Navigation;
using Windows.ApplicationModel.Appointments;
using Windows.Storage;
using System.Threading.Tasks;

namespace Muse.WP81
{
    public partial class MainPage : PhoneApplicationPage
    {
        private AppointmentStore appointmentStore;
        private AppointmentCalendar currentAppCalendar;
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
            RegisterBackgroundTask();
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

                        builder.SetTrigger(new TimeTrigger(1440, false));//every 24h
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);

            //if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstRun"))
            //{

            //    await CheckForAndCreateAppointmentCalendars();
            //    //await SyncAppointmentsFromAppServer();

            //    appointmentStore.ChangeTracker.Enable();
            //    appointmentStore.ChangeTracker.Reset();

            //    ApplicationData.Current.LocalSettings.Values["FirstRun"] = false;
            //}
        }

        //async public Task CreateNewAppointment()
        //{
        //    Appointment newAppointment = new Appointment();

        //    newAppointment.Subject = "test";
        //    newAppointment.StartTime = DateTime.Now.AddDays(5);
        //    newAppointment.Duration = TimeSpan.FromHours(2);
        //    newAppointment.Location = "here!";
        //    newAppointment.RoamingId = "984756";

        //    //save appointment to calendar
        //    await currentAppCalendar.SaveAppointmentAsync(newAppointment);
        //}

        //async public Task CheckForAndCreateAppointmentCalendars()
        //{

        //    IReadOnlyList<AppointmentCalendar> appCalendars = await appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);

        //    AppointmentCalendar appCalendar = null;


        //    // Apps can create multiple calendars. This example creates only one.
        //    if (appCalendars.Count == 0)
        //    {
        //        appCalendar = await appointmentStore.CreateAppointmentCalendarAsync("Example App Calendar");

        //    }
        //    else
        //    {
        //        appCalendar = appCalendars[0];
        //    }


        //    appCalendar.OtherAppReadAccess = AppointmentCalendarOtherAppReadAccess.Full;
        //    appCalendar.OtherAppWriteAccess = AppointmentCalendarOtherAppWriteAccess.SystemOnly;

        //    // This app will show the details for the appointment. Use System to let the system show the details.
        //    appCalendar.SummaryCardView = AppointmentSummaryCardView.App;

        //    await appCalendar.SaveAsync();

        //    currentAppCalendar = appCalendar;
        //}


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

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {

            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}
