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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KeepPaint : Page
    {
        public KeepPaint()
        {
            this.InitializeComponent();
        }

        LibraryPaint LibraryPaint = new LibraryPaint();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LibraryPaint.Init(ref Display, ref Size, ref Colour);
        }
     

        private void New_Click(object sender, RoutedEventArgs e)
        {
            LibraryPaint.New(Display);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            LibraryPaint.Open(Display);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            LibraryPaint.Save(Display);
        }

        private void Colour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LibraryPaint.Colour(ref Display, ref Colour);
        }

        private void Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LibraryPaint.Size(ref Display, ref Size);
        }
      
  
    }
}
