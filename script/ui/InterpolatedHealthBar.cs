using Godot;
using System;

[Tool]
public partial class InterpolatedHealthBar : TextureProgressBar
{
    [ExportSubgroup("Settings")]
    [Export]
    public double Speed { get; set; } = 0.5;

    // [Export]
    // public double Real { get; set; } = 0.25;

    [Export(PropertyHint.ColorNoAlpha)]
    public Color OpaqueTint { get; set; } = Color.Color8(255, 0, 0, 255);

    [Export]
    public Color TransparentTint { get; set; } = Color.Color8(255, 128, 128, 255);

    [ExportSubgroup("Nodes")]
    [Export]
    public TextureProgressBar Transparent { get; set; }
    [Export]
    public TextureProgressBar Opaque { get; set; }

    private double Real {
        get {
            return Value / MaxValue;
        }
    }
    public double Interpolated = 0.75;

    public override void _Process(double delta)
    {
        Transparent.TintProgress = TransparentTint;
        Opaque.TintProgress = OpaqueTint;

        Interpolated = Mathf.MoveToward(Interpolated, Real, Speed * delta);

        if(Interpolated > Real)
        {
            Transparent.Value = Interpolated * 100;
            Opaque.Value = Real * 100;
        } else {
            Transparent.Value = Real * 100;
            Opaque.Value = Interpolated * 100;
        }

    }
}
