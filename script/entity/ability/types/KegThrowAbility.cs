using Godot;
using System;

public partial class KegThrowAbility : Ability
{
    public override void Cast(IEntityContainer entity)
    {
        var proj = ProjectileLibrary.KegThrowProjectile.Make();

        var node = (Node2D)entity;
        
        proj.Shoot(node.Position, node.GetGlobalMousePosition());
        proj.OwnerEntity = entity.Entity;
        proj.AddCollisionExceptionWith(node);

        entity.Entity.World.AddProjectile(proj);
    }
}
