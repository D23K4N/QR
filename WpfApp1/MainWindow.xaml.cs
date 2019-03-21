using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PlotModel = new OxyPlotModel();
            this.DataContext = PlotModel;
        }

        OxyPlotModel PlotModel;

        public class OxyPlotModel : INotifyPropertyChanged
        {
            private OxyPlotModel oxyPlotModel;

            private OxyPlot.PlotModel plotModel;
            public OxyPlot.PlotModel PlotModel
            {
                get
                {
                    return plotModel;
                }
                set
                {
                    plotModel = value; OnPropertyChanged("PlotModel");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
            private void SetUpLegend()
            {
                plotModel.LegendTitle = "Legenda";
                plotModel.LegendOrientation = OxyPlot.LegendOrientation.Horizontal;
                plotModel.LegendPlacement = OxyPlot.LegendPlacement.Outside;
                plotModel.LegendPosition = OxyPlot.LegendPosition.TopRight; 
                plotModel.LegendBackground = OxyPlot.OxyColor.FromAColor(200, OxyPlot.OxyColors.White);
                plotModel.LegendBorder = OxyPlot.OxyColors.Black; 
            }

            public IList<OxyPlot.DataPoint> Points { get; private set; }
            public void PodajDaneDoWykresu()
            {
                this.plotModel.Title = "Przykład 2";
                this.Points = new List<OxyPlot.DataPoint>
    {
            new OxyPlot.DataPoint(5, 3),
            new OxyPlot.DataPoint(15, 17),
            new OxyPlot.DataPoint(25, 12),
            new OxyPlot.DataPoint(35, 4),
            new OxyPlot.DataPoint(45, 15),
            new OxyPlot.DataPoint(55, 10)
    };
            }

        }

    }
}
