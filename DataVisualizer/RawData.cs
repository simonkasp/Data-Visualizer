using System.Text.RegularExpressions;

namespace DataVisualizer
{
    public class RawData
    {
        private float axlX;
        private float axlY;
        private float axlZ;
        private float gyroX;
        private float gyroY;
        private float gyroZ;

        public (float, float, float, float, float, float) GetValues(string line)
        {
            string pattern = @"(AXL:X:)([-]\d+|\d+).(\d+)(;Y:)([-]\d+|\d+).(\d+)(;Z:)([-]\d+|\d+).(\d+)(;GYRO:X:)([-]\d+|\d+).(\d+)(;Y:)([-]\d+|\d+).(\d+)(;Z:)([-]\d+|\d+).(\d+)";
            MatchCollection matches = Regex.Matches(line, pattern);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string[] patternSplit = match.Value.Split(';');
                    //AXL
                    string patternAxlX = @"([-]\d+|\d+).(\d+)";
                    Match matchAxlX = Regex.Match(patternSplit[0], patternAxlX);
                    if (matchAxlX.Success)
                    {
                        axlX = float.Parse(matchAxlX.Value);
                    }

                    string patternAxlY = @"([-]\d+|\d+).(\d+)";
                    Match matchAxlY = Regex.Match(patternSplit[1], patternAxlY);
                    if (matchAxlY.Success)
                    {
                        axlY = float.Parse(matchAxlY.Value);
                    }

                    string patternAxlZ = @"([-]\d+|\d+).(\d+)";
                    Match matchAxlZ = Regex.Match(patternSplit[2], patternAxlZ);
                    if (matchAxlZ.Success)
                    {
                        axlZ = float.Parse(matchAxlZ.Value);
                    }
                    //GYRO
                    string patternGyroX = @"([-]\d+|\d+).(\d+)";
                    Match matchGyroX = Regex.Match(patternSplit[3], patternGyroX);
                    if (matchGyroX.Success)
                    {
                        gyroX = float.Parse(matchGyroX.Value);
                    }

                    string patternGyroY = @"([-]\d+|\d+).(\d+)";
                    Match matchGyroY = Regex.Match(patternSplit[4], patternGyroY);
                    if (matchGyroY.Success)
                    {
                        gyroY = float.Parse(matchGyroY.Value);
                    }

                    string patternGyroZ = @"([-]\d+|\d+).(\d+)";
                    Match matchGyroZ = Regex.Match(patternSplit[5], patternGyroZ);

                    if (matchGyroZ.Success)
                    {
                        gyroZ = float.Parse(matchGyroZ.Value);
                    }

                    return (axlX, axlY, axlZ, gyroX, gyroY, gyroZ);

                }
                else
                {
                    continue;
                }

            }
            return (0, 0, 0, 0, 0, 0);
        }
    }
}
