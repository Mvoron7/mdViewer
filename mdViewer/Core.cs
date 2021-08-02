using System.IO;
using System.Threading.Tasks;
using HeyRed.MarkdownSharp;

namespace mdViewer {
    public class Core {
        IVisual _window;

        private const int MillisecondsDelay = 50;
        private Markdown markdown;
        private string source;
        private string HtmlStyle;
        private FileSystemWatcher watcher;


        public Core(IVisual window) {
            _window = window;
            _window.SourceChanged += Source_TextChanged;
            _window.StyleChanged += Style_TextChanged;

            markdown = new Markdown();

            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.Attributes;
            watcher.Changed += Changed;
        }

        private void Source_TextChanged(string fileName) {
            if (!File.Exists(fileName))
                return;

            FileInfo info = new FileInfo(fileName);
            source = fileName;
            watcher.Path = info.DirectoryName;
            watcher.Filter = info.Name;
            watcher.EnableRaisingEvents = true;

            Refresh();
        }

        public async void Style_TextChanged(string fileName) {
            if (File.Exists(fileName)) {
                using (StreamReader reader = new StreamReader(fileName)) {
                    HtmlStyle = await reader.ReadToEndAsync();
                }

                Refresh();
            }
        }

        private void Changed(object sender, FileSystemEventArgs e) {
            Refresh();
        }

        private async void Refresh() {
            if (!File.Exists(source))
                return;

            await Task.Delay(MillisecondsDelay);

            using (StreamReader reader = new StreamReader(source)) {
                string html = await reader.ReadToEndAsync();
                html = markdown.Transform(html);
                if (!string.IsNullOrEmpty(HtmlStyle))
                    html = $"<style>{HtmlStyle}</style>{html}";
                _window.SetHtml(html);
            }
        }
    }
}
