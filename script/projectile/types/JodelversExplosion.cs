using Godot;
using System;

public partial class JodelversExplosion : SimpleProjectile
{

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        base.OnCollision(nodes, entities, players);

        if (JustCreated)
        {
            foreach (var ent in entities)
            {
                ApplyDamageFromProjectile(ent);
            }
        }
    }
}
