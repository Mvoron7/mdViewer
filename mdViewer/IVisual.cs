using System;

namespace mdViewer {
    public interface IVisual {
        event Action<string> SourceChanged;
        event Action<string> StyleChanged;

        void SetHtml(string html);
        void Show();
    }
}
