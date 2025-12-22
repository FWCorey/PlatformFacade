using System;

namespace PlatformFacade
{
    /// <summary>
    /// Extension methods for <see cref="Guid"/>.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Converts the Guid to a compact 32-character hex string (no dashes).
        /// Useful for compact debug output.
        /// </summary>
        public static string ToHexString(this Guid guid)
        {
            return guid.ToString("N").ToUpperInvariant();
        }
    }
}
