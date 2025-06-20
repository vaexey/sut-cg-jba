using Godot;
using System;

public partial class MovementHandler : Node
{
	[ExportSubgroup("Nodes")]
	[Export]
	public Entity Entity { get; set; }

	[Signal]
	public delegate void SetGrassSoundEventHandler(bool grass);

	public void HandleHorizontal(CharacterBody2D body, Entity ent, double delta, float direction)
	{
		float accel;

		if(body.IsOnFloor())
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
	
}
