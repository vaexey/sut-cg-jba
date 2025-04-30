using Godot;
using System;

public partial class AbilityIcon : Control
{
    [ExportSubgroup("Nodes")]
    [Export] public Ability Ability { get; set; }
    [Export] public Node2D EntityContainer { get; set; }

    [Export] public CanvasGroup TintCanvas { get; set; }
    [Export] public Sprite2D IconSprite { get; set; }
    [Export] public Label OverlayLabel { get; set; }

    // protected IEntityContainer _EntityContainer => (IEntityContainer)EntityContainer;

    public override void _Process(double delta)
    {
        var trial = Ability.CanUse((IEntityContainer)EntityContainer);
        var canUse = trial == AbilityUsageTrialResult.OK;

        var shader = (ShaderMaterial)TintCanvas.Material;
        shader.SetShaderParameter("tint_enable", !canUse);

        // TODO: Probably not efficient
        IconSprite.Texture = Ability.IconTexture;

        OverlayLabel.Text = (trial == AbilityUsageTrialResult.OnCooldown) ?
            Ability.CooldownLeft.ToString("N1") : 
            "";

    }
}