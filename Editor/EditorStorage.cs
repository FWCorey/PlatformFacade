using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of IStorage providing mock storage functionality for Unity Editor
    /// </summary>
    public class EditorStorage : IStorage
    {
        private readonly EditorPlatformSettings _settings;

        /// <summary>
        /// Initializes a new instance of the EditorStorage class
        /// </summary>
        /// <param name="settings">The editor platform settings to use</param>
        public EditorStorage(EditorPlatformSettings settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }

        public Task<Result<bool, string>> DeleteAsync(string key) {
            try {

                if (string.IsNullOrEmpty(key)) { return Task.FromResult(new Result<bool, string>("Key cannot be null or empty!")); }
                // safety checks for invalid characters
                if (key.Contains(':')) { return Task.FromResult(new Result<bool, string>("Key cannot contain colon characters!")); }
                if (key.Contains('%')) { return Task.FromResult(new Result<bool, string>("Key cannot contain the '%' character!")); }
                File.Delete(Path.Combine(Application.persistentDataPath, key));
                return Task.FromResult(new Result<bool, string>(true));

            } catch (Exception e) {

                return Task.FromResult(new Result<bool, string>(e.Message + "\n" + e.StackTrace));
            }
        }

        public Task<Result<bool, string>> ExistsAsync(string key) {
            try {

                if (string.IsNullOrEmpty(key)) { return Task.FromResult(new Result<bool, string>("Key cannot be null or empty!")); }
                // safety checks for invalid characters
                if (key.Contains(':')) { return Task.FromResult(new Result<bool, string>("Key cannot contain colon characters!")); }
                if (key.Contains('%')) { return Task.FromResult(new Result<bool, string>("Key cannot contain the '%' character!")); }
                bool exists = File.Exists(Path.Combine(Application.persistentDataPath, key));
                return Task.FromResult(new Result<bool, string>(exists));

            } catch (Exception e) {

                return Task.FromResult(new Result<bool, string>(e.Message + "\n" + e.StackTrace));
            }
        }

        public Task<Result<IEnumerable<string>, string>> ListKeysAsync(Func<string,bool> filter = null) {

            try {

                var allFiles = Directory.EnumerateFiles(Application.persistentDataPath);
                var keys = new List<string>();
                foreach (var filePath in allFiles) {
                
                    var key = filePath.Substring(Application.persistentDataPath.Length + 1);
                    if(filter == null || filter(key)) {
                        keys.Add(key);
                    }
                }
                return Task.FromResult(new Result<IEnumerable<string>, string>(keys));

            } catch (Exception e) {

                return Task.FromResult(new Result<IEnumerable<string>, string>(e.Message + "\n" + e.StackTrace));
            }
        }

        public Task<Result<byte[], string>> ReadAsync(string key) {
            
            try {
                if (string.IsNullOrEmpty(key)) { return Task.FromResult(new Result<byte[], string>("Key cannot be null or empty!")); }
                // safety checks for invalid characters
                if (key.Contains(':')) { return Task.FromResult(new Result<byte[], string>("Key cannot contain colon characters!")); }
                if (key.Contains('%')) { return Task.FromResult(new Result<byte[], string>("Key cannot contain the '%' character!")); }
                var filePath = Path.Combine(Application.persistentDataPath, key);
                if (!File.Exists(filePath)) {
                    return Task.FromResult(new Result<byte[], string>("Key not found in storage!"));
                }
                var data = File.ReadAllBytes(filePath);
                return Task.FromResult(new Result<byte[], string>(data));
            } catch (Exception e) {
                return Task.FromResult(new Result<byte[], string>(e.Message + "\n" + e.StackTrace));
            }
        }

        public Task<Result<bool, string>> WriteAsync(string key, ReadOnlySpan<byte> data) {
            
            try {
                if (string.IsNullOrEmpty(key)) { return Task.FromResult(new Result<bool, string>("Key cannot be null or empty!")); }
                // safety checks for invalid characters
                if (key.Contains(':')) { return Task.FromResult(new Result<bool, string>("Key cannot contain colon characters!")); }
                if (key.Contains('%')) { return Task.FromResult(new Result<bool, string>("Key cannot contain the '%' character!")); }
                var filePath = Path.Combine(Application.persistentDataPath, key);
                File.WriteAllBytes(filePath, data.ToArray());
                return Task.FromResult(new Result<bool, string>(true));
            } catch (Exception e) {
                return Task.FromResult(new Result<bool, string>(e.Message + "\n" + e.StackTrace));
            }
        }
    }
}