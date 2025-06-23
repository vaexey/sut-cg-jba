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

        var follow = !Input.IsActionPressed("camera_override");

        var localPoint = World.LocalPlayer.Position;
        var remotePoint = World.RemotePlayer.Position;

        if (!follow)
        {
            var mouseOffset = GetGlobalMousePosition() - localPoint;
            var mouseDirection = mouseOffset.Normalized();
            // var mouseDistance = mouseOffset.Length();

            remotePoint = localPoint + mouseDirection * 3 * MaxRange;
        }

        var midPoint = localPoint.Lerp(remotePoint, 0.5f);
        var distance = (localPoint - midPoint).Length();

        if (distance > MaxRange)
        {
            midPoint = localPoint + (remotePoint - localPoint).Normalized() * MaxRange;
        }

        Position = midPoint;
    }

}
