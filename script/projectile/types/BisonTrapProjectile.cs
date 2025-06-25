using Godot;
using System;
using System.Collections.Generic;

public partial class BisonTrapProjectile : SimpleProjectile
{
    [Export] public double ArmTime { get; set; } = 2;
    [Export] public AnimatedSprite2D Sprite { get; set; }

    public bool Landed { get; set; } = false;
    public bool Armed { get; set; } = false;
    public bool Triggered { get; set; } = false;

    public double ArmTimeLeft { get; set; } = 0;

    public List<Entity> CaughtEntities { get; set; } = new();

    public override void OnCollision(Node[] nodes, Entity[] entities, Player[] players)
    {
        base.OnCollision(nodes, entities, players);

        if (!Landed)
        {
            Landed = true;
            Armed = false;
            ArmTimeLeft = ArmTime;
            Stationary = true;

            // CollisionMask = 4; // Player
            SetCollisionMaskValue(4, true);
            SetCollisionMaskValue(3, false);
        }

        if (Armed && !Triggered)
        {
            Triggered = true;
            TimeLeft = FadeTime;

            Sprite.Play("triggered");
            GD.Print("Trap triggered");

            foreach (var ent in entities)
            {
                ApplyDamageFromProjectile(ent);

                var slow = CrowdControlLibrary.ShortNap.Make();
                ent.CC.AddEffect(slow, 1.5);

                CaughtEntities.Add(ent);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Landed)
        {
            Velocity = Vector2.Zero;
            ProjectileGravity = 0;

            if (!Armed)
            {
                ArmTimeLeft = Mathf.MoveToward(ArmTimeLeft, 0, delta);

                if (ArmTimeLeft <= 0)
                {
                    Armed = true;
                    Sprite.Play("open");
                }
            }
        }

        if (Triggered)
        {
            foreach (var ent in CaughtEntities)
            {
                ent.Parent2D.Position = new(
                    Mathf.MoveToward(
                        ent.Parent2D.Position.X,
                        Position.X,
                        (float)delta * ProjectileVelocity
                    ),
                    ent.Parent2D.Position.Y
                );
            }
        }
    }
}
