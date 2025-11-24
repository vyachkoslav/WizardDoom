using UnityEngine;

namespace Utils
{
    public static class VectorUtils
    {
        public static Vector3 WithY(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }
    }
}