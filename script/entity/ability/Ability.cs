using Godot;
using System;

public partial class Ability : Node
{
    [ExportSubgroup("Settings")]
    [Export] public string DisplayName { get; set; } = "Unnamed ability";
    [Export] public string ShortDescription { get; set; } = "Abcdefg";
    [Export] public Texture2D IconTexture { get; set; }
    [Export] public AbilityCastMode CastMode { get; set; } = AbilityCastMode.Instant;
    [Export] public double CastTime { get; set; } = 0;
    [Export] public double Cooldown { get; set; } = 1;

    public double CooldownLeft { get; set; } = 0;
    public double CastTimeLeft { get; set; } = 0;
    public bool IsCasting => CastTimeLeft > 0;
    public bool IsOnCooldown => CooldownLeft > 0;
    
    public virtual AbilityUsageTrialResult CanUse(IEntityContainer owner)
    {
        if(IsOnCooldown)
            return AbilityUsageTrialResult.OnCooldown;
        
        if(IsCasting)
            return AbilityUsageTrialResult.IsAlreadyCasting;

        if(!CastMode.HasFlag(AbilityCastMode.CastableDuringCC) && owner.Entity.IsSilenced)
            return AbilityUsageTrialResult.IsSilenced;

        if(!CastMode.HasFlag(AbilityCastMode.CastableDuringOther) && owner.Entity.IsCasting)
            return AbilityUsageTrialResult.IsCastingOther;

        return AbilityUsageTrialResult.OK;
    }

    // This method is called when entity wants to cast this ability.
    // The entity should earlier check whether this is possible using CanUse.
    // Usually, this method does not need to be overriden.
    public virtual void Use(IEntityContainer owner)
    {
        if(CastTime > 0)
        {
            CastTimeLeft = CastTime;
            return;
        }

        CooldownLeft += Cooldown;
        Cast(owner);
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
                Cast(owner);
            }
        }
    }

}
