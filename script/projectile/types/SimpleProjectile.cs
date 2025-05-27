using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SimpleProjectile : CharacterBody2D
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Entity OwnerEntity { get; set; }

    [ExportSubgroup("Settings")]
    [Export]
    public string DisplayName { get; set; } = "Unnamed projectile";
    [Export]
    public float ProjectileVelocity { get; set; } = 200;

    [Export]
    public float ProjectileGravity { get; set; } = 500;

    [Export]
    public bool DestroyOnCollision { get; set; } = true;

    // [Export]
    // public bool IgnoreOwnerCollision { get; set; } = true;

    // public override void _Ready()
    // {
    //     if(IgnoreOwnerCollision)
    //     {
    //         AddCollisionExceptionWith();
    //     }
    // }

    public override void _PhysicsProcess(double delta)
    {
        if(ProjectileGravity > 0 && !IsOnFloor())
            Velocity = Velocity + new Vector2(0, ProjectileGravity * (float)delta);

        MoveAndSlide();

        var collisions = GetSlideCollisionCount();

        if(collisions > 0)
        {
            var objs = new GodotObject[collisions];
            for(int i = 0; i < collisions; i++)
            {
                objs[i] = GetSlideCollision(i).GetCollider();
            }

            GD.Print($"Slided with {objs.Length}");
            OnCollisionRaw(objs);
        }
    }

    protected virtual void OnCollisionRaw(GodotObject[] rawObjects)
    {
        var nodes = new List<Node>();
        var entities = new List<Entity>();
        var players = new List<Player>();

        foreach(var node in rawObjects.Distinct())
        {
            var type = node.GetType();

            if(!type.IsAssignableTo(typeof(Node)))
                continue;

            nodes.Add((Node)node);

            if(!type.IsAssignableTo(typeof(IEntityContainer)))
                continue;

            entities.Add(((IEntityContainer)node).Entity);

            if(!type.IsAssignableTo(typeof(Player)))
                continue;

            players.Add((Player)node);
        }

        OnCollision(nodes.ToArray(), entities.ToArray(), players.ToArray());
    }

    public virtual void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        GD.Print($"Collision with {nodes.Length}N, {entities.Length}E, {players.Length}P");

        var targets = entities.Where(ent => ent != OwnerEntity);

        if(targets.Any() || nodes.Length != entities.Length)
        {
            foreach (var item in targets)
            {
                GD.Print($"Projectile collided with {item.Name}");
            }

            if(DestroyOnCollision)
            {
                QueueFree();
            }
        }
    }

    public virtual void Shoot(Vector2 from, Vector2 to)
    {
        Position = from;
        
        LookAt(to);

        Velocity = new Vector2(ProjectileVelocity, 0).Rotated(Rotation);
    }

}
