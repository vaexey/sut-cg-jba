using Godot;
using System;
using System.Linq;

public partial class AutoJodlerAbility : Ability
{

    public override void Use(IEntityContainer entity)
    {
        base.Use(entity);

        var proj = ProjectileLibrary.AutoJodlerProjectile.Make();

        var node = (Node2D)entity;
        var src = node.GlobalPosition;

        var to = node.GetGlobalMousePosition();
        var vw = node.GetViewport().GetVisibleRect().Size;
        var diff = (src - to).Normalized()
            * (Math.Max(vw.X, vw.Y) / 2f + 20f);

        var from = src + diff;

        proj.Shoot(from, to);
        proj.OwnerEntity = entity.Entity;

        entity.Entity.World.AddProjectile(proj);
    }

}
