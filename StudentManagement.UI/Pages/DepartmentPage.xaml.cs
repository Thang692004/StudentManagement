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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : UserControl
    {
        public DepartmentPage()
        {
            InitializeComponent();
        }
        private void svForm_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var sv = (ScrollViewer)sender;

            double newOffset = sv.HorizontalOffset - e.Delta;

            // Giới hạn trong phạm vi cuộn
            if (newOffset < 0) newOffset = 0;
            if (newOffset > sv.ScrollableWidth) newOffset = sv.ScrollableWidth;

            sv.ScrollToHorizontalOffset(newOffset);

            // Ngăn cuộn dọc mặc định
            e.Handled = true;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            if (!string.IsNullOrEmpty(query))
            {
                MessageBox.Show($"Tìm kiếm: {query}", "Search");
                // TODO: gọi logic filter dữ liệu ở MainContent
            }
        }
    }
}
