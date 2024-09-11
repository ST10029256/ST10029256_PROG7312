using System.Windows;

namespace ST10029256_PROG7312
{
    public partial class CelebrationWindow : Window
    {
        public CelebrationWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  // Close the window when the "Close" button is clicked
        }
    }
}
