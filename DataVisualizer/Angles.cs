using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer
{
    public class Angles
    {
        private float alpha;
        private float beta;
        private float gamma;

        public (float, float, float) GetAngles(float qx, float qy, float qz, float qw)
        {
            float R11 = 2 * qx * qx - 1 + 2 * qy * qy;
            float R21 = 2 * (qy * qz - qx * qw);
            float R31 = 2 * (qy * qw + qx * qz);
            float R32 = 2 * (qz * qw - qx * qy);
            float R33 = 2 * qx * qx - 1 + 2 * qw * qw;

            alpha = (float)Math.Atan2(R32, R33);
            beta = -(float)Math.Atan(R31 / Math.Sqrt(1 - R31 * R31));
            gamma = (float)Math.Atan2(R21, R11);

            return (alpha * 180f / (float)Math.PI, (beta * 180f / (float)Math.PI), gamma * 180f / (float)Math.PI);
        }
    }
}
