using Godot;
using System;

public partial class Ability : Node
{
    [ExportSubgroup("Settings")]
    [Export] public string DisplayName { get; set; } = "Unnamed ability";
    [Export] public string ShortDescription { get; set; } = "Abcdefg";
    [Export] public Texture2D IconTexture { get; set; }
    [Export] public double Cooldown { get; set; } = 1;

    public double CooldownLeft { get; set; } = 0;
    
    public virtual AbilityUsageTrialResult CanUse(IEntityContainer entity)
    {
        if(CooldownLeft > 0)
            return AbilityUsageTrialResult.OnCooldown;

        return AbilityUsageTrialResult.OK;
    }

    public virtual void Use(IEntityContainer entity)
    {
        CooldownLeft += Cooldown;
    }

    protected virtual void ProcessCooldown(double delta)
    {
        CooldownLeft = Math.Max(0, CooldownLeft - delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessCooldown(delta);
    }

}
