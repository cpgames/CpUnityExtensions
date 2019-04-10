using UnityEngine;

namespace cpGames.core
{
    public static class QuaternionExtensions
    {
        #region Methods
        public static Quaternion FromFloat(float[] f)
        {
            if (f == null || f.Length < 4)
            {
                return Quaternion.identity;
            }
            return new Quaternion(f[0], f[1], f[2], f[3]);
        }

        public static void ToFloat(this Quaternion q, ref float[] result)
        {
            if (result == null)
            {
                result = new float[4];
            }

            result[0] = q.x;
            result[1] = q.y;
            result[2] = q.z;
            result[3] = q.w;
        }

        public static float[] ToFloat(this Quaternion q)
        {
            var result = new float[4];

            result[0] = q.x;
            result[1] = q.y;
            result[2] = q.z;
            result[3] = q.w;

            return result;
        }
        #endregion
    }
}