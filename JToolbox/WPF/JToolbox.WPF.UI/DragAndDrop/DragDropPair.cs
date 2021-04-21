using System;
using System.Windows;

namespace JToolbox.WPF.UI.DragAndDrop
{
    public class DragDropPair
    {
        public DragDropPair(Type sourceTargetType)
            : this(sourceTargetType, sourceTargetType)
        {
        }

        public DragDropPair(Type sourceType, Type targetType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(sourceType))
            {
                throw new ArgumentException("Source type should derive from FrameworkElement");
            }

            if (!typeof(FrameworkElement).IsAssignableFrom(sourceType))
            {
                throw new ArgumentException("Target type should derive from FrameworkElement");
            }

            SourceType = sourceType;
            TargetType = targetType;
        }

        public Type SourceType { get; }
        public Type TargetType { get; }
    }
}