using System.Windows;

namespace JToolbox.WPF.UI
{
    public class WindowBase : Window
    {
        public WindowBase()
        {
            WindowEvents = new WindowEvents(this);
            WindowEvents.Attach();
        }

        public WindowBase(object dataContext)
            : this()
        {
            DataContext = dataContext;
        }

        public WindowEvents WindowEvents { get; }

        public virtual void OnData(object data)
        {
        }
    }
}