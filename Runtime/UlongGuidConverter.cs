using System;
using System.Runtime.InteropServices;

namespace PlatformFacade
{
    /// <summary>
    /// Zero-allocation converter between ulong pairs and Guid using explicit memory layout overlay.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct UlongGuidConverter
    {
        [FieldOffset(0)] public ulong Low;
        [FieldOffset(8)] public ulong High;
        [FieldOffset(0)] public Guid Guid;

        /// <summary>
        /// Creates a Guid from two ulong values (zero allocation)
        /// </summary>
        public static Guid ToGuid(ulong low, ulong high)
        {
            var converter = new UlongGuidConverter { Low = low, High = high };
            return converter.Guid;
        }

        /// <summary>
        /// Creates a Guid from a single ulong value, with high bits set to zero (zero allocation)
        /// </summary>
        public static Guid ToGuid(ulong value)
        {
            var converter = new UlongGuidConverter { Low = value, High = 0 };
            return converter.Guid;
        }

        /// <summary>
        /// Extracts the low and high ulong values from a Guid (zero allocation)
        /// </summary>
        public static void FromGuid(Guid guid, out ulong low, out ulong high)
        {
            var converter = new UlongGuidConverter { Guid = guid };
            low = converter.Low;
            high = converter.High;
        }

        /// <summary>
        /// Extracts the low ulong value from a Guid (zero allocation).
        /// Useful for platforms that only use 64-bit IDs.
        /// </summary>
        public static ulong ToUlong(Guid guid)
        {
            var converter = new UlongGuidConverter { Guid = guid };
            return converter.Low;
        }
    }
}
