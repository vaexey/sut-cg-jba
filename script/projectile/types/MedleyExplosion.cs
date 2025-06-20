using Godot;
using System;
using System.Linq;

public partial class MedleyExplosion : SimpleProjectile
{
    [ExportSubgroup("Nodes")]
    [Export] public Node2D SmokeParent1 { get; set; }
    [Export] public Node2D SmokeParent2 { get; set; }

    [ExportSubgroup("Settings")]
    [Export] public double DamageTickPeriod { get; set; } = 0.1;

    public double TimeSinceLastTick = 0;
    public int DamageTicks = 0;

    public override void _Ready()
    {
        base._Ready();

        Scale = new Vector2(0.1f, 0.1f);

        var rand = new RandomNumberGenerator();
        foreach (var child in
            SmokeParent1.GetChildren().Concat(SmokeParent2.GetChildren()))
        {
            if (child.GetType().IsAssignableTo(typeof(AnimatedSprite2D)))
            {
                var sprite = (AnimatedSprite2D)child;
                sprite.Frame = rand.RandiRange(0, 7);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var scale = (float)(1 - TimeLeft / DestroyAfterTime) * 0.9f + 0.1f;

        Scale = new Vector2(scale, scale);

        TimeSinceLastTick += delta;

        while (TimeSinceLastTick > DamageTickPeriod)
        {
            TimeSinceLastTick -= DamageTickPeriod;
            DamageTicks++;
        }
    }

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        base.OnCollision(nodes, entities, players);

        foreach (var ent in entities)
        {
            var slow = CrowdControlLibrary.WaterInPants.Make();

            ent.CC.AddEffect(slow, 0.5);

            if (DamageTicks > 0)
            {
                var dmg = new Damage(OwnerEntity);
                dmg.FlatValue = DamageFlat * DamageTicks;
                dmg.PercentageValue = DamagePercentage * DamageTicks;
                dmg.Flags = DamageFlags;

                ent.ApplyDamage(dmg);

                DamageTicks = 0;

                var stun = CrowdControlLibrary.ShortNap.Make();
                ent.CC.AddEffect(stun, DamageTickPeriod * 0.8);
            }
        }
    }
}
