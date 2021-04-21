using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace JToolbox.WPF.UI
{
    public static class Utils
    {
        public static T FindParentOfType<T>(DependencyObject current)
            where T : DependencyObject
        {
            return FindParentOfType(current, typeof(T)) as T;
        }

        public static object FindParentOfType(DependencyObject current, Type parentType)
        {
            return FindParentOfTypes(current, new List<Type> { parentType });
        }

        public static object FindParentOfTypes(DependencyObject current, List<Type> parentTypes)
        {
            if (parentTypes?.Count > 0)
            {
                do
                {
                    if (parentTypes.Contains(current.GetType()))
                    {
                        return current;
                    }
                    current = VisualTreeHelper.GetParent(current);
                }
                while (current != null);
            }
            return null;
        }

        public static FrameworkElement FindParentFrameworkElement(DependencyObject current)
        {
            do
            {
                if (typeof(FrameworkElement).IsAssignableFrom(current.GetType()))
                {
                    return (FrameworkElement)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}