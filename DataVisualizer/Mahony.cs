using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer
{
    public class Mahony
    {

        public void MahonyAHRS(float samplePeriod, float kp, float ki)
        {
            SamplePeriod = samplePeriod;
            Kp = kp;
            Ki = ki;
            Quat = new float[] { 1f, 0f, 0f, 0f };
            eInt = new float[] { 0f, 0f, 0f };
        }

        public Mahony()
        {

        }

        public float[] ReturnQuat{ get { return Quat; } }
        public float SamplePeriod { get; set; }
        public float Kp { get; set; }
        public float Ki { get; set; }
        private float[] eInt { get; set; }
        public float[] Quat { get; set; }



        public void Update(float ax, float ay, float az, float gx, float gy, float gz)
        {
            float q1 = Quat[0], q2 = Quat[1], q3 = Quat[2], q4 = Quat[3];   // short name local variable for readability
            float norm;
            float vx, vy, vz;
            float ex, ey, ez;
            float pa, pb, pc;

            // Normalise accelerometer measurement
            //norm = (float)Math.Sqrt(ax * ax + ay * ay + az * az);
            norm = InvSqrt(ax * ax + ay * ay + az * az);
            //if (norm == 0f) return; // handle NaN
            //norm = 1 / norm;        // use reciprocal for division
            ax *= norm;
            ay *= norm;
            az *= norm;

            // Estimated direction of gravity
            vx = 2.0f * (q2 * q4 - q1 * q3);
            vy = 2.0f * (q1 * q2 + q3 * q4);
            vz = q1 * q1 - q2 * q2 - q3 * q3 + q4 * q4;

            // Error is cross product between estimated direction and measured direction of gravity
            ex = (ay * vz - az * vy);
            ey = (az * vx - ax * vz);
            ez = (ax * vy - ay * vx);
            if (Ki > 0f)
            {
                eInt[0] += ex;      // accumulate integral error
                eInt[1] += ey;
                eInt[2] += ez;
            }
            else
            {
                eInt[0] = 0.0f;     // prevent integral wind up
                eInt[1] = 0.0f;
                eInt[2] = 0.0f;
            }

            // Apply feedback terms
            gx = gx + Kp * ex + Ki * eInt[0];
            gy = gy + Kp * ey + Ki * eInt[1];
            gz = gz + Kp * ez + Ki * eInt[2];

            // Integrate rate of change of quaternion
            pa = q2;
            pb = q3;
            pc = q4;
            q1 = q1 + (-q2 * gx - q3 * gy - q4 * gz) * (0.5f * SamplePeriod);
            q2 = pa + (q1 * gx + pb * gz - pc * gy) * (0.5f * SamplePeriod);
            q3 = pb + (q1 * gy - pa * gz + pc * gx) * (0.5f * SamplePeriod);
            q4 = pc + (q1 * gz + pa * gy - pb * gx) * (0.5f * SamplePeriod);

            // Normalise quaternion
            //norm = (float)Math.Sqrt(q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4);
            norm = InvSqrt(q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4);
            //norm = 1.0f / norm;

            Quat[0] = q1 * norm;
            Quat[1] = q2 * norm;
            Quat[2] = q3 * norm;
            Quat[3] = q4 * norm;

            //GetValues();
        }
        unsafe static float InvSqrt(float number)
        {
            long i;
            float x, y;
            const float f = 1.5F;

            x = number * 0.5F;
            y = number;
            i = *(long*)&y;
            i = 0x5f3759df - (i >> 1);
            y = *(float*)&i;
            y = y * (f - (x * y * y));
            y = y * (f - (x * y * y));
            return number * y;
        }
    }
}
