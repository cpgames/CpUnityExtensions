using UnityEngine;

namespace cpGames.core
{
    public static class Vector3Extensions
    {
        #region Methods
        public static Vector3 FromFloat(float[] f)
        {
            if (f == null || f.Length < 3)
            {
                return Vector3.zero;
            }
            return new Vector3(f[0], f[1], f[2]);
        }

        public static void ToFloat(this Vector3 v, ref float[] result)
        {
            if (result == null)
            {
                result = new float[3];
            }

            result[0] = v.x;
            result[1] = v.y;
            result[2] = v.z;
        }

        public static float[] ToFloat(this Vector3 v)
        {
            var result = new float[3];

            result[0] = v.x;
            result[1] = v.y;
            result[2] = v.z;

            return result;
        }
        #endregion
    }
}