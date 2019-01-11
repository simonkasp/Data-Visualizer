using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer
{
    public class Butterworth
    {
        private readonly float q = (float)Math.Sqrt(2);
        private readonly float frequency;
        private readonly int samplePeriod;
        private readonly PassType passType;

        private readonly float c, a1, a2, a3, b1, b2, D;

        private float[] inputHistory = new float[2];

        private float[] outputHistory = new float[3];

        public Butterworth(float frequency, int samplePeriod, PassType passType)
        {
           
            this.frequency = frequency;
            this.samplePeriod = samplePeriod;
            this.passType = passType;

            switch (passType)
            {
                case PassType.Lowpass:
                    c = 1.0f / (float)Math.Tan(Math.PI * frequency / samplePeriod);
                    a1 = 1.0f / (1.0f + q * c + c * c);
                    a2 = 2f * a1;
                    a3 = a1;
                    b1 = 2.0f * (1.0f - c * c) * a1;
                    b2 = (1.0f - q * c + c * c) * a1;
                    break;
                case PassType.Highpass:
                    c = (float)Math.Tan(Math.PI * frequency / samplePeriod);
                    D = (1.0f + q * c + c * c);
                    a2 = -2f / D;
                    a3 = D;
                    b1 = 2.0f * (c * c - 1.0f) / D;
                    b2 = (1.0f - q * c + c * c) / D;
                    break;

            }
        }

        public enum PassType
        {
            Highpass,
            Lowpass,
        }

        public void Update(float newInput)
        {
            float newOutput = D * newInput + a2 * this.inputHistory[0] + a3 * this.inputHistory[1] - b1 * this.outputHistory[0] - b2 * this.outputHistory[1];

            this.inputHistory[1] = this.inputHistory[0];
            this.inputHistory[0] = newInput;

            this.outputHistory[2] = this.outputHistory[1];
            this.outputHistory[1] = this.outputHistory[0];
            this.outputHistory[0] = newOutput;
        }

        public float Value
        {
            get { return this.outputHistory[0]; }
        }
    }
}
