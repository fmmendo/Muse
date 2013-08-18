using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Muse
{
	public partial class MainPage : PhoneApplicationPage
	{
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
	}
}