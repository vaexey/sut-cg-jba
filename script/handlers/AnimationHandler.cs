using Godot;
using System;

public partial class AnimationHandler : Node
{
    [ExportSubgroup("Nodes")]
    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    public void HandleHorizontalFlip(float direction)
    {
        if(direction == 0)
            return;
        
        Sprite.FlipH = direction < 0;
    }
    public void HandleWalk(float speed)
    {
        var animSpeed = speed / 200;
        var walk = Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left");
        if (walk)
        {
            Sprite.Play("walk", animSpeed);
        }
        else
        {
            Sprite.Play("idle");
        }

    }
}
