using System.Windows;

namespace JToolbox.WPF.UI
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
            DependencyProperty.Register(nameof(Proxy), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}