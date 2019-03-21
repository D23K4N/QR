using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.IO;
using Microsoft.Win32;

namespace WpfApp4
{
    
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool WasGenerate = false;
        randomValueRange readyForQR = new randomValueRange();
        plotDistribution distribution;

        public MainWindow()
        {
            InitializeComponent();

        }

        public void ResetSeed()
        {
            readyForQR.fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
        }

        private void RangeStart_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            readyForQR.rangeStartChange(RangeStart_TextBox.Text);
            //RangeEnd_TextBox.Text = '6';
            readyForQR.fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
        }
        private void RangeEnd_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            readyForQR.rangeEndChange(RangeEnd_TextBox.Text);
            readyForQR.fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
        }
        private void RangeEnd_TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            readyForQR.rangeEndChange(RangeEnd_TextBox.Text);
            readyForQR.fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
        }

        private void GenerateQR_button_Click(object sender, RoutedEventArgs e)
        {
            QRImage.Source = readyForQR.paintQR();
            //readyForQR.fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
        }

        private void SaveQR_button_Click(object sender, RoutedEventArgs e)
        {
            readyForQR.makeAndSave();
        }

        private void LoadQR_button_Click(object sender, RoutedEventArgs e)
        {
            QRImage.Source = readyForQR.readAndDecode();
        }

        private void GenerateSeries_button_Click(object sender, RoutedEventArgs e)
        {
            WasGenerate = true;
            readyForQR.fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
            QRList.Items.Clear();
            distribution = new plotDistribution();
            readyForQR.makeSeries();
            for (int i = 0; i < (readyForQR.length); i++) { QRList.Items.Add(readyForQR[i]); distribution.dystrubuanta(readyForQR[i]); }

            distribution.last();
        }

        private void SaveSeries_button_Click(object sender, RoutedEventArgs e)
        {
            readyForQR.makeSeries();
            Microsoft.Win32.SaveFileDialog SaveDiablog = new SaveFileDialog();

            string sPath = "save.txt";
            SaveDiablog.FileName = "serries";
            SaveDiablog.DefaultExt = ".txt";
            Nullable<bool> result = SaveDiablog.ShowDialog();

            if (result == true)
            {
                sPath = SaveDiablog.FileName;

                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(sPath);
                for (int i = 0; i < (readyForQR.length - 1); i++)
                {
                    SaveFile.WriteLine(readyForQR[i]);
                }
                SaveFile.Close();

            }

        }

        private void LoadSeries_button_Click(object sender, RoutedEventArgs e)
        {
            QRList.Items.Clear();
            distribution = new plotDistribution();
            readyForQR = new randomValueRange();
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".jpg";

            Nullable<bool> result = openDialog.ShowDialog();
            if (result == true)
            {
                QRList.Items.Clear();

                List<string> lines = new List<string>();
                using (StreamReader r = new StreamReader(openDialog.OpenFile()))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        QRList.Items.Add(line);
                        distribution.dystrubuanta(int.Parse(line));

                    }
                    distribution.last();
                }
            }
        }

        private void ShowDistribution_button_Click(object sender, RoutedEventArgs e)
        {
            if (!(WasGenerate)) MessageBox.Show("Serries wasn't generated yet!");
            else
            {
                Window1 distrybution_window = new Window1(distribution.Points);
                distrybution_window.Show();
            }
        }

        private void QRList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void lengthOfSeries_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = (string.IsNullOrWhiteSpace(lengthOfSeries.Text));
            if (flag)
            {
                readyForQR.length = 100;
            }
            else
            {
                
                if (int.Parse(lengthOfSeries.Text)>10000)
                {
                    readyForQR.length = 100;
                    MessageBox.Show("");
                }
                else readyForQR.length = int.Parse(lengthOfSeries.Text);
            }
        }

        private void LicenseMIT_Click(object sender, RoutedEventArgs e)
        {
            Window2 MIT = new Window2();
            MIT.Show();
        }
    }



    public interface Ichange
    {
        void rangeStartChange(string value);
        void rangeEndChange(string value);
    }

    public class randomValueRange : Ichange
    {
        private int EndValue = 2;
        private int BeginValue = 1;
        public int QRValueRando;
        public double[] fullSeriesDouble = Core.generator.LCG(DateTime.Now.Millisecond);
        public int numberOfRandomValue = 0;
        public int length = 100;
        public int[] series;
        public int seriesLength;

       

        public int this [int i]
        {
            get
            {
                return this.series[i];
            }
        }
        public void rangeStartChange(string start)
        {
            bool flag = true;
            foreach (char element in start)
            {
                if (!(Char.IsNumber(element)))
                    flag = false;
            }
           // flag = !(string.IsNullOrWhiteSpace(start));
            if (!(string.IsNullOrWhiteSpace(start))&&flag)
            {
                if (BeginValue > EndValue)
                {
                    BeginValue += EndValue;
                    EndValue = BeginValue - EndValue;
                    BeginValue -= EndValue;
                }
                BeginValue= int.Parse(start);
            }
            else
            {
                MessageBox.Show("Its not a value in decimal representation!");
                BeginValue = 2;


            }
        }

        public void rangeEndChange(string end)
        {
            bool flag = true;
            foreach (char element in end)
            {
                if (!(Char.IsNumber(element)))
                    flag = false;
            }
            flag=!(string.IsNullOrWhiteSpace(end));
            if (flag)
            {
                if (BeginValue > EndValue)
                {
                    BeginValue += EndValue;
                    EndValue = BeginValue - EndValue;
                    BeginValue -= EndValue;
                }
                EndValue=int.Parse(end);
            }
            else
            {
                MessageBox.Show("Its not a value in decimal representation!");
                EndValue = 2;
            }
        }

        public BitmapImage paintQR()
        {
            QRCodeWriter qrcode = new QRCodeWriter();
            int temp = (int)((fullSeriesDouble[numberOfRandomValue++]) * (EndValue - BeginValue + 1));
            temp += BeginValue;
            String qrValue = temp.ToString();

            MessageBox.Show(string.Format("QR = {0} ", qrValue));

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 350,
                    Width = 350,
                    Margin = 1
                }
            };

            using (Bitmap bitmap = barcodeWriter.Write(qrValue))
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                stream.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = stream;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                return bi;


            }



        }

        public void makeAndSave()
        {
            Microsoft.Win32.SaveFileDialog SaveDiablog = new SaveFileDialog();
            SaveDiablog.FileName = "QR";
            SaveDiablog.DefaultExt = ".jpg";

            Nullable<bool> result = SaveDiablog.ShowDialog();

            if (result == true)
            {




                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 250,
                        Width = 250,
                        Margin = 1
                    }
                };




                int temp = (int)((fullSeriesDouble[numberOfRandomValue - 1]) * (EndValue - BeginValue + 1));
                temp += BeginValue;
                String qrValue = temp.ToString();
                Bitmap TempSave = barcodeWriter.Write(qrValue);



                string filename = SaveDiablog.FileName;

                TempSave.Save(SaveDiablog.FileName);


            }
        }

        public BitmapImage readAndDecode()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".jpg";

            Nullable<bool> result = openDialog.ShowDialog();
            if (result == true)
            {
                Bitmap qrs = new Bitmap(openDialog.FileName);

                BarcodeReader reader = new BarcodeReader { AutoRotate = true, TryInverted = true };
                Result res = reader.Decode(qrs);
                string decoded = res.ToString().Trim();


                MessageBox.Show(string.Format("QR = {0} ", decoded));
                return new BitmapImage(new Uri(openDialog.FileName));
            }
            else
            {
                MessageBox.Show("Some trouble with decode(it's not a QR code)!");
                return readAndDecode();
            }
        }

        public void makeSeries()
        {
            series = new int[1];


              for (int i =0; i<=length; i++)
              {
                int temp = (int)(((fullSeriesDouble[i]) * (EndValue - BeginValue+1)));
                temp += BeginValue;

                if (i >= series.Length) Array.Resize(ref series,(i+1));

                series[i] = temp;
                seriesLength++;
              }
        }



    }


    public class plotDistribution : randomValueRange
    {
        public double[][] Points;
        public int index = 0;
        bool itWas = false;

        public void dystrubuanta(int value)
        {
            itWas = false;
            if (index ==0)
            {
                Points = new double[1][];
                Points[0] = new double[3]; //[value][P/100][D]
                Points[0][0] = value;
                Points[0][1] = 1;
                index++;
            }
            else
            {
                itWas = false;
                for (int i = 0; i < index; i++)
                {
                    if (Points[i][0] == value)
                    {
                        ++Points[i][1];
                        itWas = true;
                    }
                }
                if (!itWas)
                {
                    Array.Resize(ref Points, index + 1);
                    Points[index] = new double[3];
                    Points[index][0] = value;
                    Points[index][1] = 1;
                    itWas = false;
                    ++index;

                }
            } //probability

            
        }

        public void last()
        {
            for (int i = 0; i < index; i++)
            {
                for (int j = 0; j < index; j++)
                {
                    if (Points[i][0] >= Points[j][0])
                    {
                        Points[i][2] += Points[j][1] / 100;
                    }

                }
            }
            bool isNotEnd = true;
            int iteration = 0;
            while(isNotEnd)
            {
                iteration = 0;
                for (int i=0; i<Points.Length-1;i++)
                {
                    if(Points[i][0]>Points[i+1][0])
                    {
                        double temp1 = Points[i][0];
                        double temp2 = Points[i][1];
                        double temp3 = Points[i][2];

                        Points[i][0] = Points[i + 1][0];
                        Points[i][1] = Points[i + 1][1];
                        Points[i][2] = Points[i + 1][2];

                        Points[i + 1][0] = temp1;
                        Points[i + 1][1] = temp2;
                        Points[i + 1][2] = temp3;
                        iteration=1;
                    }
                    
                }
                if (iteration == 0) isNotEnd = false;
            }

        }


    }


}
