using Barcodes.Core.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Barcodes.Core.UI.Views
{
    public partial class ShellView : UserControl
    {
        private PresentationSource presentationSource;

        public ShellView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            presentationSource = PresentationSource.FromVisual((Visual)sender);
            presentationSource.ContentRendered += PresentationSource_ContentRendered;
        }

        private void PresentationSource_ContentRendered(object sender, EventArgs e)
        {
            OnViewShown();
            presentationSource.ContentRendered -= PresentationSource_ContentRendered;
        }

        private void OnViewShown()
        {
            (DataContext as IShownAware)?.OnViewShown();
        }
    }
}
