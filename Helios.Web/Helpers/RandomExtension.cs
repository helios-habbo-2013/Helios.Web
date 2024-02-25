﻿namespace Helios.Web.Helpers
{
    public static class RandomExtension
    {
        public static bool NextBoolean(this Random random)
        {
            return random.Next() > int.MaxValue / 2;
            // Next() returns an int in the range [0..Int32.MaxValue]
        }
    }
}
