using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace animce
{
	public interface ISlideshowEffect
	{
		string Name { get; }
		void JustEffect(Image imageIn, Image imageOut);
	}
}
