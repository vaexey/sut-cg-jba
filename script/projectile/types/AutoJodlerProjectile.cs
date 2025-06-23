using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AutoJodlerProjectile : WallbangProjectile
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Sprite2D Sprite { get; set; }

    [ExportSubgroup("Settings")]
    [Export] public double CarryTime { get; set; } = 1;
    [Export] public double StunTime { get; set; } = 2;

    public List<Entity> HitEntities { get; set; } = new();
    public Dictionary<Entity, double> CarriedEntities { get; set; } = new();

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Velocity.X != 0)
        {
            Sprite.FlipV = Velocity.X < 0;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        ProcessCarriedEntities(delta);
    }

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        foreach (var ent in entities)
        {
            if (HitEntities.Contains(ent))
                continue;

            HitEntities.Add(ent);

            if (ent.CC.GetEffects()
                .Where(c => c.GetType().IsAssignableTo(typeof(MussDasSeinCC)))
                .Any())
            {
                PickupEntity(ent);
            }

            var stun = CrowdControlLibrary.ShortNap.Make();
            ent.CC.AddEffect(stun, StunTime);

            ApplyDamageFromProjectile(ent);
        }
    }

    protected void PickupEntity(Entity ent)
    {
        GD.Print($"Picked up {ent}");

        var mds = ent.CC.GetEffects()
                .Where(c => c.GetType().IsAssignableTo(typeof(MussDasSeinCC)))
                .Select(c => (MussDasSeinCC)c);

        foreach (var cc in mds)
        {
            cc.End(ent);
            cc.Free();
        }

        CarriedEntities[ent] = CarryTime;
    }

    protected void DropEntity(Entity ent)
    {
        GD.Print($"Dropped {ent}");

        CarriedEntities.Remove(ent);
    }

    protected void ProcessCarriedEntities(double delta)
    {
        foreach (var (ent, time) in CarriedEntities.Select(k => (k.Key, k.Value)).ToList())
        {
            CarriedEntities[ent] = Mathf.MoveToward(time, 0, delta);

            if (CarriedEntities[ent] <= 0)
            {
                DropEntity(ent);
                continue;
            }

            GD.Print($"Adjusting pos {ent.Position}");
            ent.Parent2D.Position = ent.Parent2D.Position
                .MoveToward(Position, Velocity.Length() * (float)delta * 1.2f);

            var body = (CharacterBody2D)ent.Parent2D;
            body.Velocity = Vector2.Zero;
        }
    }
}
