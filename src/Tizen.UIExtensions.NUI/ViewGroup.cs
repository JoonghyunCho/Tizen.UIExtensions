using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{

    /// <summary>
    /// A ViewGroup provides a class which can be a container for other controls.
    /// </summary>
    /// <remarks>
    /// This class is used as a container view for Layouts from Xamarin.Forms.Platform.Tizen framework.
    /// It is used for implementing xamarin pages and layouts.
    /// </remarks>
    public class ViewGroup : View, IContainable<View>
    {
        readonly ObservableCollection<View> _children = new ObservableCollection<View>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroup"/> class.
        /// </summary>
        /// <remarks>ViewGroup doesn't support replacing its children, this will be ignored.</remarks>
        public ViewGroup()
        {
            Layout = new LayoutItem();
            WidthResizePolicy = ResizePolicyType.FillToParent;
            HeightResizePolicy = ResizePolicyType.FillToParent;
            Relayout += OnRelayout;
            _children.CollectionChanged += OnCollectionChanged;
        }

        IList<View> IContainable<View>.Children => _children;

        /// <summary>
        /// Gets list of native elements that are placed in the ViewGroup.
        /// </summary>
        public new IList<View> Children => _children;

        /// <summary>
        /// Notifies that the layout has been updated.
        /// </summary>
        public event EventHandler<LayoutEventArgs> LayoutUpdated;

        void OnRelayout(object sender, EventArgs e)
        {
            LayoutUpdated?.Invoke(this, new LayoutEventArgs
            {
                Geometry = new Rect(Position.X, Position.Y, Size.Width, Size.Height)
            });
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var v in e.NewItems)
                {
                    if (v is View view)
                    {
                        Add(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var v in e.OldItems)
                {
                    if (v is View view)
                    {
                        Remove(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var child in base.Children.ToList())
                {
                    Remove(child);
                }
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // do not dispose the child, just remove from children, it is a NUI rule
                _children.Clear();
            }
            base.Dispose(disposing);
        }
    }
}