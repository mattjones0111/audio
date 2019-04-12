namespace Domain.Utilities
{
    using System;

    public static class Ensure
    {
        public static void IsNotNullOrEmpty(string value, string paramName)
        {
            if(value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(
                    $"'{paramName}' must not be an empty string.",
                    paramName);
            }
        }

        public static void IsNotNegative(TimeSpan value, string paramName)
        {
            if(value < TimeSpan.Zero)
            {
                throw new ArgumentException(
                    $"'{paramName}' must be a non-negative TimeSpan.",
                    paramName);
            }
        }
    }
}