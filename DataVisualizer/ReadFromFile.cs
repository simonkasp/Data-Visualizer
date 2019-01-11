using System;
using System.Collections.Generic;
using System.IO;

namespace DataVisualizer
{
    public class ReadFromFile
    {
        private string fileName;
        private string[] arrayData;

        public List<float> ReturnData { get { return ImuData; } }
        public List<float> ImuData { get; set; }

        public ReadFromFile(string fileName)
        {
            this.fileName = fileName;
            ImuData = new List<float> { 0.5f, 0.5f, 1, 1, 1, 1};
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public void Read()
        {
            StreamReader reader = new StreamReader(FileName);

            string line = reader.ReadLine();
            
            try
            {
                Console.WriteLine("File {0} succesfully opened!", FileName);
                using (reader)
                {
                    while (line != null)
                    {
                        line = reader.ReadLine();

                        try
                        {

                            arrayData = line.Split(' ');

                            ImuData.Add(float.Parse(arrayData[0]));
                            ImuData.Add(float.Parse(arrayData[1]));
                            ImuData.Add(float.Parse(arrayData[2]));
                            ImuData.Add(float.Parse(arrayData[3]));
                            ImuData.Add(float.Parse(arrayData[4]));
                            ImuData.Add(float.Parse(arrayData[5]));

                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("------------\nEnd of file.");
                        }
                        catch (System.ArgumentOutOfRangeException) { }
                    }

                }

            }

            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("File could not be found: {0}.", FileName);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid directory.");
            }
            catch (IOException)
            {
                Console.WriteLine("Can not open the file {0}.", FileName);
            }

        }
    }
}
