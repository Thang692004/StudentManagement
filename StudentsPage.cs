using System.Windows.Controls;
using StudentManagement.Core;
using System.Collections.Generic;
using StudentManagement.Core;

namespace StudentManagement.UI.Pages
{
    public partial class StudentsPage : UserControl
    {
        private ReadStudents read;

        public StudentsPage()
        {
            InitializeComponent();

            read = new ReadStudents();
            LoadStudents();
        }

        private void LoadStudents()
        {
            var students = read.GetStudents();
            StudentsDataGrid.ItemsSource = students;
        }
    }
}
