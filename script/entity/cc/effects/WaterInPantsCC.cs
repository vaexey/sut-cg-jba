using Godot;
using System;
using System.Linq;

public partial class WaterInPantsCC : CrowdControlEffect
{
    public WaterInPantsCC()
    {
        Time = 10;
    }

    public override void Start(Entity effected)
    {
        GD.Print(effected);
        effected.PassiveAttributes.SpeedModifierMultiplicative -= 0.5f;
    }

    public override void End(Entity effected)
    {
        effected.PassiveAttributes.SpeedModifierMultiplicative += 0.5f;
    }

    public override bool OnDuplicateEffects(Entity effected, CrowdControlEffect[] duplicates)
    {
        return OnDuplicateSelectLongest(effected, duplicates);
        // var ccs = duplicates.ToList();
        // ccs.Add(this);

        // Time = ccs.OrderByDescending(cc => cc.Time).First().Time;

        // foreach (var cc in duplicates)
        // {
        //     cc.End(effected);
        //     cc.QueueFree();
        // }

        // return true;
    }
}
