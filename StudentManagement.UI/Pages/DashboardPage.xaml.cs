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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DashboardPage : UserControl
    {
        public DashboardPage()
        {
            InitializeComponent();
            // Load statistics on initialization
            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadStatistics();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Dashboard load error: {ex.Message}");
            }
        }

        private void LoadStatistics()
        {
            // Use Read* services from core to fetch data
            var readStudents = new StudentManagement.ReadStudents();
            var readClasses = new StudentManagement.ReadClasses();
            var readFaculties = new StudentManagement.ReadFaculties();

            var students = readStudents.GetStudents();
            var classes = readClasses.GetClasses();
            var faculties = readFaculties.GetFaculties();

            // Totals
            txtStudents.Text = students?.Count.ToString() ?? "0";
            txtClasses.Text = classes?.Count.ToString() ?? "0";
            txtFaculties.Text = faculties?.Count.ToString() ?? "0";

            // Prepare counts per faculty
            var facultyCounts = faculties.ToDictionary(f => f.FacultyCode, f => new { Name = f.FacultyName, Count = 0 });

            foreach (var s in students)
            {
                var key = s.MaKhoa ?? "";
                if (facultyCounts.ContainsKey(key))
                {
                    facultyCounts[key] = new { Name = facultyCounts[key].Name, Count = facultyCounts[key].Count + 1 };
                }
                else
                {
                    // Include unknown faculty names
                    facultyCounts[key] = new { Name = s.TenKhoa ?? key, Count = 1 };
                }
            }

            // Render chart
            RenderBarChart(facultyCounts.Values.Select(v => (v.Name, v.Count)).ToList());
        }

        private void RenderBarChart(System.Collections.Generic.List<(string Name, int Count)> data)
        {
            ChartPanel.Children.Clear();

            if (data == null || data.Count == 0) return;

            int max = data.Max(d => d.Count);
            if (max == 0) max = 1; // avoid div zero

            // pastel colors list
            var colors = new[] { "#8DD3C7", "#FFFFB3", "#BEBADA", "#FB8072", "#80B1D3", "#FDB462", "#B3DE69", "#FCCDE5" };

            int colorIndex = 0;
            double maxBarHeight = 180; // pixels

            foreach (var item in data)
            {
                // Each column: a vertical StackPanel with count label, bar, and name label.
                var stack = new StackPanel { Margin = new Thickness(8, 10, 8, 10), VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Center };

                // Bar height calculation
                double ratio = (double)item.Count / max;
                double barHeight = Math.Max(4, ratio * maxBarHeight);

                var rect = new Rectangle
                {
                    Width = 48,
                    Height = barHeight,
                    Fill = (SolidColorBrush)(new BrushConverter().ConvertFromString(colors[colorIndex % colors.Length])),
                    RadiusX = 6,
                    RadiusY = 6,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                rect.ToolTip = $"{item.Name}: {item.Count}";

                // Label for count above bar (fixed height area)
                var lblCount = new TextBlock
                {
                    Text = item.Count.ToString(),
                    FontWeight = FontWeights.SemiBold,
                    Foreground = Brushes.Black,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = 22,
                    TextAlignment = TextAlignment.Center
                };

                // Name label below (fixed height to keep balance), trim if too long
                var lblName = new TextBlock
                {
                    Text = item.Name,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Height = 40,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };

                // Add in order: count, bar, name. Use spacer to keep bar anchored bottom if needed.
                stack.Children.Add(lblCount);
                stack.Children.Add(rect);
                stack.Children.Add(lblName);

                ChartPanel.Children.Add(stack);

                colorIndex++;
            }
        }
    }
}
