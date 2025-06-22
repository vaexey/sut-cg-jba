using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SimpleProjectile : CharacterBody2D
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Entity OwnerEntity { get; set; }
    [Export]
    public CanvasItem FadedObject { get; set; }

    [ExportSubgroup("Projectile settings")]
    [Export]
    public string DisplayName { get; set; } = "Unnamed projectile";
    [Export]
    public float ProjectileVelocity { get; set; } = 200;

    [Export]
    public float ProjectileGravity { get; set; } = 500;

    [Export]
    public float RotationSpeed { get; set; } = 0;

    [Export]
    public bool Stationary { get; set; } = false;

    [Export]
    public bool DestroyOnCollision { get; set; } = true;

    [Export]
    public double DestroyOnCollisionDelay { get; set; } = 0;

    [Export]
    public double FadeTime { get; set; } = 0;

    [Export]
    public double DestroyAfterTime { get; set; } = 15.0;

    [ExportSubgroup("Damage settings")]
    [Export]
    public double DamageFlat { get; set; } = 0;
    [Export]
    public double DamagePercentage { get; set; } = 0;
    [Export]
    public DamageFlags DamageFlags { get; set; } = 0;

    public bool JustCreated = true;
    [Export]public double TimeLeft = 0;

    // [ExportSubgroup("Multiplayer sync")]
    // [Export] private double MPS_TimeLeft { get => TimeLeft; set => TimeLeft = value; }

    // [Export]
    // public bool IgnoreOwnerCollision { get; set; } = true;

    // public override void _Ready()
    // {
    //     if(IgnoreOwnerCollision)
    //     {
    //         AddCollisionExceptionWith();
    //     }
    // }
    public MultiplayerSynchronizer Sync { get; set; }
    public override void _Ready()
    {

        JustCreated = true;
        TimeLeft = DestroyAfterTime;
        
        Sync = new();
        AddChild(Sync);

        Sync.Name = $"OnReadySync";
        Sync.ReplicationConfig = new();
        Sync.ReplicationConfig.AddProperty($":TimeLeft");
        Sync.ReplicationConfig.AddProperty($":position");
        Sync.ReplicationConfig.AddProperty($":rotation");
    }

    public override void _Process(double delta)
    {
        if (FadedObject != null)
        {
            double fade = Math.Clamp(
                (FadeTime - TimeLeft) / FadeTime,
                0, 1
            );

            FadedObject.SelfModulate = Color.Color8(
                255,
                255,
                255,
                (byte)((1 - fade) * 255)
            );
        }

        if (RotationSpeed > 0)
        {
            Rotation += (float)(RotationSpeed * 2 * Math.PI * delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!Multiplayer.IsServer()) return;

        if (ProjectileGravity > 0 && !IsOnFloor())
            Velocity = Velocity + new Vector2(0, ProjectileGravity * (float)delta);

        var oldPos = Position;

        MoveAndSlide();

        if (Stationary)
            Position = oldPos;

        var collisions = GetSlideCollisionCount();

        if (collisions > 0)
        {
            var objs = new GodotObject[collisions];
            for (int i = 0; i < collisions; i++)
            {
                objs[i] = GetSlideCollision(i).GetCollider();
            }

            GD.Print($"Slided with {objs.Length}");
            OnCollisionRaw(objs);
        }

        TimeLeft = Mathf.MoveToward(TimeLeft, 0, delta);
        JustCreated = false;

        if (TimeLeft <= 0)
        {
            QueueFree();
        }
    }

    protected virtual void OnCollisionRaw(GodotObject[] rawObjects)
    {
        var nodes = new List<Node>();
        var entities = new List<Entity>();
        var players = new List<Player>();

        foreach (var node in rawObjects.Distinct())
        {
            var type = node.GetType();

            if (!type.IsAssignableTo(typeof(Node)))
                continue;

            nodes.Add((Node)node);

            if (!type.IsAssignableTo(typeof(IEntityContainer)))
                continue;

            entities.Add(((IEntityContainer)node).Entity);

            if (!type.IsAssignableTo(typeof(Player)))
                continue;

            players.Add((Player)node);
        }

        OnCollision(nodes.ToArray(), entities.ToArray(), players.ToArray());
    }

    public virtual void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        GD.Print($"Collision with {nodes.Length}N, {entities.Length}E, {players.Length}P");

        var targets = entities.Where(ent => ent != OwnerEntity);

        if (targets.Any() || nodes.Length != entities.Length)
        {
            foreach (var item in targets)
            {
                GD.Print($"Projectile collided with {item.Name}");
            }

            if (DestroyOnCollision)
            {
                // QueueFree();
                TimeLeft = DestroyOnCollisionDelay;
            }
        }
    }

    public virtual void Shoot(Vector2 from, Vector2 to)
    {
        Position = from;

        LookAt(to);

        Velocity = new Vector2(ProjectileVelocity, 0).Rotated(Rotation);
    }

    protected virtual void ApplyDamageFromProjectile(Entity target)
    {
        var dmg = new Damage(OwnerEntity);
        dmg.FlatValue = DamageFlat;
        dmg.PercentageValue = DamagePercentage;
        dmg.Flags = DamageFlags;

        target.ApplyDamage(dmg);
    }

}
