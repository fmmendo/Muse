using System;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Input;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace Muse
{
	public partial class MainPage : PhoneApplicationPage
	{
        private const string LandingBitFileName = "MuseAppFirstLaunch";
		/// <summary>
		/// Constructor
		/// </summary>
		public MainPage()
		{
			InitializeComponent();

			// Set the data context of the listbox control
			DataContext = App.ViewModel;
			this.Loaded += new RoutedEventHandler(MainPage_Loaded);
		}

		/// <summary>
		/// Load data for the ViewModel Items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
            if (!HasUserSeenIntro())
            {
                MessageBox.Show("Hey Guys!\n\nThanks for downloading the App (and being patient with a few bugs :) )\n\nThis new build has a few more fixes and a few new bits, and I'm currently working on a big overhaul of the app so feel free to leave comments and suggestions in the ratings!");
            }
		}

		#region Gesture Handlers
		/// <summary>
		/// Handles a tap event on an element of the News list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GestureListener_Tap(object sender, GestureEventArgs e)
		{
			ListBox lb = sender as ListBox;
			if (lb == null) return;
			if (lb.Items.Count <= 0) return;

            App.MuseService.CurrentItemIndex = lb.SelectedIndex;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
			NavigationService.Navigate(new Uri("/NewsPage.xaml", UriKind.Relative));
		}

		/// <summary>
		/// Handles a tap event on an element of the Tour Dateslist
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GestureListener_Tap_tour(object sender, GestureEventArgs e)
		{
			ListBox lb = sender as ListBox;
			if (lb == null) return;
			if (lb.Items.Count <= 0) return;

            App.MuseService.CurrentItemIndex = lb.SelectedIndex;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Tour);
			NavigationService.Navigate(new Uri("/TourPage.xaml", UriKind.Relative));
		}

		/// <summary>
		/// Handles a tap event on an element of the gallery
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GestureListener_Tap_photo(object sender, GestureEventArgs e)
		{
			ListBox lb = sender as ListBox;
			if (lb == null) return;
			if (lb.Items.Count <= 0) return;

            App.MuseService.CurrentItemIndex = lb.SelectedIndex;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
			NavigationService.Navigate(new Uri("/PhotoPage.xaml", UriKind.Relative));

		}
		#endregion


        private static bool hasSeenIntro;

        /// <summary>Will return false only the first time a user ever runs this.
        /// Everytime thereafter, a placeholder file will have been written to disk
        /// and will trigger a value of true.</summary>
        public static bool HasUserSeenIntro()
        {
            if (hasSeenIntro) return true;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists(LandingBitFileName))
                {
                    // just write a placeholder file one byte long so we know they've landed before
                    using (var stream = store.OpenFile(LandingBitFileName, System.IO.FileMode.Create))
                    {
                        stream.Write(new byte[] { 1 }, 0, 1);
                    }
                    return false;
                }

                hasSeenIntro = true;
                return true;
            }
        }
	}
}