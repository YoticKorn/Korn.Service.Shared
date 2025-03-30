using System;

static class RandomExtensions
{
    public static ulong NextUInt64(this Random self) => (ulong)(((long)self.Next() << 32) | (long)self.Next());
}