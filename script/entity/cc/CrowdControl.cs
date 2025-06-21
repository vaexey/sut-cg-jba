using Godot;
using System;
using System.Linq;

public partial class CrowdControl : Node
{

    [ExportSubgroup("Nodes")]
    [Export]
    public Entity ParentEntity { get; set; }

    public CrowdControlEffect[] GetEffects()
    {
        return GetChildren()
            .Where(child => child.GetType().IsAssignableTo(typeof(CrowdControlEffect)))
            .Select(child => (CrowdControlEffect)child)
            .ToArray();
    }

    public void AddEffect(CrowdControlEffect effect)
    {
        var key = effect.GetDuplicationKey();
        var duplicates = GetEffects()
            .Select(effect => (effect, key: effect.GetDuplicationKey()))
            .Where(it => it.key == key)
            .Select(it => it.effect)
            .ToArray();

        if(duplicates.Any())
        {
            GD.Print($"Trying to apply effect {effect.DisplayName} on entity with {duplicates.Length} similiar effects");

            bool skip = !effect.OnDuplicateEffects(ParentEntity, duplicates);

            if(skip)
            {
                return;
            }
        }

        AddChild(effect, true);
        effect.Start(ParentEntity);
    }

    public void AddEffect(CrowdControlEffect effect, double time)
    {
        effect.Time = time;
        AddEffect(effect);
    }

    public void Cleanse()
    {
        if (!Multiplayer.IsServer()) return;

        var effects = GetEffects();
    
        foreach (var cc in effects)
        {
            cc.End(ParentEntity);
            cc.QueueFree();
        }
    }

    public override void _Ready()
    {
        base._Ready();

        // AddEffect(CrowdControlLibrary.HikingInstinct.Make());
    }

    public override void _PhysicsProcess(double delta)
    {
        var effects = GetEffects();

        foreach(var cc in effects)
        {
            cc.Effect(ParentEntity, delta);

            cc.Time -= delta;

            if(cc.Time <= 0 && Multiplayer.IsServer())
            {
                cc.End(ParentEntity);
                cc.QueueFree();
            }
        }
    }
    
}
