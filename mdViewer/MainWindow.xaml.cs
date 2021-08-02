using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace mdViewer {
    public partial class MainWindow : Window, IVisual
    {
        public event Action<string> SourceChanged;
        public event Action<string> StyleChanged;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new Core(this);
        }

        public void SetHtml(string html) {
            DoInvoke(() => brow.NavigateToString(html));
        }

        private void Source_TextChanged(object sender, TextChangedEventArgs e)
        {
            SourceChanged?.Invoke(Source.Text);
        }

        private void Style_TextChanged(object sender, TextChangedEventArgs e)
        {
            StyleChanged?.Invoke(FileOfStyle.Text);
        }

        private void DoInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            Dispatcher.Invoke(action, priority);
        }
    }
}
