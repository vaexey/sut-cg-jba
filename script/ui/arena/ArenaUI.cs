using Godot;
using System;
using System.Linq;

public partial class ArenaUI : CanvasLayer
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Label StatsLabel;

    [Export] public AbilityBar AbilityBar;
    [Export] public EntityStats PlayerStats;
    [Export] public EntityStats EnemyStats;

    [Export] public World World;

    // public override string[] _GetConfigurationWarnings()
    // {
    //     return Assertions.Stack(base._GetConfigurationWarnings())
	// 		.AssertNotNull(StatsLabel)
	// 		.AssertNotNull(PlayerEntity);
    // }

    public override void _Process(double delta)
    {
        if (World.LeftPlayer == null)
        {
            return;
        }

        // StatsLabel.Text = $"Beverage: {
        //         Player.Entity.Beverage.Percentage
        //     }\nStamina: {
        //         Player.Entity.Stamina.Percentage
        //     }\nInspiration: {
        //         Player.Entity.Inspiration.Percentage
        //     }";
        StatsLabel.Text = "CC: " + World.LocalPlayer.Entity.CC.GetEffects().Select(cc => cc.DisplayName).ToArray().Join(", ");
        AbilityBar.Entity = World.LocalPlayer.Entity;

        PlayerStats.Entity = World.LeftPlayer.Entity;

        if (World.RightPlayer == null)
        {
            return;
        }

        EnemyStats.Entity = World.RightPlayer.Entity;
    }
}
