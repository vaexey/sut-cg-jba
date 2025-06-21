using Godot;
using System;

public partial class BibiHendlAbility : Ability
{
    public override void Cast(Entity owner)
    {
        var hendl = CrowdControlLibrary.BibiHendl.Make();

        owner.CC.AddEffect(hendl, 2);
    }
}
