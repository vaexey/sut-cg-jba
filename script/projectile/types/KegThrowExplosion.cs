using Godot;
using System;

public partial class KegThrowExplosion : SimpleProjectile
{

    public override void _Ready()
    {
        base._Ready();

        var rng = new RandomNumberGenerator();

        DamageFlat *= rng.RandfRange(
            0.9f, 1.1f
        );
    }

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        base.OnCollision(nodes, entities, players);

        if (JustCreated)
        {
            foreach (var ent in entities)
            {
                ApplyDamageFromProjectile(ent);

                var slow = CrowdControlLibrary.WaterInPants.Make();
                ent.CC.AddEffect(slow, 2);
            }
        }
    }
}
