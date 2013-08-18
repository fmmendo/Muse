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

namespace Muse
{
	public partial class NewsPage : PhoneApplicationPage
	{
		public NewsPage()
		{
			InitializeComponent();

           // NewsBrowser.NavigateToString(App.NewsViewModel.Description);
			// Set the data context of the page to the News List
			DataContext = App.MuseService.CurrentItem;


        }

        private void WebBrowser_OnLoaded(object sender, RoutedEventArgs e)
        {

        }


		private void Back_Click(object sender, EventArgs e)
		{
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            DataContext = App.MuseService.CurrentItem;
            //int page = App.NewsViewModel._page;
            //page -= 1;
            //if (page < 0) page = App.ViewModel.Items.Count - 1;

            //App.NewsViewModel._page = page;
            //App.NewsViewModel.LoadPage(page);

           // NewsBrowser.NavigateToString(App.NewsViewModel.Description);
		}

		private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            DataContext = App.MuseService.CurrentItem;
            //int page = App.NewsViewModel._page;
            //page += 1;
            //if (page >= App.ViewModel.Items.Count) page = 0;

            //App.NewsViewModel._page = page;
            //App.NewsViewModel.LoadPage(page);

          //  NewsBrowser.NavigateToString(App.NewsViewModel.Description);
		}

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            ContentPanel.Opacity = 1;
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                if (e.HorizontalVelocity < 0)
                    Next_Click(null, null);


                if (e.HorizontalVelocity > 0)
                    Back_Click(null, null);
            }
        }

        private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                var m = ContentPanel.Margin;
                if (ContentPanel.Opacity > 0 && ContentPanel.Opacity > (Math.Abs(e.HorizontalChange)/100))
                ContentPanel.Opacity -= (Math.Abs(e.HorizontalChange)/100);
                ContentPanel.Margin = new Thickness(m.Left+e.HorizontalChange, m.Top, m.Right-e.HorizontalChange, m.Bottom);
            }
        }

        private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            ContentPanel.Margin = new Thickness(12,0,12,0);
            ContentPanel.Opacity = 1;
        }
	}
}