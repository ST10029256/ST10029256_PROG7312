using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// Interaction logic for CelebrationWindow2.xaml
    /// </summary>
    public partial class CelebrationWindow2 : Window
    {
        public CelebrationWindow2()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  // Close the window when the "Close" button is clicked
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
