using System;

namespace EntityFrameworkCore
{
    static class RandomExtension
    {
        public static string NextChar(this Random rand, int min = 65, int max = 91)
        {
            return char.ConvertFromUtf32(rand.Next(min, max));
        }

        public static bool NextBool(this Random rand)
        {
            return rand.NextDouble() > 0.5;
        }
    }
}
