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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FractalExplorer
{
    public partial class MainPage : UserControl
    {
        private WriteableBitmap bmp;
        private GridLength prevSidebarWidth;

        public MainPage()
        {
            InitializeComponent();
            bmp = new WriteableBitmap(2*640, 2*480);
            fractalImage.Source = bmp;
            Fractals.WriteMandelbrot(bmp);
            bmp.Invalidate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (layoutGrid.ColumnDefinitions[2].Width.Value > 0)
            {
                prevSidebarWidth = layoutGrid.ColumnDefinitions[2].Width;
                layoutGrid.ColumnDefinitions[2].Width = new GridLength(0);
            }
            else
            {
                layoutGrid.ColumnDefinitions[2].Width = prevSidebarWidth;
            }
        }
    }
}
