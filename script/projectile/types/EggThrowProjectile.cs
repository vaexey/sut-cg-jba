using Godot;
using System;

public partial class EggThrowProjectile : SimpleProjectile
{

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        base.OnCollision(nodes, entities, players);

        // var dmg = new Damage(OwnerEntity);
        // dmg.FlatValue = 20;

        // foreach(var ent in entities)
        // {
        //     ent.ApplyDamage(dmg);
        // }
        foreach (var ent in entities)
        {
            ApplyDamageFromProjectile(ent);
        }
    }

}
