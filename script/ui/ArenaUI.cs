using Godot;
using System;

public partial class ArenaUI : CanvasLayer
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Label StatsLabel;

    [Export]
    public Entity PlayerEntity;

    // public override string[] _GetConfigurationWarnings()
    // {
    //     return Assertions.Stack(base._GetConfigurationWarnings())
	// 		.AssertNotNull(StatsLabel)
	// 		.AssertNotNull(PlayerEntity);
    // }

    public override void _Process(double delta)
    {
        StatsLabel.Text = $"Beverage: {
                PlayerEntity.Beverage
            }\nStamina: {
                PlayerEntity.Stamina
            }\nInspiration: {
                PlayerEntity.Inspiration
            }";
    }
}
