using Godot;
using System;

public partial class JodelversAbility : Ability
{
    public override void Cast(IEntityContainer owner)
    {
        var proj = ProjectileLibrary.JodelversExplosion.Make();

        var node = (Node2D)owner;

        proj.Shoot(node.Position, node.GetGlobalMousePosition());
        proj.OwnerEntity = owner.Entity;
        proj.AddCollisionExceptionWith(node);

        owner.Entity.World.AddProjectile(proj);
    }
}
