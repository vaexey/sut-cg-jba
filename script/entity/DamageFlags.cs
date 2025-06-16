using System;

[Flags]
public enum DamageFlags
{
    SourceGeneric = 1 << 1,
    SourcePhysical = 1 << 2,
    SourceInspired = 1 << 3,
}