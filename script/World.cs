using Godot;
using System;
using System.Linq;

public partial class World : Node
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Node EntitiesContainer { get; set; }
    [Export]
    public Player Player { get; set; }

    public Entity[] GetEntities()
    {
        return EntitiesContainer.GetChildren()
            .Where(child => child.GetType().IsAssignableTo(typeof(IEntityContainer)))
            .Select(child => (IEntityContainer)child)
            .Select(iec => iec.Entity)
            .ToArray();
    }

    public void AddProjectile(SimpleProjectile projectile)
    {
        AddChild(projectile);
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
