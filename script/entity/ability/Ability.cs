using Godot;
using System;

public partial class Ability : Node
{
    public struct AbilityUsageTrial
    {
        public bool Allowed;
        public string Message;

        public AbilityUsageTrial()
        {
            Allowed = true;
            Message = "";
        }

        public AbilityUsageTrial(string msg)
        {
            Allowed = false;
            Message = msg;
        }
    }

    [ExportSubgroup("Settings")]
    [Export] public string DisplayName { get; set; } = "Unnamed ability";
    [Export] public string ShortDescription { get; set; } = "Abcdefg";
    [Export] public double Cooldown { get; set; } = 1;

    public double CooldownLeft { get; set; } = 0;
    
    public virtual AbilityUsageTrial CanUse(IEntityContainer entity)
    {
        if(CooldownLeft > 0)
            return new("Cooldown");

        return new();
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
