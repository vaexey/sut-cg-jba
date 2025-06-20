using Godot;
using System;

public partial class MedleyAbility : Ability
{
    public override void Cast(IEntityContainer owner)
    {
        var proj = ProjectileLibrary.MedleyExplosion.Make();

        var node = (Node2D)owner;

        proj.Position = node.Position;
        proj.OwnerEntity = owner.Entity;
        proj.AddCollisionExceptionWith(node);

        owner.Entity.World.AddProjectile(proj);
    }
}
