using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SlideShow
{
    public interface ISlideshowEffect
    {
        string Name { get; }
        void JustEffect(Image imageIn, Image imageOut);
    }
}
