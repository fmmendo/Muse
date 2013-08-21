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
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Muse
{
    public partial class PhotoPage : PhoneApplicationPage
    {
        public PhotoPage()
        { 
            InitializeComponent();

            DataContext = App.MuseService.CurrentItem;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            DataContext = App.MuseService.CurrentItem;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            DataContext = App.MuseService.CurrentItem;
        }

        //private void Share_Click(object sender, EventArgs e)
        //{
        //    ShareLinkTask shareLinkTask = new ShareLinkTask();
        //    shareLinkTask.Title = "MUSE Photo";
        //    shareLinkTask.LinkUri = new Uri(App.MuseService.CurrentItem.ImageURL, UriKind.Absolute);
        //    shareLinkTask.Message = "Look at this awesome photo from MUSE.";

        //    shareLinkTask.Show();

        //}

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            //ContentPanel.Opacity = 1;
            //if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            //{
            //    if (e.HorizontalVelocity < 0)
            //        Next_Click(null, null);


            //    if (e.HorizontalVelocity > 0)
            //        Back_Click(null, null);
            //}
        }

        private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            //ContentPanel.Margin = new Thickness(12, 0, 12, 0);
            //ContentPanel.Opacity = 1;
        }


        private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {

        }

        private void photo_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // First make sure we're translating and not scaling (one finger vs. two)
            if (e.DeltaManipulation.Scale.X != 0.0 && e.DeltaManipulation.Scale.Y != 0.0)
            {
                double tmp = ScaleTransform.ScaleX * e.DeltaManipulation.Scale.X;

                if (tmp < 1.0)
                    tmp = 1.0;
                else if (tmp > 4.0)
                    tmp = 4.0;

                ScaleTransform.ScaleX = tmp;
                ScaleTransform.ScaleY = tmp;
            }
            else
            {
                Image photo = sender as Image;
                var transformgroup = photo.RenderTransform as TransformGroup;
                var transform = transformgroup.Children.First(c => c is TranslateTransform) as TranslateTransform;

                if (transform != null)
                {

                    // Compute the new X component of the transform
                    double x = transform.X + e.DeltaManipulation.Translation.X;
                    double y = transform.Y + e.DeltaManipulation.Translation.Y;

                    // going left                          and    right side hits right border        move no more!
                    if (e.DeltaManipulation.Translation.X < 0)
                    {
                        if (Application.Current.Host.Content.ActualWidth - photo.ActualWidth * ScaleTransform.ScaleX > 0) return;
                        if (x * ScaleTransform.ScaleX < Application.Current.Host.Content.ActualWidth - photo.ActualWidth * ScaleTransform.ScaleX)
                            x = (Application.Current.Host.Content.ActualWidth / ScaleTransform.ScaleX) - photo.ActualWidth;
                    }
                    // going up                   
                    if (e.DeltaManipulation.Translation.Y < 0)
                    {
                        if ((Application.Current.Host.Content.ActualHeight - 106) - photo.ActualHeight * ScaleTransform.ScaleX > 0) return;
                        if (y * ScaleTransform.ScaleX < (Application.Current.Host.Content.ActualHeight - 106) - photo.ActualHeight * ScaleTransform.ScaleX)
                            y = ((Application.Current.Host.Content.ActualHeight - 106) / ScaleTransform.ScaleX) - photo.ActualHeight;
                    }
                    // going right
                    if (e.DeltaManipulation.Translation.X > 0 && x > 0)// && x * ScaleTransform.ScaleX < Application.Current.Host.Content.ActualWidth - photo.ActualWidth * ScaleTransform.ScaleX)
                        x = 0;//(Application.Current.Host.Content.ActualWidth / ScaleTransform.ScaleX) - photo.ActualWidth;
                    // going down
                    if (e.DeltaManipulation.Translation.Y > 0 && y > 0)// && y * ScaleTransform.ScaleX < Application.Current.Host.Content.ActualHeight - photo.ActualHeight * ScaleTransform.ScaleX)
                        y = 0;//(Application.Current.Host.Content.ActualHeight / ScaleTransform.ScaleX) - photo.ActualHeight;

                    // Apply the computed value to the transform
                    transform.X = x;        // Scale in the X direction 
                    transform.Y = y;
                }
            }
        }

        private void photo_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (e.IsInertial)
            {
                Image photo = sender as Image;

                // Compute the inertial distance to travel
                double dx = e.FinalVelocities.LinearVelocity.X / 10.0;
                double dy = e.FinalVelocities.LinearVelocity.Y / 10.0;

                var transformgroup = photo.RenderTransform as TransformGroup;
                var transform = transformgroup.Children.First(c => c is TranslateTransform) as TranslateTransform;
                if (transform != null)
                {

                    double x = transform.X + dx;
                    double y = transform.Y + dy;


                    // going left                          and    right side hits right border        move no more!
                    if (dx < 0)
                    {
                        if (Application.Current.Host.Content.ActualWidth - photo.ActualWidth * ScaleTransform.ScaleX > 0) return;
                        if (x * ScaleTransform.ScaleX < Application.Current.Host.Content.ActualWidth - photo.ActualWidth * ScaleTransform.ScaleX)
                            x = (Application.Current.Host.Content.ActualWidth / ScaleTransform.ScaleX) - photo.ActualWidth;
                    }
                    // going up                   
                    if (dy < 0)
                    {
                        if ((Application.Current.Host.Content.ActualHeight - 106) - photo.ActualHeight * ScaleTransform.ScaleX > 0) return;
                        if (y * ScaleTransform.ScaleX < (Application.Current.Host.Content.ActualHeight - 106) - photo.ActualHeight * ScaleTransform.ScaleX)
                            y = ((Application.Current.Host.Content.ActualHeight - 106) / ScaleTransform.ScaleX) - photo.ActualHeight;
                    }
                    // going right
                    if (dx > 0 && x > 0)// && x * ScaleTransform.ScaleX < Application.Current.Host.Content.ActualWidth - photo.ActualWidth * ScaleTransform.ScaleX)
                        x = 0;//(Application.Current.Host.Content.ActualWidth / ScaleTransform.ScaleX) - photo.ActualWidth;
                    // going down
                    if (dy > 0 && y > 0)// && y * ScaleTransform.ScaleX < Application.Current.Host.Content.ActualHeight - photo.ActualHeight * ScaleTransform.ScaleX)
                        y = 0;//(Application.Current.Host.Content.ActualHeight / ScaleTransform.ScaleX) - photo.ActualHeight;


                    // Apply the computed value to the animation
                    PanAnimationX.To = x;
                    PanAnimationY.To = y;

                    // Trigger the animation
                    Pan.Begin();
                }
            }
        }


        private void photo_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Pan.Stop();
        }

        /// <summary>
        /// Save picture to library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    MediaLibrary library = new MediaLibrary();
                    library.SavePicture(System.IO.Path.GetFileName(App.MuseService.CurrentItem.ImageURL), ea.Result);
                }
            };
            client.OpenReadAsync(new Uri(App.MuseService.CurrentItem.ImageURL, UriKind.Absolute));
        }
    }
}