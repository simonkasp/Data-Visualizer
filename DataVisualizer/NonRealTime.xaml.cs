using System;
using System.Windows;
using System.Windows.Input;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.IO;
using System.Collections.Generic;


namespace DataVisualizer
{
    /// <summary>
    /// Interaction logic for NonRealTime.xaml
    /// </summary>
    public partial class NonRealTime : Window
    {
        private string modelPath = @"C:\Users\skeit\OneDrive\Documents\Models\car.3DS";

        Madgwick mf = new Madgwick();
        Butterworth bw = new Butterworth(0.08f, 90, Butterworth.PassType.Highpass);
        Angles ang = new Angles();
        WriteToFile wf = new WriteToFile();

        private ModelVisual3D object3D = new ModelVisual3D();
        private List<float> rawData;
        private (float[], float[]) rawDataCoff;
        private (float, float, float) angle;
        private float[] q;
        private float ax, ay, az;
        private float anglex = 0, angley = 0, anglez = 0;
        private float anglex2 = 0, angley2 = 0, anglez2 = 0;
        bool fired = false, fired2 = false;
        private bool l = false, r = false, rl = false, rr = false;

        private void BackBtn(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }

        public NonRealTime()
        {
            InitializeComponent();

            object3D.Content = Display3d(modelPath);
            viewPort3d.Children.Add(object3D);


        }
       
        public void Visualize()
        {
            int i = 0;
            string fileName = TextBox1.Text.ToString();
            ReadFromFile rdf = new ReadFromFile(fileName);
            var lineCount = LineCounter(fileName);
            
            rdf.Read();
            rawData = rdf.ReturnData;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.06) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                try
                {
                    AxlLabel.Content = rawData[0 + i] + " " + rawData[1 + i] + " " + rawData[2 + i];
                    GyroLabel.Content = rawData[3 + i] + " " + rawData[4 + i] + " " + rawData[5 + i];
                    mf.MadgwickAHRS(0.08f, 0.01f);

                    rawDataCoff = SensorsCoff(new float[] { rawData[0 + i], rawData[1 + i], rawData[2 + i] },
                                              new float[] { rawData[3 + i], rawData[4 + i], rawData[5 + i] });

                    bw.Update(rawDataCoff.Item1[0]);
                    ax = bw.Value;
                    bw.Update(rawDataCoff.Item1[1]);
                    ay = bw.Value;
                    bw.Update(rawDataCoff.Item1[2]);
                    az = bw.Value;

                    mf.Update(ax, ay, az, rawDataCoff.Item2[0], rawDataCoff.Item2[1], rawDataCoff.Item2[2]);
                    q = mf.ReturnQuat;
                    angle = ang.GetAngles(q[0], q[1], q[2], q[3]);
                    Rotate(angle.Item1, angle.Item2, angle.Item3);


                    if (i >= (rawData.Count * 6))
                    {
                        timer.Stop();
                    }

                    i += 6;
                }
                catch(System.ArgumentOutOfRangeException aooore)
                {

                }
            };

         }

        private int LineCounter(string fileName)
        {
            var lineCount = 0;
            using (var reader = File.OpenText(fileName))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;

        }

        private void VisualizeBtn(object sender, RoutedEventArgs e)
        {
            Visualize();
        }

        private void ChooseFileBtn(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "All files (*.*)|*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                TextBox1.Text = filename;
            }
        }

        private (float[], float[]) SensorsCoff(float[] acc, float[] gyro)
        {
            acc[0] *= 0.01f;
            acc[1] *= 0.01f;
            acc[2] *= 0.01f;

            gyro[0] *= 0.01f;
            gyro[1] *= 0.01f;
            gyro[2] *= 0.01f;

            return (acc, gyro);
        }

        private void CleanBtn(object sender, RoutedEventArgs e)
        {
            Rotate(-anglex, -angley, -anglez);
            AxlLabel.Content = 0 + " " + 0 +" " + 0;
            GyroLabel.Content = 0 + " " + 0 +" " + 0;
            AnglesLabel.Content = "";
        }

        private void Rotate(float alpha, float beta, float gamma)
        {
            var matrix = object3D.Transform.Value;

            anglex2 += alpha;
            angley2 += beta;
            anglez2 += gamma;

            anglex += alpha;
            angley += beta;
            anglez += gamma;

            AnglesLabel.Content = anglex + " " + angley + " " + anglez;
            matrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), -beta));
            matrix.Rotate(new Quaternion(new Vector3D(0, 1, 0) * matrix, alpha));
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1) * matrix, -gamma));

            object3D.Transform = new MatrixTransform3D(matrix);

        }

        private Model3D Display3d(string model)
        {
            Model3D obj = null;
            try
            {
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                ModelImporter import = new ModelImporter();
                obj = import.Load(model);
            }

            catch (Exception e)
            {
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }

            return obj;
        }

    }
}
