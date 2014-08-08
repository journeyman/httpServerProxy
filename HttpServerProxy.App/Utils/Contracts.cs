using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace HttpServerProxy.App.Utils
{
    public static class Contracts
    {
        [Conditional("DEBUG")]
        [ContractAnnotation("condition:false => halt")]
        public static void Assert<TException>(bool condition, [NotNull] string message = "") where TException : Exception
        {
            if (!condition)
            {
                throw (Exception)Activator.CreateInstance(typeof(TException), message);
            }
        }

        [ContractAnnotation("value:null => halt; value:notnull => notnull")]
        public static T ArgNotNull<T>([CanBeNull] this T value, [NotNull] [InvokerParameterName] string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        [ContractAnnotation("value:null => halt;value:notnull => notnull")]
        [NotNull]
        public static T NotNull<T>([CanBeNull] this T value, string message = "") where T : class
        {
            if (value == null)
            {
                throw new Exception("Violated contract, expected not null value. Custom message: " + message);
            }
            return value;
        }

        [ContractAnnotation("value:null => halt;value:notnull => notnull")]
        [NotNull]
        public static T NotNull<T>([CanBeNull] this T? value) where T : struct
        {
            if (value == null)
            {
                throw new Exception("Violated contract, expected not null value");
            }
            return value.Value;
        }

        /// <summary>
        /// Throws an <see cref="T:System.ArgumentOutOfRangeException" /> if a condition does not evaluate to true.
        /// </summary>
        [DebuggerStepThrough]
        [ContractAnnotation("condition:false => halt")]
        public static void Range(bool condition, string parameterName, string message = null)
        {
            if (!condition)
            {
                Contracts.FailRange(parameterName, message);
            }
        }
        /// <summary>
        /// Throws an <see cref="T:System.ArgumentOutOfRangeException" /> if a condition does not evaluate to true.
        /// </summary>
        /// <returns>Nothing.  This method always throws.</returns>
        [DebuggerStepThrough]
        [ContractAnnotation("=> halt")]
        public static Exception FailRange(string parameterName, string message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
            throw new ArgumentOutOfRangeException(parameterName, message);
        }
    }
}
