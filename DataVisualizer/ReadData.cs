using System.IO.Ports;
using System.Windows;

namespace DataVisualizer
{
    public class ReadData
    {
        private string data;
        private static SerialPort port;

        public void OpenPort()
        {
            var portName = GetComPortName();

            if (portName != "")
                port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);

            port.Open();

        }

        private static string GetComPortName()
        {
            var ports = new string[] { };
            ports = SerialPort.GetPortNames();
            var portName = "";

            try
            {
                portName = ports[0];
            }
            catch(System.IndexOutOfRangeException)
            {
                MessageBox.Show("Prievadas uždarytas");
            }
            
            return portName;

        }

        public (float, float, float, float, float, float) GetData()
        {

            data = port.ReadExisting();
            var line = new RawData();
            var rawValues = line.GetValues(data);

            return (rawValues.Item1, rawValues.Item2, rawValues.Item3, rawValues.Item4, rawValues.Item5, rawValues.Item6);
            //mahonyAHRS.Update(rawValues.Item1, rawValues.Item2, rawValues.Item3, rawValues.Item4, rawValues.Item5, rawValues.Item6);



        }
    }
}
