using Godot;
using System;

public partial class BibiHendlCC : CrowdControlEffect
{
    [Export] public Node2D Bubble { get; set; }
    [Export] public Node2D Chicken { get; set; }

    public Node2D ReferenceNode;

    public override void Start(Entity effected)
    {
        // ReferenceNode = effected.World.Player;
        effected.PassiveAttributes.ReceivedDamagePhysicalModifier -= 1;
        effected.PassiveAttributes.ReceivedDamageInspiredModifier -= 1;
    }

    public override void End(Entity effected)
    {
        // TODO:
        // ReferenceNode = null;

        effected.PassiveAttributes.ReceivedDamagePhysicalModifier += 1;
        effected.PassiveAttributes.ReceivedDamageInspiredModifier += 1;
    }

    public override void Effect(Entity effected, double delta)
    {
        ReferenceNode = effected.Parent2D;
    }

    public override void _Process(double delta)
    {

        if (ReferenceNode != null)
        {
            Bubble.Position = ReferenceNode.Position;

            double percentage = Math.Pow(1 - Time / 2, 2);

            double transparency = 1 - percentage;
            float scale = ((float)percentage * 2f + 1) * 2f;

            Chicken.Scale = new Vector2(scale, scale);
            Chicken.SelfModulate = Color.Color8(255, 255, 255, (byte)(transparency * 255));
        }
    }
}
