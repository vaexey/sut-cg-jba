using Godot;
using System;
using System.Linq;

public partial class World : Node
{
    [ExportSubgroup("Nodes")]
    [Export] public Node EntitiesContainer { get; set; }

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

        if(parent == null)
        {
            throw new ArgumentException("Could not find a World in the parent tree");
        }

        if(parent.GetType().IsAssignableTo(typeof(World)))
        {
            return (World)parent;
        }

        return FindFor(parent);
    }
}
