using Godot;
using System;

public partial class Ability : Node
{
    [ExportSubgroup("Settings")]
    [Export] public string DisplayName { get; set; } = "Unnamed ability";
    [Export] public string ShortDescription { get; set; } = "Abcdefg";
    [Export] public AbilityPower PowerType { get; set; } = AbilityPower.Basic;
    [Export] public AbilityCategory CategoryType { get; set; } = AbilityCategory.Physical;
    [Export] public Texture2D IconTexture { get; set; }
    [Export] public AbilityCastMode CastMode { get; set; } = AbilityCastMode.Instant;
    [Export] public double CastTime { get; set; } = 0;
    [Export] public double Cooldown { get; set; } = 1;
    [Export] public double UseCostFlat { get; set; } = 0;
    [Export] public double UseCostPercentage { get; set; } = 0;

    public double CooldownLeft { get; set; } = 0;
    public double CastTimeLeft { get; set; } = 0;
    public bool IsCasting => CastTimeLeft > 0;
    public bool IsOnCooldown => CooldownLeft > 0;
    
    
	[Signal]
	public delegate void OnCastEventHandler();

    public virtual AbilityUsageTrialResult CanUse(IEntityContainer owner)
    {
        if (!owner.Entity.IsAlive)
            return AbilityUsageTrialResult.EntityDead;

        if (IsOnCooldown)
            return AbilityUsageTrialResult.OnCooldown;

        var cost = GetUseCostTotal(owner.Entity);

        if (CategoryType == AbilityCategory.Physical)
            if (owner.Entity.Stamina.Value < cost)
                return AbilityUsageTrialResult.NoStamina;

        if (CategoryType == AbilityCategory.Inspired)
            if (owner.Entity.Inspiration.Value < cost)
                return AbilityUsageTrialResult.NoInspiration;

        if (IsCasting)
            return AbilityUsageTrialResult.IsAlreadyCasting;

        if (!CastMode.HasFlag(AbilityCastMode.CastableDuringCC) && owner.Entity.IsSilenced)
            return AbilityUsageTrialResult.IsSilenced;

        if (!CastMode.HasFlag(AbilityCastMode.CastableDuringOther) && owner.Entity.IsCasting)
            return AbilityUsageTrialResult.IsCastingOther;

        return AbilityUsageTrialResult.OK;
    }

    // This method is called when entity wants to check
    // the cost of this ability (eg. for UI)
    // Value is flat Stamina/Inspiration based on category.
    // Usually, this method does not need to be overriden.
    public virtual double GetUseCostTotal(Entity ent)
    {
        double value = 0;

        if (CategoryType == AbilityCategory.Physical)
            value = ent.Stamina.Value * ent.PassiveAttributes.StaminaUsageModifier;

        if (CategoryType == AbilityCategory.Inspired)
            value = ent.Inspiration.Value;

        return UseCostFlat + UseCostPercentage * value;
    }

    // This method is called when respective ability cost
    // should be consumed.
    // Usually, this method does not need to be overriden.
    public virtual void ConsumeCost(Entity ent)
    {
        var cost = GetUseCostTotal(ent);
        if (CategoryType == AbilityCategory.Physical)
            ent.ConsumeStamina(cost);

        if (CategoryType == AbilityCategory.Inspired)
            ent.ConsumeInspiration(cost);
    }

    // This method is called when entity wants to cast this ability.
    // The entity should earlier check whether this is possible using CanUse.
    // Usually, this method does not need to be overriden.
    public virtual void Use(IEntityContainer owner)
    {
        if (CastTime > 0)
        {
            CastTimeLeft = CastTime;
            return;
        }

        CooldownLeft += Cooldown;

        ConsumeCost(owner.Entity);
        Cast(owner);
        EmitSignal(SignalName.OnCast);
    }

    // This method is called to execute the ability effect,
    // e.g. after cast time is complete.
    // This method should be overwritten for each ability.
    public virtual void Cast(IEntityContainer owner)
    {
        
    }

    public virtual void ProcessOwner(IEntityContainer owner, double delta)
    {
        ProcessCooldown(delta);
        ProcessCastTime(delta, owner);
    }

    protected virtual void ProcessCooldown(double delta)
    {
        CooldownLeft = Math.Max(0, CooldownLeft - delta);
    }

    public virtual void OnCastCancelInput(IEntityContainer owner)
    {
        if(CastMode.HasFlag(AbilityCastMode.CastTimeCancellable) && IsCasting)
        {
            CastTimeLeft = 0;
        }
    }

    protected virtual void ProcessCastTime(double delta, IEntityContainer owner)
    {
        if(IsCasting)
        {
            if(CastMode.HasFlag(AbilityCastMode.CastTimeResetOnInterrupt) && owner.Entity.IsSilenced)
            {
                CastTimeLeft = CastTime;
            }

            if(CastMode.HasFlag(AbilityCastMode.CastTimeInterruptable) && owner.Entity.IsSilenced)
            {
                CastTimeLeft = 0;
                return;
            }
            
            CastTimeLeft = Math.Max(0, CastTimeLeft - delta);

            if(!IsCasting)
            {
                CooldownLeft += Cooldown;
                ConsumeCost(owner.Entity);
                Cast(owner);
                EmitSignal(SignalName.OnCast);
            }
        }
    }

}
