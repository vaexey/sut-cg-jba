using Godot;
using System;

public partial class EntityStats : Control
{
    [ExportSubgroup("Nodes")]
    [Export] public Entity Entity { get; set; }
    
    [Export]
    public TextureProgressBar BeverageBar;
    [Export]
    public TextureProgressBar StaminaBar;
    [Export]
    public TextureProgressBar InspirationBar;

    public override void _Process(double delta)
    {
        // BeverageBar.Value = Entity.Beverage.Percentage * 100;
        // StaminaBar.Value = Entity.Stamina.Percentage * 100;
        // InspirationBar.Value = Entity.Inspiration.Percentage * 100;
        BeverageBar.MaxValue = Entity.Beverage.Max;
        BeverageBar.Value = Entity.Beverage.Value;
        StaminaBar.MaxValue = Entity.Stamina.Max;
        StaminaBar.Value = Entity.Stamina.Value;
        InspirationBar.MaxValue = Entity.Inspiration.Max;
        InspirationBar.Value = Entity.Inspiration.Value;
    }
}
