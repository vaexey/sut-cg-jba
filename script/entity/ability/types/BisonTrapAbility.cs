using Godot;
using System;

public partial class BisonTrapAbility : Ability
{
    public override void Cast(Entity owner)
    {
        var proj = ProjectileLibrary.BisonTrapProjectile.Make();

        proj.Position = owner.Parent2D.Position;
        proj.Velocity =
            proj.ProjectileVelocity
            * (owner.PointingAt - owner.Parent2D.Position).Normalized();

        proj.OwnerEntity = owner;

        owner.World.AddProjectile(proj);
    }
}
