using Godot;
using System;

public partial class BibiHendlCC : CrowdControlEffect
{
    [Export] public Node2D Bubble { get; set; }
    [Export] public Node2D Chicken { get; set; }

    public override void Start(Entity effected)
    {
        effected.PassiveAttributes.ReceivedDamagePhysicalModifier -= 1;
        effected.PassiveAttributes.ReceivedDamageInspiredModifier -= 1;
    }

    public override void End(Entity effected)
    {
        effected.PassiveAttributes.ReceivedDamagePhysicalModifier += 1;
        effected.PassiveAttributes.ReceivedDamageInspiredModifier += 1;
    }

    public override void _Process(double delta)
    {
        double percentage = Math.Pow(1 - Time / 2, 2);

        double transparency = 1 - percentage;
        float scale = ((float)percentage * 2f + 1) * 2f;

        Chicken.Scale = new Vector2(scale, scale);
        Chicken.SelfModulate = Color.Color8(255, 255, 255, (byte)(transparency * 255));
    }
}
