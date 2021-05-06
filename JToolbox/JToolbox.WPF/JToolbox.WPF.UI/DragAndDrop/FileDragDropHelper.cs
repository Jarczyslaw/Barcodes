using JToolbox.WPF.Core.Awareness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JToolbox.WPF.UI.DragAndDrop
{
    public delegate void OnFileDrag(UiFileDragArgs args);

    public delegate void OnFileDrop(UiFileDropArgs args);

    public class FileDragDropHelper : BaseDragDropHelper
    {
        private readonly List<Type> fileDragSources;
        private readonly List<Type> fileDropTargets;

        public event OnFileDrag OnFileDrag;

        public event OnFileDrop OnFileDrop;

        public FileDragDropHelper(FrameworkElement containerElement, Type fileDragSource, Type fileDropTarget)
            : this(containerElement, new List<Type> { fileDragSource }, new List<Type> { fileDropTarget })
        {
        }

        public FileDragDropHelper(FrameworkElement containerElement, List<Type> fileDragSources, List<Type> fileDropTargets)
            : base(containerElement)
        {
            this.fileDragSources = fileDragSources;
            this.fileDropTargets = fileDropTargets;
        }

        protected override string Key => DataFormats.FileDrop;
        public string AdditionalKey => nameof(FileDragDropHelper);

        private void CallOnDragChain(UiFileDragArgs args)
        {
            OnFileDrag?.Invoke(args);
            if (args.Files?.Count > 0)
            {
                return;
            }

            if (args.Element.DataContext is IFileDragDropAware parentAware)
            {
                parentAware.OnFileDrag(args);
                if (args.Files?.Count > 0)
                {
                    return;
                }
            }

            if (args.Element != containerElement && containerElement.DataContext is IFileDragDropAware elementAware)
            {
                elementAware.OnFileDrag(args);
            }
        }

        private void CallOnDropChain(UiFileDropArgs args)
        {
            args.Handled = false;
            OnFileDrop?.Invoke(args);
            if (args.Handled)
            {
                return;
            }

            if (args.Element.DataContext is IFileDragDropAware parentAware)
            {
                parentAware.OnFilesDrop(args);
                if (args.Handled)
                {
                    return;
                }
            }

            if (args.Element != containerElement && containerElement.DataContext is IFileDragDropAware elementAware)
            {
                elementAware.OnFilesDrop(args);
            }
        }

        protected override void DragStart(object sender, MouseEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            if (Utils.FindParentOfTypes(source, fileDragSources) is FrameworkElement parent)
            {
                var args = new UiFileDragArgs
                {
                    Element = parent,
                    Source = parent.DataContext
                };
                CallOnDragChain(args);
                if (args.Files?.Count > 0)
                {
                    var dataObject = new DataObject(DataFormats.FileDrop, args.Files.ToArray());
                    dataObject.SetData(AdditionalKey, new object());
                    DragDrop.DoDragDrop(source, dataObject, DragDropEffects.Move);
                    foreach (var file in args.Files)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                    }
                    startPosition = null;
                }
            }
        }

        protected override void DropStart(object sender, DragEventArgs e)
        {
            var target = e.OriginalSource as DependencyObject;
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var fromHelper = e.Data.GetData(AdditionalKey) != null;
            if (files?.Length > 0 && !fromHelper && Utils.FindParentOfTypes(target, fileDropTargets) is FrameworkElement parent)
            {
                var args = new UiFileDropArgs
                {
                    Files = files.ToList(),
                    Element = parent,
                    Target = parent.DataContext
                };
                CallOnDropChain(args);
            }
        }
    }
}