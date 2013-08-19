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
            //if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            //{
            //    var m = ContentPanel.Margin;
            //    if (ContentPanel.Opacity > 0 && ContentPanel.Opacity > (Math.Abs(e.HorizontalChange) / 100))
            //        ContentPanel.Opacity -= (Math.Abs(e.HorizontalChange) / 100);
            //    ContentPanel.Margin = new Thickness(m.Left + e.HorizontalChange, m.Top, m.Right - e.HorizontalChange, m.Bottom);
            //}

            // if is not touch enabled or the scale is different than 1 then don’t allow moving
            if (ImageTransform.ScaleX <= 1.1)
                return;
            double centerX = ImageTransform.CenterX;
            double centerY = ImageTransform.CenterY;
            double translateX = ImageTransform.TranslateX;
            double translateY = ImageTransform.TranslateY;
            double scale = ImageTransform.ScaleX;
            double width = photo.ActualWidth;
            double height = photo.ActualHeight;

            // verify limits to not allow the image to get out of area

            if (centerX - scale * centerX + translateX + e.HorizontalChange < 0 &&
             centerX + scale * (width - centerX) + translateX + e.HorizontalChange > width)
            {
                ImageTransform.TranslateX += e.HorizontalChange;
            }

            if (centerY - scale * centerY + translateY + e.VerticalChange < 0 &&
             centerY + scale * (height - centerY) + translateY + e.VerticalChange > height)
            {
                ImageTransform.TranslateY += e.VerticalChange;
            }

            return;
        }

        private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            ContentPanel.Margin = new Thickness(12, 0, 12, 0);
            ContentPanel.Opacity = 1;
        }

        private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            _scale = ImageTransform.ScaleX;

            Point firstTouch = e.GetPosition(photo, 0);
            Point secondTouch = e.GetPosition(photo, 1);

            _point = new Point(firstTouch.X + (secondTouch.X - firstTouch.X) / 2.0,
                                               firstTouch.Y + (secondTouch.Y - firstTouch.Y) / 2.0);

        }

        private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e)
        {
            // if its less that the original  size or more than 4x then don’t apply
            if (_scale * e.DistanceRatio > 4 || (_scale != 1 && e.DistanceRatio == 1) || _scale * e.DistanceRatio < 1)
                return;

            // if its original size then center it back
            if (e.DistanceRatio <= 1.08)
            {
                ImageTransform.CenterY = 0;
                ImageTransform.CenterY = 0;
                ImageTransform.TranslateX = 0;
                ImageTransform.TranslateY = 0;
            }

            ImageTransform.CenterX = _point.X;
            ImageTransform.CenterY = _point.Y;

            // update the rotation and scaling
            if (this.Orientation == PageOrientation.Landscape)
            {


                // when in landscape we need to zoom faster, if not it looks choppy
                ImageTransform.ScaleX = _scale * (1 + (e.DistanceRatio - 1) * 2);
            }
            else
            {
                ImageTransform.ScaleX = _scale * e.DistanceRatio;
            }
            ImageTransform.ScaleY = ImageTransform.ScaleX;
        }

        private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {

        }
    }
}