using HeyRed.MarkdownSharp;
using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace WPFUserInterface
{
    public partial class MainWindow : Window
    {
        private Timer timer;
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
            watcher.Changed += Watcher_Changed;

            timer = new Timer(5000);
            timer.Elapsed += new ElapsedEventHandler(Refresh);
            timer.Start();
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e) {
            throw new NotImplementedException();
        }

        private async void Refresh(object sender, ElapsedEventArgs e)
        {
            if (!File.Exists(source))
                return;

            using (StreamReader reader = new StreamReader(source))
            {
                string source = await reader.ReadToEndAsync();
                source = markdown.Transform(source);
                if (!string.IsNullOrEmpty(HtmlStyle))
                    source = $"<style>{HtmlStyle}</style>{source}";
                DoInvoke(() => brow.NavigateToString(source));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }

        private void Source_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (File.Exists(Source.Text)) {
                source = Source.Text;
                
            }
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

        private void StartStopTimer(object sender, RoutedEventArgs e)
        {
            if (timer.Enabled)
                timer.Stop();
            else
                timer.Start();
        }

        private void DoInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            Dispatcher.Invoke(action, priority);
        }
    }
}
