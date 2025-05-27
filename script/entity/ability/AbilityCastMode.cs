using System;

[Flags]
public enum AbilityCastMode
{
    // No flags - instant
    Instant = 0,

    // // Requires some cast time before casting
    // CastTime = 1 << 1,

    // Cast time flags

    // Cast time can be cancelled
    CastTimeCancellable = 1 << 2,
    // Cast time can be interrupted by CC
    CastTimeInterruptable = 1 << 3,
    // Casting will be restarted once the CC stops
    CastTimeResetOnInterrupt = 1 << 4,
    

    // Usually entity can be casting one ability at a time.
    // This flag overrides the check for other casting.
    CastableDuringOther = 1 << 4,

    // This flag overrides any stunlike CC effect
    // and makes the spell castable.
    CastableDuringCC = 1 << 5

}