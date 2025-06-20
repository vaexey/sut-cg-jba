using Godot;
using System;

public partial class ShortNapCC : CrowdControlEffect
{
    public ShortNapCC()
    {
        Time = 1;
    }

    public override void Start(Entity effected)
    {
        effected.IsSilenced++;
        effected.IsCrippledHorizontally++;
        effected.IsCrippledVertically++;
    }

    public override void End(Entity effected)
    {
        effected.IsSilenced--;
        effected.IsCrippledHorizontally--;
        effected.IsCrippledVertically--;
    }

    public override bool OnDuplicateEffects(Entity effected, CrowdControlEffect[] duplicates)
    {
        return OnDuplicateSelectLongest(effected, duplicates);
    }

}
