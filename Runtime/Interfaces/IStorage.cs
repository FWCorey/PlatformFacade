using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformFacade
{
    /// <summary>
    /// Defines a contract for a storage system that supports asynchronous operations for writing, reading, deleting, 
    /// and querying data by key.
    /// </summary>
    /// <remarks>This interface provides methods for interacting with a key-value-based storage system. Each
    /// operation is  asynchronous and returns a <see cref="Result{TSuccess, TFailure}"/> object to indicate success or
    /// failure along with additional information in case of failure. Keys are expected to be unique identifiers, and 
    /// operations may fail if preconditions, such as non-null keys, are not met.</remarks>
    public interface IStorage
    {
        /// <summary>
        /// Asynchronously writes the specified data to the underlying storage with the given key.
        /// </summary>
        /// <remarks>The method ensures that the data is written to the storage associated with the
        /// specified key. If the operation fails, the returned result will contain an error message describing the
        /// failure.</remarks>
        /// <param name="key">The unique identifier associated with the data to be written. Cannot be null or empty.</param>
        /// <param name="data">The data to be written, represented as a read-only span of bytes.</param>
        /// <returns>A task that represents the asynchronous write operation. The result is a <see cref="Result{TSuccess,
        /// TFailure}"/> containing a boolean indicating success or failure, and a string providing additional
        /// information in case of failure.</returns>
        Task<Result<bool, string>> WriteAsync(string key, ReadOnlySpan<byte> data);

        /// <summary>
        /// Asynchronously reads data associated with the specified key.
        /// </summary>
        /// <param name="key">The key used to identify the data to be read. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TSuccess,
        /// TFailure}"/> where the success value is a byte array representing the data, and the failure value is a
        /// string describing the error.</returns>
        Task<Result<byte[],string>> ReadAsync(string key);

        /// <summary>
        /// Deletes the item associated with the specified key asynchronously.
        /// </summary>
        /// <remarks>The method performs the deletion operation asynchronously. If the specified key does
        /// not exist, the operation will return a failure result with an appropriate error message.</remarks>
        /// <param name="key">The unique identifier of the item to delete. Cannot be null or empty.</param>
        /// <returns>A <see cref="Result{T, TError}"/> object containing a boolean value indicating whether the deletion was
        /// successful, and a string providing additional information in case of failure.</returns>
        Task<Result<bool, string>> DeleteAsync(string key);

        /// <summary>
        /// Determines whether an item with the specified key exists in the data store.
        /// </summary>
        /// <param name="key">The unique identifier of the item to check for existence. Cannot be null or empty.</param>
        /// <returns>A <see cref="Result{T, TError}"/> containing a boolean value indicating whether the item exists (<see
        /// langword="true"/> if the item exists; otherwise, <see langword="false"/>), and an optional error message if
        /// the operation fails.</returns>
        Task<Result<bool, string>> ExistsAsync(string key);

        /// <summary>
        /// Asynchronously retrieves a collection of keys that match the specified prefix.
        /// </summary>
        /// <remarks>The method performs an asynchronous operation to retrieve keys from the underlying
        /// data store. If no keys match the specified prefix, the success result will contain an empty
        /// collection.</remarks>
        /// <param name="keyPrefix">The prefix used to filter the keys. Only keys that start with this prefix will be included in the result.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TSuccess,
        /// TFailure}"/> object, where the success value is an <see cref="IEnumerable{T}"/> of strings representing the
        /// matching keys, and the failure value is a string describing the error if the operation fails.</returns>
        Task<Result<IEnumerable<string>,string>> ListKeysAsync(Func<string,bool> filter = null);
    }
}