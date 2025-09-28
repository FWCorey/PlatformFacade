using System;
using System.Threading.Tasks;

namespace PlatformFacade
{
    /// <summary>
    /// Represents the result of an operation that can either succeed or fail.
    /// Implements Railway Oriented Programming pattern.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value</typeparam>
    /// <typeparam name="TError">The type of the error value</typeparam>
    public readonly struct Result<TSuccess, TError>
    {
        public readonly bool IsSuccess;
        private readonly TSuccess _value;
        private readonly TError _error;

        // --- Constructors ---
        public Result(TSuccess value)
        {
            IsSuccess = true;
            _value = value;
            _error = default;
        }

        public Result(TError error)
        {
            IsSuccess = false;
            _value = default;
            _error = error;
        }

        // --- Accessors ---
        public TSuccess Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Result was a failure.");

        public TError Error => !IsSuccess
            ? _error
            : throw new InvalidOperationException("Result was a success.");

        /// <summary>
        /// Chains an operation that returns a Task&lt;Result&gt; if the previous task was a success.
        /// </summary>
        /// <param name="task">The previous async operation in the chain.</param>
        /// <param name="func">The function to execute if the previous operation was a success.</param>
        /// <returns>The Result of the new operation, or the Failure from the previous one.</returns>
        public static async Task<Result<TNewSuccess, TError>> Then<TNewSuccess>(
            this Task<Result<TSuccess, TError>> task,
            Func<TSuccess, Task<Result<TNewSuccess, TError>>> func)
        {
            // Await the result of the previous operation
            var initialResult = await task;

            if (initialResult.IsSuccess)
            {
                // If it succeeded, execute the next function in the chain
                return await func(initialResult.Value);
            }
            else
            {
                // If it failed, short-circuit and propagate the error
                return new Result<TNewSuccess, TError>(initialResult.Error);
            }
        }
    }
}