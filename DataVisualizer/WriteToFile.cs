using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataVisualizer
{
    public class WriteToFile
    {
        private readonly string FileName = @"C:\Users\skeit\source\repos\DataVisualizer\DataVisualizer\Logs\data.txt";

        public void Clear()
        {
            File.WriteAllText(FileName, String.Empty);
        }

        public void Write(string line)
        {
            using (StreamWriter writeText = new StreamWriter(FileName, true))
            {
                writeText.WriteLine(line);
            }
        }
    }
}
