using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
namespace animce
{
    internal class Vertical : ISlideshowEffect
    {
        public string Name => "vertical";

        public void JustEffect(Image imageIn, Image imageOut)
        {
            Image inside = imageIn;
            // pos++;
            Storyboard Storyfisrt = new Storyboard();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 768, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(FrameworkElement.HeightProperty));
            ThicknessAnimation Thickness1 = new ThicknessAnimation(new Thickness(0, 768, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 1, 250), 0);
            Storyboard.SetTargetProperty(Thickness1, new PropertyPath(FrameworkElement.MarginProperty));

            Storyboard.SetTarget(Thickness1, inside);
            Storyboard.SetTarget(Animation1, inside);

            Storyboard Story2nd = new Storyboard();
            DoubleAnimation Animation2 = new DoubleAnimation(768, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTarget(Animation2, imageOut);


            Storyfisrt.Children.Add(Animation1);
            Storyfisrt.Children.Add(Thickness1);
            Storyfisrt.Begin();
            Story2nd.Children.Add(Animation2);
            Story2nd.Begin();
        }
    }
}
