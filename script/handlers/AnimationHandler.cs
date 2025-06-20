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

    public void HandleAnimations(CharacterBody2D body, float speed)
    {

        var animSpeed = speed / 200;
        var isGoingUp = body.Velocity.Normalized().Y < 0;


        if (body.IsOnFloor())
        {
            if (body.Velocity.Length() > 0)
            {
                Sprite.Play("walk", animSpeed);
                return;
            }

            Sprite.Play("idle");
            return;

        }

        if (isGoingUp)
        {
            Sprite.Play("jump");
            return;

        }
        Sprite.Play("fall");
        

    }

}
