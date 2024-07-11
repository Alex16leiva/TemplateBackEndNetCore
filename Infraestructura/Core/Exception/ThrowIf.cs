using Dominio.Core.Extensions;

namespace Infraestructura.Core.Exception
{
     /// <summary>
    /// Exception Helper Class for Parameter Checking.
    /// </summary>
    public static class ThrowIf
    {
        public static class Argument
        {
            /// <summary>
            /// Argument Null Checking
            /// </summary>
            /// <param name="argument">The argument to check.</param>
            /// <param name="argumentName">The argument's name.</param>
            public static void IsNull(object argument, string argumentName) 
            {
                if (argument.IsNull()) throw new ArgumentNullException(argumentName);
            }

            /// <summary>
            /// Argument Null or WhiteSpace Checking
            /// </summary>
            /// <param name="argument">The argument to check.</param>
            /// <param name="argumentName">The argument's name.</param>
            public static void IsNullOrWhiteSpace(string argument, string argumentName)
            {
                if (argument.IsMissingValue()) throw new ArgumentNullException("Argument should not be null or white space", argumentName);
            }

            public static void IsZeroOrNegative(decimal argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentException("Argument should neither zero nor negative", argumentName);
                }
            }
        }
    }
}
