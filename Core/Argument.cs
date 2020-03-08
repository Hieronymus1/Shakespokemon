using System;

namespace Shakespokemon.Core
{
    public static class Argument
    {
        public static void IsNotNullOrWhitespace(string value, string argumentName)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Can't be null or empty.", nameof(argumentName));
            }
        }

        public static void IsNotNull(object value, string argumentName)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }
        }

        public static void IsValid(bool condition, string message)
        {
            if(!condition)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
