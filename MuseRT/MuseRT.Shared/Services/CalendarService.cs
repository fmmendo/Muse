using System;
using System.Threading.Tasks;

#if WINDOWS_PHONE_APP
using Windows.ApplicationModel.Appointments;
using Windows.Storage;
using Windows.UI.Popups;

#endif

namespace MuseRT.Services
{
    public class CalendarService
    {
        private static AppointmentStore appointmentStore = null;
        private static AppointmentCalendar currentAppCalendar = null;

        /// <summary>
        /// 
        /// </summary>
        private static async Task<AppointmentStore> GetAppointmentStore()
        {
            if (appointmentStore == null)
            {
                appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);

                if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstRun"))
                {
                    appointmentStore.ChangeTracker.Enable();
                    appointmentStore.ChangeTracker.Reset();

                    ApplicationData.Current.LocalSettings.Values["FirstRun"] = false;
                }
            }

            return appointmentStore;
        }

        /// <summary>
        /// 
        /// </summary>
        private static async Task<AppointmentCalendar> GetCurrentAppCalendar()
        {
            if (currentAppCalendar == null)
            {
                var store = await GetAppointmentStore();

                var appCalendars = await store.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);

                if (appCalendars.Count == 0)
                    currentAppCalendar = await store.CreateAppointmentCalendarAsync("Muse Tour Calendar");
                else
                    currentAppCalendar = appCalendars[0];

                currentAppCalendar.OtherAppReadAccess = AppointmentCalendarOtherAppReadAccess.Full;
                currentAppCalendar.OtherAppWriteAccess = AppointmentCalendarOtherAppWriteAccess.SystemOnly;

                // This app will show the details for the appointment. Use System to let the system show the details.
                currentAppCalendar.SummaryCardView = AppointmentSummaryCardView.App;

                await currentAppCalendar.SaveAsync();
            }

            return currentAppCalendar;
        }

        public static async Task AddToCalendarAsync(string subject, DateTime date, bool allDay = true)
        {
            try
            {
                Appointment newAppointment = new Appointment();

                newAppointment.Subject = "MUSE: " + subject;
                newAppointment.StartTime = date;
                newAppointment.AllDay = allDay;
                //newAppointment.Location = "here!";
                //newAppointment.RoamingId = "984756";

                //save appointment to calendar
                var calendar = await GetCurrentAppCalendar();
                await calendar.SaveAppointmentAsync(newAppointment);

                var md = new MessageDialog(String.Format("\"{0}\" on {1} was successfully added to your calendar.", subject, date.ToString("dd MMM yyyy")));
                md.ShowAsync();
            }
            catch (Exception)
            {
                var md = new MessageDialog("Couldn't add tour date to calendar");
                md.ShowAsync();
            }
        }
    }
}
