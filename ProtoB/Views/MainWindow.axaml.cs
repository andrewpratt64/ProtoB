using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProtoB.ViewModels;
using System.ComponentModel;

namespace ProtoB.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // Using a key other than f12, since that key would always break the debugger in Visual Studio
            // You may want to change this if your keyboard's f keys only goes up to 12
            this.AttachDevTools(new KeyGesture(Key.F13));
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Closing += OnClosing;
        }

        
        private void OnClosing(object? sender, CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.Dispose();
            }
        }
    }
}
