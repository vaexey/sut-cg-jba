using Godot;
using System;

public partial class EggThrowAbility : Ability
{
    public override void Cast(Entity entity)
    {
        var proj = ProjectileLibrary.EggThrowProjectile.Make();

        var node = entity.Parent2D;
        
        proj.Shoot(node.Position, node.GetGlobalMousePosition());
        proj.OwnerEntity = entity;
        proj.AddCollisionExceptionWith(node);

        entity.World.AddProjectile(proj);
    }
}
