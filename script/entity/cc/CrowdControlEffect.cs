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
    [Export] public Texture2D IconTexture { get; set; }

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
    
    protected virtual bool OnDuplicateSelectLongest(Entity effected, CrowdControlEffect[] duplicates)
    {
        if (duplicates.Length == 0)
            return true;

        var times = duplicates.Select(cc => cc.Time).ToList();
        times.Add(Time);

        var max = times.Max();

        duplicates[0].Time = max;

        return false;
    }

    // The key on which duplication will be determined
    public virtual string GetDuplicationKey()
    {
        return DisplayName;
    }
}
