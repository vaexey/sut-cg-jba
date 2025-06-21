using Godot;
using System;

public partial class BibiHendlAbility : Ability
{
    public override void Cast(IEntityContainer owner)
    {
        var ent = owner.Entity;

        var hendl = CrowdControlLibrary.BibiHendl.Make();

        ent.CC.AddEffect(hendl, 2);
    }
}
