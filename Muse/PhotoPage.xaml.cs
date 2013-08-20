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
    public partial class PhotoPage : PhoneApplicationPage
    {
        Point _point;
        double _scale;

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

        //private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        //{
        //    //if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
        //    //{
        //    //    var m = ContentPanel.Margin;
        //    //    if (ContentPanel.Opacity > 0 && ContentPanel.Opacity > (Math.Abs(e.HorizontalChange) / 100))
        //    //        ContentPanel.Opacity -= (Math.Abs(e.HorizontalChange) / 100);
        //    //    ContentPanel.Margin = new Thickness(m.Left + e.HorizontalChange, m.Top, m.Right - e.HorizontalChange, m.Bottom);
        //    //}

        //    // if is not touch enabled or the scale is different than 1 then don’t allow moving
        //    if (ImageTransform.ScaleX <= 1.1)
        //        return;
        //    double centerX = ImageTransform.CenterX;
        //    double centerY = ImageTransform.CenterY;
        //    double translateX = ImageTransform.TranslateX;
        //    double translateY = ImageTransform.TranslateY;
        //    double scale = ImageTransform.ScaleX;
        //    double width = photo.ActualWidth;
        //    double height = photo.ActualHeight;

        //    // verify limits to not allow the image to get out of area

        //    if (centerX - scale * centerX + translateX + e.HorizontalChange < 0 &&
        //     centerX + scale * (width - centerX) + translateX + e.HorizontalChange > width)
        //    {
        //        ImageTransform.TranslateX += e.HorizontalChange;
        //    }

        //    if (centerY - scale * centerY + translateY + e.VerticalChange < 0 &&
        //     centerY + scale * (height - centerY) + translateY + e.VerticalChange > height)
        //    {
        //        ImageTransform.TranslateY += e.VerticalChange;
        //    }

        //    return;
        //}

        private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            //ContentPanel.Margin = new Thickness(12, 0, 12, 0);
            //ContentPanel.Opacity = 1;
        }

        //private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        //{
        //    _scale = ImageTransform.ScaleX;

        //    Point firstTouch = e.GetPosition(photo, 0);
        //    Point secondTouch = e.GetPosition(photo, 1);

        //    _point = new Point(firstTouch.X + (secondTouch.X - firstTouch.X) / 2.0,
        //                                       firstTouch.Y + (secondTouch.Y - firstTouch.Y) / 2.0);

        //}

        //private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e)
        //{
        //    // if its less that the original  size or more than 4x then don’t apply
        //    if (_scale * e.DistanceRatio > 4 || (_scale != 1 && e.DistanceRatio == 1) || _scale * e.DistanceRatio < 1)
        //        return;

        //    // if its original size then center it back
        //    if (e.DistanceRatio <= 1.08)
        //    {
        //        ImageTransform.CenterY = 0;
        //        ImageTransform.CenterY = 0;
        //        ImageTransform.TranslateX = 0;
        //        ImageTransform.TranslateY = 0;
        //    }

        //    ImageTransform.CenterX = _point.X;
        //    ImageTransform.CenterY = _point.Y;

        //    // update the rotation and scaling
        //    if (this.Orientation == PageOrientation.Landscape)
        //    {


        //        // when in landscape we need to zoom faster, if not it looks choppy
        //        ImageTransform.ScaleX = _scale * (1 + (e.DistanceRatio - 1) * 2);
        //    }
        //    else
        //    {
        //        ImageTransform.ScaleX = _scale * e.DistanceRatio;
        //    }
        //    ImageTransform.ScaleY = ImageTransform.ScaleX;
        //}

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
                        if ((Application.Current.Host.Content.ActualHeight - 206) - photo.ActualHeight * ScaleTransform.ScaleX > 0) return;
                        if (y * ScaleTransform.ScaleX < (Application.Current.Host.Content.ActualHeight - 206) - photo.ActualHeight * ScaleTransform.ScaleX)
                            y = ((Application.Current.Host.Content.ActualHeight - 206) / ScaleTransform.ScaleX) - photo.ActualHeight;
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
                        if ((Application.Current.Host.Content.ActualHeight - 206) - photo.ActualHeight * ScaleTransform.ScaleX > 0) return;
                        if (y * ScaleTransform.ScaleX < (Application.Current.Host.Content.ActualHeight - 206) - photo.ActualHeight * ScaleTransform.ScaleX)
                            y = ((Application.Current.Host.Content.ActualHeight - 206) / ScaleTransform.ScaleX) - photo.ActualHeight;
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
    }
}