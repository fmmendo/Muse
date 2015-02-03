﻿using System;
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
    }
}