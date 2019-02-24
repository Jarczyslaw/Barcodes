using System.Windows;

namespace Barcodes.Core.UI
{
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Proxy
        {
            get { return GetValue(ProxyProperty); }
            set { SetValue(ProxyProperty, value); }
        }

        public static readonly DependencyProperty ProxyProperty =
            DependencyProperty.Register("Proxy", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
