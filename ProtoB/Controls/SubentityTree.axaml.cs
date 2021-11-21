using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProtoB.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProtoB.Controls
{
    /// <summary>Control for a treeview of a subentity parent hierarchy</summary>
    // TODO: Increase efficiency here, if possible. Having a root property and a
    //  seperate rootaslist property is a little too hacky
    public partial class SubentityTree : UserControl
    {
        private IEnumerable _root = new List<SubentityViewModel>(1);

        public IEnumerable Root
        {
            get => _root;
            set => SetAndRaise(ItemsProperty, ref _root, value);
        }

        public static readonly DirectProperty<SubentityTree, IEnumerable> ItemsProperty =
            ItemsControl.ItemsProperty.AddOwner<SubentityTree>(
                o => o.Root,
                (o, v) => o.Root = v
            );

        public SubentityTree(SubentityViewModel? root)
        {
            DataContext = this;
            InitializeComponent();
            if (root != null)
                ((List<SubentityViewModel>)Root).Add(root);
        }

        public string Tmp => ((List<SubentityViewModel>)Root)[0].EditorName;

        public SubentityTree()
            : this(null)
        { }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
