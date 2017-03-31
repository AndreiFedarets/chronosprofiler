﻿namespace Chronos
{
    /// <summary>
    /// Represents platform of windows process
    /// </summary>
    public enum ProcessPlatform : int
    {
        Unknown = -1,
        Native = 0,
        I386 = 0x014c,
        Itanium = 0x0200,
        X64 = 0x8664,
        AnyCPU = int.MaxValue
    }
}
