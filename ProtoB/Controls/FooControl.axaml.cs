using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Avalonia.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProtoB.Controls
{
    public class FooItem
    {
        private static int _num = 0;
        private string _name = "";

        public string MyName
        {
            get => _name;
            set => _name = value;
        }

        public FooItem()
        {
            MyName = "NUMBAH_" + _num++;
        }
    }

    public partial class FooControl : UserControl
    {
        private IEnumerable<FooItem> _bar = new ObservableCollection<FooItem>();

        public IEnumerable<FooItem> EnumerableBar
        {
            get => _bar;
            set => SetAndRaise<IEnumerable<FooItem>>(EnumerableBarProperty, ref _bar, value);
        }


        public static readonly DirectProperty<FooControl, IEnumerable<FooItem>> EnumerableBarProperty =
            AvaloniaProperty.RegisterDirect<FooControl, IEnumerable<FooItem>>(
                nameof(EnumerableBar),
                o => o.EnumerableBar,
                (o, v) => o.EnumerableBar = v
            );


        public FooControl()
        {
            //DataContext = this;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
