using Godot;
using System;

public partial class KegThrowProjectile : SimpleProjectile
{
    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        base.OnCollision(nodes, entities, players);

        var proj = ProjectileLibrary.KegThrowExplosion.Make();

        proj.Position = Position;
        OwnerEntity.World.AddProjectile(proj);
    }
}
