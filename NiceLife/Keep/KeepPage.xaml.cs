using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KeepPage : Page
    {
        public KeepPage()
        {
            this.InitializeComponent();
        }
        public LibraryText Library = new LibraryText();
        

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            Bold.IsChecked = Library.Bold(ref Display);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            Italic.IsChecked = Library.Italic(ref Display);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            Underline.IsChecked = Library.Underline(ref Display);
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = Library.Left(ref Display);
            Centre.IsChecked = false;
            Right.IsChecked = false;
        }

        private void Centre_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = false;
            Centre.IsChecked = Library.Centre(ref Display);
            Right.IsChecked = false;
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = false;
            Centre.IsChecked = false;
            Right.IsChecked = Library.Right(ref Display);
        }

        private void Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Library.Size(ref Display, ref Size);
        }

        private void Colour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Library.Colour(ref Display, ref Colour);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Library.New(Display);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Library.Open(Display);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Library.Save(Display);
        }
        private void Paint_Click(object sender, RoutedEventArgs e)
        {
            KeepPageFrame.Navigate(typeof(KeepPaint));
        }
      
    }
}
