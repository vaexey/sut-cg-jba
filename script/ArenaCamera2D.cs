using Godot;
using System;

public partial class ArenaCamera2D : Camera2D
{
    [ExportSubgroup("Nodes")]
    [Export]
    public Node2D MainNode { get; set; }
    [Export]
    public Node2D DirectionNode { get; set; }

    public float MaxRange { get; set; } = 200;

    public override void _Process(double delta)
    {
        // base._Process(delta);

        var midpoint = MainNode.Position.Lerp(DirectionNode.Position, 0.5f);
        var distance = (MainNode.Position - midpoint).Length();

        if(distance > MaxRange)
        {
            // midpoint = MainNode.Position.Lerp(DirectionNode.Position, MaxRange / distance);
            midpoint = MainNode.Position + (DirectionNode.Position - MainNode.Position).Normalized() * MaxRange;
        }

        Position = midpoint;
    }

}
