using Godot;
using System;

public partial class MedleyAbility : Ability
{
    public override void Cast(Entity owner)
    {
        var proj = ProjectileLibrary.MedleyExplosion.Make();

        var node = owner.Parent2D;

        proj.Position = node.Position;
        proj.OwnerEntity = owner;
        proj.AddCollisionExceptionWith(node);

        owner.World.AddProjectile(proj);
    }
}
