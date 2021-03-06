﻿using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using MuseRT.Common;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using System.Diagnostics;
using Windows.ApplicationModel.Appointments;

#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif
#if WINDOWS_PHONE_APP
    using Windows.Phone.UI.Input;
    using MuseRT.Views;
    using Windows.ApplicationModel.Appointments;
    using Windows.Storage;
    using System.Collections.Generic;
    using MuseBackgroundTask;
#endif

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace MuseRT
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        public const string APP_NAME = "Muse";
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        
        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += OnBackPressed;
            RegisterBackgroundTask();
#endif
        }

        static public Frame RootFrame { get; private set; }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Handles back button press.  If app is at the root page of app, don't go back and the
        /// system will suspend the app.
        /// </summary>
        /// <param name="sender">The source of the BackPressed event.</param>
        /// <param name="e">Details for the BackPressed event.</param>
        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            RootFrame = Window.Current.Content as Frame;
            if (RootFrame == null)
            {
                return;
            }

            if (RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
                e.Handled = true;
            }
        }
#endif

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();

                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(RootFrame, "AppFrame");

                // TODO: change this value to a cache size that is appropriate for your application
                RootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            if (RootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!RootFrame.Navigate(typeof(HubPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        private AppointmentStore appointmentStore = null;
        /// <summary>
        /// 
        /// </summary>
        public AppointmentStore AppointmentStore
        {
            get
            {
                if (appointmentStore == null)
                {
                    var t = AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite).AsTask();
                    //t.Start();
                    t.Wait();
                    appointmentStore = t.Result;

                    if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstRun"))
                    {

                        appointmentStore.ChangeTracker.Enable();
                        appointmentStore.ChangeTracker.Reset();

                        ApplicationData.Current.LocalSettings.Values["FirstRun"] = false;
                    }

                }

                return appointmentStore;
            }
            //set { appointmentStore = value; }
        }

        public AppointmentCalendar currentAppCalendar = null;
        /// <summary>
        /// 
        /// </summary>
        public AppointmentCalendar CurrentAppCalendar
        {
            get
            {
                if (currentAppCalendar == null)
                {
                    IReadOnlyList<AppointmentCalendar> appCalendars = AppointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden).AsTask().Result;

                    //AppointmentCalendar appCalendar = null;


                    // Apps can create multiple calendars. This example creates only one.
                    if (appCalendars.Count == 0)
                    {
                        currentAppCalendar = AppointmentStore.CreateAppointmentCalendarAsync("Muse Tour Calendar").AsTask().Result;

                    }
                    else
                    {
                        currentAppCalendar = appCalendars[0];
                    }


                    currentAppCalendar.OtherAppReadAccess = AppointmentCalendarOtherAppReadAccess.Full;
                    currentAppCalendar.OtherAppWriteAccess = AppointmentCalendarOtherAppWriteAccess.SystemOnly;

                    // This app will show the details for the appointment. Use System to let the system show the details.
                    currentAppCalendar.SummaryCardView = AppointmentSummaryCardView.App;

                    var save = currentAppCalendar.SaveAsync().AsTask();
                    //save.Start();
                    save.Wait();

                    //currentAppCalendar = appCalendar;
                }

                return currentAppCalendar;
            }
        }


        private async Task RegisterBackgroundTask()
        {
            try
            {
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity || status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity)
                {
                    bool isRegistered = BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == typeof(NewItemsNotifierBackgroundTask).FullName);
                    if (!isRegistered)
                    {
                        BackgroundTaskBuilder builder = new BackgroundTaskBuilder
                        {
                            Name = typeof(NewItemsNotifierBackgroundTask).FullName,
                            TaskEntryPoint = typeof(NewItemsNotifierBackgroundTask).FullName
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
    }
}