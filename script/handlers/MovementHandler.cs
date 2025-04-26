using Godot;
using System;

public partial class MovementHandler : Node
{
	[ExportSubgroup("Settings")]
	[Export]
	public float Speed { get; set; } = 200;

	[Export]
	public float GroundAcceleration { get; set; } = 3000;
	
	[Export]
	public float GroundDeceleration { get; set; } = 4000;

	[Export]
	public float AirAcceleration { get; set; } = 5000;

	[Export]
	public float AirDeceleration { get; set; } = 1500;

	public void HandleHorizontal(CharacterBody2D body, double delta, float direction)
	{
		float accel;

		if(body.IsOnFloor())
		{
			accel = direction != 0 ? GroundAcceleration : GroundDeceleration;
		}
		else
		{
			accel = direction != 0 ? AirAcceleration : AirDeceleration;
		}
		
		body.Velocity = new(
			Mathf.MoveToward(body.Velocity.X, direction * Speed, accel * (float)delta),
			body.Velocity.Y
		);
	}
	
}
