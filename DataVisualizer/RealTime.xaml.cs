using System;
using System.Windows;
using System.Windows.Input;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace DataVisualizer
{

    public partial class RealTime : Window
    {

        private string modelPath = @"C:\Users\skeit\OneDrive\Documents\Models\car.3DS";

        private ModelVisual3D object3D = new ModelVisual3D();
        int s = 0;
        private (float, float, float, float, float, float) rawData;
        private (float[], float[]) rawDataCoff;
        private float anglex = 0, angley = 0, anglez = 0;
        private float anglex2 = 0, angley2 = 0, anglez2 = 0;
        private (float, float, float) angle;
        private float[] q;
        private float ax, ay, az;
        private float x = 0, y = 0, z = 0;
        private bool l = false, r = false, rl = false, rr = false;

        private void BackBtn(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }

        private bool fired = false, fired2 = false, fired3 = false;

        public float[] rot = new float[] { 0, 0, 0 };

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var matrix = object3D.Transform.Value;

            rot[0] = (float)Vector3D.AngleBetween(new Vector3D(1, 0, 0), matrix.Transform(new Vector3D(1, 0, 0)));
            rot[1] = (float)Vector3D.AngleBetween(new Vector3D(0, 1, 0), matrix.Transform(new Vector3D(0, 1, 0)));
            rot[2] = (float)Vector3D.AngleBetween(new Vector3D(0, 0, 1), matrix.Transform(new Vector3D(0, 0, 1)));

            Rotate(-rot[0], -rot[1], -rot[2]);

        }

        public RealTime()
        {
            InitializeComponent();
            object3D.Content = Display3d(modelPath);
            viewPort3d.Children.Add(object3D);

            var mf = new Madgwick();
            var rd = new ReadData();
            var ang = new Angles();
            var wf = new WriteToFile();
            var bw = new Butterworth(0.08f, 90, Butterworth.PassType.Highpass);

            wf.Clear();
            rd.OpenPort();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.01) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {

                mf.MadgwickAHRS(0.1f, 0.008f);
                rawData = rd.GetData();

                if (rawData.Item1 != 0)
                {
                    rawDataCoff = SensorsCoff(new float[] { rawData.Item1, rawData.Item2, rawData.Item3 },
                                              new float[] { rawData.Item4, rawData.Item5, rawData.Item6 });

                    bw.Update(rawDataCoff.Item1[0]);
                    ax = bw.Value;
                    bw.Update(rawDataCoff.Item1[1]);
                    ay = bw.Value;
                    bw.Update(rawDataCoff.Item1[2]);
                    az = bw.Value;

                    wf.Write(rawData.Item1 + " " + rawData.Item2 + " " + rawData.Item3 + " " + rawData.Item4 + " " + rawData.Item5 + " " + rawData.Item6);

                    mf.Update(ax, ay, az, rawDataCoff.Item2[0], rawDataCoff.Item2[1], rawDataCoff.Item2[2]);
                    q = mf.ReturnQuat;
                    angle = ang.GetAngles(q[0], q[1], q[2], q[3]);

                    Rotate(angle.Item1, angle.Item2, angle.Item3);
                }

            };
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

        private void Rotate(float alpha, float beta, float gamma)
        {
            anglex2 += alpha;
            angley2 += beta;
            anglez2 += gamma;

            anglex += alpha;
            angley += beta;
            anglez += gamma;

            var matrix = object3D.Transform.Value;

            label2.Content = anglex + " " + angley + " " + anglez;

            label4.Content = rawData.Item4 + " " + rawData.Item5 + " " + rawData.Item6;
            label1.Content = rawData.Item1 + " " + rawData.Item2 + " " + rawData.Item3;

            matrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), -beta));
            matrix.Rotate(new Quaternion(new Vector3D(0, 1, 0) * matrix, alpha));
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1) * matrix, -gamma));

            AngleBetweenAxis(matrix);

            object3D.Transform = new MatrixTransform3D(matrix);
            s++;
        }

        private void AngleBetweenAxis(Matrix3D matrix)
        {
            rot[0] = (float)Vector3D.AngleBetween(new Vector3D(1, 0, 0), matrix.Transform(new Vector3D(1, 0, 0)));
            rot[1] = (float)Vector3D.AngleBetween(new Vector3D(0, 1, 0), matrix.Transform(new Vector3D(0, 1, 0)));
            rot[2] = (float)Vector3D.AngleBetween(new Vector3D(0, 0, 1), matrix.Transform(new Vector3D(0, 0, 1)));


            label3.Content = rot[0] + " " + rot[1] + " " + rot[2];
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
