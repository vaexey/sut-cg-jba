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
}
