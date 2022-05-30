using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
namespace animce
{
    internal class Opacitycs : ISlideshowEffect
    {

        public string Name => "opacity";

        public void JustEffect(Image imageIn, Image imageOut)
        {
            // Image inside = toshow.Pop();
            Image inside = imageIn;
            //pos++;
            Storyboard Storyfisrt = new Storyboard();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(Animation1, inside);


            Storyboard Story2nd = new Storyboard();
            DoubleAnimation Animation2 = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(UIElement.OpacityProperty));
            //  Storyboard.SetTarget(Animation2, toshow.Peek());
            Storyboard.SetTarget(Animation2, imageOut);

            Storyfisrt.Children.Add(Animation1);
            Storyfisrt.Begin();

            Story2nd.Children.Add(Animation2);
            Story2nd.Begin();
        }
    }
}
