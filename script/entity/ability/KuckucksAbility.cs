using Godot;
using System;
using System.Linq;

public partial class KuckucksAbility : Ability
{
    public override void Cast(Entity owner)
    {
        foreach (var ability in owner.Abilities.All
            .Where(a => a != this))
            ability.ResetCooldown();

        
    }
}
