using Godot;
using System;
using System.Linq;

public partial class CrowdControlEffect : Node
{
    [ExportSubgroup("Settings")]
    [Export]
    public string DisplayName { get; set; } = "Unnamed CC";
    [Export]
    public string ShortDescription { get; set; } = "Abcdeefg";

    [ExportSubgroup("Modifiers")]
    [Export]
    public virtual double Time { get; set; } = 0.0;

    public virtual void Start(Entity effected)
    {

    }

    public virtual void Effect(Entity effected, double delta)
    {
        
    }
    
    public virtual void End(Entity effected)
    {

    }

    // Return true, if a duplicate effect can be added to the effects tree
    public virtual bool OnDuplicateEffects(Entity effected, CrowdControlEffect[] duplicates)
    {
        return true;
    }

    // The key on which duplication will be determined
    public virtual string GetDuplicationKey()
    {
        return DisplayName;
    }
}
