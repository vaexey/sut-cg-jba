using Godot;
using System;

public partial class MovementHandler : Node
{
	[ExportSubgroup("Nodes")]
	[Export]
	public Entity Entity { get; set; }
	public bool IsFalling { get; protected set; } = false;

	[Signal]
	public delegate void SetGrassSoundEventHandler(bool grass);
	[Rpc(CallLocal = false)] private void RpcSetGrassSound(bool grass) => EmitSignal(SignalName.SetGrassSound, grass);

	public override void _Ready()
	{
		if (Multiplayer.IsServer())
		{
			SetGrassSound += (grass) => Rpc(MethodName.RpcSetGrassSound, grass);
		}
	}

	public void HandleHorizontal(CharacterBody2D body, Entity ent, double delta, float direction)
	{
		if (!Multiplayer.IsServer())
			return;

		float accel;

		if (body.IsOnFloor())
		{
			accel = direction != 0 ? Entity.PassiveAttributes.GroundAcceleration : Entity.PassiveAttributes.GroundDeceleration;
		}
		else
		{
			accel = direction != 0 ? Entity.PassiveAttributes.AirAcceleration : Entity.PassiveAttributes.AirDeceleration;
		}

		if (ent.IsCrippledHorizontally)
		{
			direction = 0;
			accel = Entity.PassiveAttributes.GroundDeceleration;
		}

		EmitSignal(
			SignalName.SetGrassSound,
			body.IsOnFloor() && body.Velocity.X != 0
			);

		body.Velocity = new(
			Mathf.MoveToward(body.Velocity.X, direction * Entity.PassiveAttributes.Speed, accel * (float)delta),
			body.Velocity.Y
		);
	}

	public void HandleVertical(CharacterBody2D body, Entity ent, double delta, float input)
	{
		IsFalling = body.Velocity.Y > 0 && !body.IsOnFloor();

		bool wantsToFall = input > 0;
		bool wantsToUp = input < 0;

		float accel = 0;
		float speed = 100000;

		if (!body.IsOnFloor())
			accel = ent.PassiveAttributes.Gravity;

		if (ent.IsOnLadder)
		{
			if (input < 0)
			{
				speed = -ent.PassiveAttributes.ClimbingSpeed;
				accel = ent.PassiveAttributes.LadderAcceleration;
			}
			else if (input == 0)
			{
				speed = 0;
				accel = ent.PassiveAttributes.LadderDeceleration;
			}
		}

		// if (ent.IsOnLadder && wantsToUp)
			// {
			// 	accel = -ent.PassiveAttributes.LadderAcceleration;
			// 	terminal = -ent.PassiveAttributes.ClimbingSpeed;
			// }
			// else if (ent.IsOnLadder && !wantsToFall)
			// {
			// 	accel = 0;
			// }

			body.Velocity = new(
			body.Velocity.X,
			Mathf.MoveToward(body.Velocity.Y, speed, accel * (float)delta)
		);
		// if (!body.IsOnFloor())
		// 		body.Velocity = body.Velocity + new Vector2(0, ent.PassiveAttributes.Gravity * (float)delta);

	}
	
}
