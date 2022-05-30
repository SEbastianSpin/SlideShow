using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace Animations2
{
    public interface ISlideshowEffect
    {
        string Name { get; }
        void JustEffect(Image imageIn, Image imageOut);
    }
}
