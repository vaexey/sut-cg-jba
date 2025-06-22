using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class World : Node2D
{
    [ExportSubgroup("Nodes")]
    [Export] public Node2D EntitiesContainer { get; set; }
    [Export] public Node2D LeftSpawn { get; set; }
    [Export] public Node2D RightSpawn { get; set; }

    [Export] public bool GameActive { get; set; } = true;

    public bool PlayersPresent => LocalPlayer != null && RemotePlayer != null;

    public bool LocalDefeat => PlayersPresent && !LocalPlayer.Entity.IsAlive;
    public bool LocalVictory => PlayersPresent && !RemotePlayer.Entity.IsAlive;

    public Player LeftPlayer => EntitiesContainer
        .GetChildren()
        .Select(node => (Player)node)
        .Where(p => p.Id == 1)
        .FirstOrDefault();

    public Player RightPlayer => EntitiesContainer
        .GetChildren()
        .Select(node => (Player)node)
        .Where(p => p.Id != 1)
        .FirstOrDefault();

    public Player LocalPlayer => Multiplayer.GetUniqueId() == 1 ? LeftPlayer : RightPlayer;
    public Player RemotePlayer => Multiplayer.GetUniqueId() == 1 ? RightPlayer : LeftPlayer;

    public IEnumerable<SimpleProjectile> Projectiles => GetChildren()
            .Where(c => c.GetType().IsAssignableTo(typeof(SimpleProjectile)))
            .Select(n => (SimpleProjectile)n);

    [Signal]
    public delegate void OnDefeatEventHandler();
    [Signal]
    public delegate void OnVictoryEventHandler();

    public Entity[] GetEntities()
    {
        return EntitiesContainer.GetChildren()
            .Where(child => child.GetType().IsAssignableTo(typeof(IEntityContainer)))
            .Select(child => (IEntityContainer)child)
            .Select(iec => iec.Entity)
            .ToArray();
    }

    // public IEntityContainer GetEnemy() => (IEntityContainer)MainEnemy;

    public void AddProjectile(SimpleProjectile projectile)
    {
        AddChild(projectile, true);
    }

    public static World FindFor(Node node)
    {
        var parent = node.GetParent();

        if (parent == null)
        {
            throw new ArgumentException("Could not find a World in the parent tree");
        }

        if (parent.GetType().IsAssignableTo(typeof(World)))
        {
            return (World)parent;
        }

        return FindFor(parent);
    }

    public Vector2 SpawnPointFor(Entity ent)
    {
        if (((Player)ent.Parent2D).Id == 1)
        {
            return LeftSpawn.Position;
        }
        else
        {
            return RightSpawn.Position;
        }
    }

    public override void _Ready()
    {
        GameActive = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        // if (!Multiplayer.IsServer()) return;

        if (GameActive && (LocalVictory || LocalDefeat))
        {
            GameActive = false;

            if (LocalDefeat)
                EmitSignal(SignalName.OnDefeat);
            else if (LocalVictory)
                EmitSignal(SignalName.OnVictory);
        }
    }

    public void ResetWorld()
    {
        if (LeftPlayer != null)
            LeftPlayer.Entity.Spawn();

        if (RightPlayer != null)
            RightPlayer.Entity.Spawn();

        foreach (var proj in Projectiles)
            proj.QueueFree();

        GameActive = true;
    }
}
