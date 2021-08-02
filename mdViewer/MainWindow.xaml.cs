using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HeyRed.MarkdownSharp;

namespace WPFUserInterface
{
    public partial class MainWindow : Window
    {
        private const int MillisecondsDelay = 50;
        private Markdown markdown;
        private string source;
        private string HtmlStyle;
        private FileSystemWatcher watcher;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            markdown = new Markdown();

            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.Attributes;
            watcher.Changed += Changed;
        }

        private void Changed(object sender, FileSystemEventArgs e)
        {
            Refresh();
        }

        private async void Refresh()
        {
            if (!File.Exists(source))
                return;

            await Task.Delay(MillisecondsDelay);

            using (StreamReader reader = new StreamReader(source))
            {
                string html = await reader.ReadToEndAsync();
                html = markdown.Transform(html);
                if (!string.IsNullOrEmpty(HtmlStyle))
                    html = $"<style>{HtmlStyle}</style>{html}";
                DoInvoke(() => brow.NavigateToString(html));
            }
        }

        private void Source_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!File.Exists(Source.Text))
                return;

            FileInfo info = new FileInfo(Source.Text);
            source = Source.Text;
            watcher.Path = info.DirectoryName;
            watcher.Filter = info.Name;
            watcher.EnableRaisingEvents = true;

            Refresh();
        }

        private async void Style_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (File.Exists(FileOfStyle.Text))
            {
                using (StreamReader reader = new StreamReader(FileOfStyle.Text))
                {
                    HtmlStyle = await reader.ReadToEndAsync();
                }
            }
        }

        private void DoInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            Dispatcher.Invoke(action, priority);
        }
    }
}
