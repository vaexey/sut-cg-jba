using Godot;
using System;
using System.Threading.Tasks;

public partial class AnimationHandler : Node
{
    [ExportSubgroup("Nodes")]
    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    private bool playedDeath = false;
    public void HandleHorizontalFlip(float direction)
    {
        if(direction == 0)
            return;
        
        Sprite.FlipH = direction < 0;
    }

    public void HandleAnimations(CharacterBody2D body, Entity entity)
    {

        var animSpeed = entity.PassiveAttributes.Speed / 200;
        var isGoingUp = body.Velocity.Normalized().Y < 0;

        if (playedDeath)
        {
            return;
        }

        if (!entity.IsAlive)
        {
            playedDeath = true;
            Sprite.Play("death");
            return;
        }
        if (entity.IsSilenced && entity.IsCrippledHorizontally && entity.IsCrippledVertically) {
            Sprite.Play("sleep", 2.0f);
            return;
        }

        if (entity.IsCrippledHorizontally || entity.IsCrippledVertically)
        {
            // cripple
        }

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
    public async void OnDamaged()
    {
        // flash red
        Sprite.Modulate = new Color(1, 0, 0);
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        Sprite.Modulate = Colors.White;

        // flash white
        Sprite.Modulate = new Color(1, 1, 1);
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        Sprite.Modulate = Colors.White;

        // flash red again
        Sprite.Modulate = new Color(1, 0, 0);
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        Sprite.Modulate = Colors.White;
    }

}
