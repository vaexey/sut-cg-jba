using Godot;
using System;

public partial class ArenaCamera2D : Camera2D
{
    [ExportSubgroup("Nodes")]
    // [Export]
    // public Node2D MainNode { get; set; }
    // [Export]
    // public Node2D DirectionNode { get; set; }
    [Export] World World { get; set; }

    public float MaxRange { get; set; } = 200;

    public override void _Process(double delta)
    {
        if (World.RemotePlayer == null)
            return;

        var midpoint = World.LocalPlayer.Position.Lerp(World.RemotePlayer.Position, 0.5f);
        var distance = (World.LocalPlayer.Position - midpoint).Length();

        if(distance > MaxRange)
        {
            // midpoint = MainNode.Position.Lerp(DirectionNode.Position, MaxRange / distance);
            midpoint = World.LocalPlayer.Position + (World.RemotePlayer.Position - World.LocalPlayer.Position).Normalized() * MaxRange;
        }

        Position = midpoint;
    }

}
