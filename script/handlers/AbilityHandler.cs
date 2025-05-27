using Godot;
using System;

public partial class AbilityHandler : Node
{
    // [ExportSubgroup("Nodes")]

    public void HandleAbilities(IEntityContainer ec, InputHandler input, double delta)
    {
        var abilities = ec.Entity.Abilities;

        if(input.GetAbilityBasicNext())
        {
            abilities.BasicIndex += 1;
        } else if(input.GetAbilityBasicPrevious())
        {
            abilities.BasicIndex -= 1;
        }

        abilities.BasicIndex %= 10;
        if(abilities.BasicIndex < 0)
            abilities.BasicIndex += 10;
        
        int abilityInputs = 0;

        if(input.GetAbilityBasic()) abilityInputs++;
        if(input.GetAbilityComplex1()) abilityInputs++;
        if(input.GetAbilityComplex2()) abilityInputs++;
        if(input.GetAbilityComplex3()) abilityInputs++;
        if(input.GetAbilityGodlike()) abilityInputs++;

        var node = (CharacterBody2D)ec;

        if(ec.Entity.IsCasting && (abilityInputs > 0 || node.Velocity.Length() > 0))
        {
            abilities.OnCastCancelInput(ec);
        }

        HandleAbility(ec, abilities.BasicSelected, input.GetAbilityBasic(), delta);
        HandleAbility(ec, abilities.Complex1, input.GetAbilityComplex1(), delta);
        HandleAbility(ec, abilities.Complex2, input.GetAbilityComplex2(), delta);
        HandleAbility(ec, abilities.Complex3, input.GetAbilityComplex3(), delta);
        HandleAbility(ec, abilities.Godlike, input.GetAbilityGodlike(), delta);
    }

    protected void HandleAbility(IEntityContainer ec, Ability ability, bool input, double delta)
    {
        if(ability == null)
        {
            if(input)
                GD.Print("Tried to use nonexistent ability!");

            return;
        }

        ability.ProcessOwner(ec, delta);

        if(input &&
            ability.CanUse(ec) == AbilityUsageTrialResult.OK)
        {
            ability.Use(ec);
        }
    }
}
