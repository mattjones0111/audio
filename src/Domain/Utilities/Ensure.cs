namespace Domain.Utilities
{
    using System;

    public static class Ensure
    {
        public static void IsNotNullOrEmpty(string value, string paramName)
        {
            IsNotNull(value, paramName);

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

        public static void IsNotNull(object o, string paramName)
        {
            if(o == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void IsNonNegativeInteger(long value, string paramName)
        {
            if(value < 0)
            {
                throw new ArgumentException(
                    $"'{paramName}' must be a non-negative integer.",
                    paramName);
            }
        }
    }
}
