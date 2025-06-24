using Godot;
using System;

public partial class ContemplateAbility : Ability
{
    // Franzl stops to contemplate, slowing himself and gaining flat value of 50% inspiration at a cost of 90% of remaining stamina.

    public override void Cast(Entity owner)
    {
        owner.Inspiration.Value += owner.Inspiration.Max * 0.5;
    }
}