using Godot;
using System;
using System.Threading.Tasks;

public partial class AnimationHandler : Node
{
    [ExportSubgroup("Nodes")]
    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    public ShaderMaterial Shader => (ShaderMaterial)Sprite.Material;

    private bool playedDeath = false;
    public void HandleHorizontalFlip(float direction)
    {
        if (direction == 0)
            return;

        Sprite.FlipH = direction < 0;
    }

    public void HandleAnimations(CharacterBody2D body, Entity entity)
    {
        var player = (Player)entity.Parent2D;
        Shader.SetShaderParameter("enable", player.Id != 1);

        var animSpeed = entity.PassiveAttributes.Speed(entity.Stamina.Value) / 200;
        var isGoingUp = body.Velocity.Normalized().Y < 0;


        if (!entity.IsAlive)
        {
            if (playedDeath)
            {
                return;
            }

            playedDeath = true;
            Sprite.Play("death");
            return;
        }

        playedDeath = false;

        if (entity.IsCasting)
        {
            var ability = entity.Casting;

            if (ability.GetType().IsAssignableTo(typeof(AutoJodlerAbility)))
            {
                Sprite.Play("auto_jodler");
            }
            else
            {
                Sprite.Play("guitar");
            }

            return;
        }
        
        if (entity.IsSilenced && entity.IsCrippledHorizontally && entity.IsCrippledVertically)
            {
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

        Shader.SetShaderParameter("modulate", new Color(1, 0, 0));
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        // Shader.SetShaderParameter("modulate", Colors.White);

        // flash white
        Shader.SetShaderParameter("modulate", new Color(1, 1, 1));
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        // Shader.SetShaderParameter("modulate", Colors.White);

        // flash red again
        Shader.SetShaderParameter("modulate", new Color(1, 0, 0));
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        Shader.SetShaderParameter("modulate", Colors.White);
    }

}
