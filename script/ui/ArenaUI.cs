using Godot;
using System;
using System.Linq;

public partial class ArenaUI : CanvasLayer
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Label StatsLabel;
    
    [Export]
    public TextureProgressBar BeverageBar;
    [Export]
    public TextureProgressBar StaminaBar;
    [Export]
    public TextureProgressBar InspirationBar;

    [Export]
    public AbilityBar AbilityBar;

    [Export]
    public Player Player;

    // public override string[] _GetConfigurationWarnings()
    // {
    //     return Assertions.Stack(base._GetConfigurationWarnings())
	// 		.AssertNotNull(StatsLabel)
	// 		.AssertNotNull(PlayerEntity);
    // }

    public override void _Process(double delta)
    {
        // StatsLabel.Text = $"Beverage: {
        //         Player.Entity.Beverage.Percentage
        //     }\nStamina: {
        //         Player.Entity.Stamina.Percentage
        //     }\nInspiration: {
        //         Player.Entity.Inspiration.Percentage
        //     }";
        StatsLabel.Text = "CC: " + Player.Entity.CC.GetEffects().Select(cc => cc.DisplayName).ToArray().Join(", ");

        BeverageBar.Value = Player.Entity.Beverage.Percentage * 100;
        StaminaBar.Value = Player.Entity.Stamina.Percentage * 100;
        InspirationBar.Value = Player.Entity.Inspiration.Percentage * 100;

        AbilityBar.EntityContainer = Player;
    }
}
