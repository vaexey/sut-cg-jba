using Godot;
using System;
using System.Collections.Generic;

public partial class AutoJodlerProjectile : WallbangProjectile
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Sprite2D Sprite { get; set; }

    public List<Entity> HitEntities { get; set; } = new();

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Velocity.X != 0)
        {
            Sprite.FlipV = Velocity.X < 0;
        }
    }

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        foreach (var ent in entities)
        {
            if (HitEntities.Contains(ent))
                continue;

            HitEntities.Add(ent);

            var stun = CrowdControlLibrary.ShortNap.Make();
            ent.CC.AddEffect(stun, 2);

            ApplyDamageFromProjectile(ent);
        }
    }
}
