using System.Windows;

namespace WpfApp4
{
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Wpf;

    public class MainViewModel
    {

        public MainViewModel(double[][] a)
        {
            this.Title = "DISTRIBUTUIN";
            this.Points = new List<DataPoint>
            {
                //new DataPoint(0,0)
            };
            for (int i = 0; i < a.Length; i++)
            {
                this.Points.Add(new DataPoint(a[i][0], a[i][2]));
            }

        }
        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; }
    }
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public double[][] b;
        public OxyPlot.PlotModel plocik = new PlotModel();
        OxyPlot.Series.LineSeries punkty = new OxyPlot.Series.LineSeries();

        public Window1(double[][] a)
        {
            this.DataContext = new MainViewModel(a);
            b = a;
            InitializeComponent();
            this.MaxHeight = this.MaxHeight;
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < b.Length; i++)
            {
               punkty.Points.Add(new DataPoint(b[i][0], b[i][2]));
            }
            plocik.Series.Add(punkty);
            //OxyPlot.Wpf.PngExporter.Export(plocik, "plot.jpg", 1600, 1600, OxyPlot.OxyColors.White, 96);
            PngExporter.Export(plocik, @"D:\file.png", 1600, 1600, OxyColors.White);
        }
    }
}
