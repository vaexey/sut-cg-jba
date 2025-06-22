using Godot;
using System;

public partial class JodelversAbility : Ability
{
    public override void Cast(Entity owner)
    {
        var proj = ProjectileLibrary.JodelversExplosion.Make();

        var node = owner.Parent2D;

        proj.Shoot(node.Position, owner.PointingAt);
        proj.OwnerEntity = owner;
        proj.AddCollisionExceptionWith(node);

        owner.World.AddProjectile(proj);
    }
}
