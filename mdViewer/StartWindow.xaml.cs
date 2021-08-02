using System.Windows;


namespace mdViewer {
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void BuildDependensies(object sender, RoutedEventArgs e)
        {
            IVisual visual = new MainWindow();
            Core core = new Core(visual);

            Hide();
            core.Start();

            Close();
        }
    }
}
