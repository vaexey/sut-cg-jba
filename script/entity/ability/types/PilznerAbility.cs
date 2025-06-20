using Godot;
using System;

public partial class PilznerAbility : Ability
{
    public override void Cast(IEntityContainer owner)
    {
        var ent = owner.Entity;

        var speed = CrowdControlLibrary.HikingInstinct.Make();
        var jumpBoost = CrowdControlLibrary.Appenzeller.Make();

        ent.CC.AddEffect(speed, 3);
        ent.CC.AddEffect(jumpBoost, 3);
    }
}
