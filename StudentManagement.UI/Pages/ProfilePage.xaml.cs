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
using System.Xml.Linq;

namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : UserControl
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetReadOnly(false);
            BtnEdit.Visibility = Visibility.Collapsed;
            BtnSave.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SetReadOnly(true);
            BtnSave.Visibility = Visibility.Collapsed;
            BtnEdit.Visibility = Visibility.Visible;
            MessageBox.Show("Thông tin đã được lưu thành công!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SetReadOnly(bool state)
        {
            txtName.IsReadOnly = state;
            txtEmail.IsReadOnly = state;
            txtPassword.IsReadOnly = state;
            txtLocation.IsReadOnly = state;
            txtGender.IsReadOnly = state;
            txtDOB.IsReadOnly = state;
            txtPhone.IsReadOnly = state;
            txtPermission.IsReadOnly = state;
        }
    }
}
