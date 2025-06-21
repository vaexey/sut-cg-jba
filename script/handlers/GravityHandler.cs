using Godot;
using System;

public partial class GravityHandler : Node
{
	[ExportSubgroup("Settings")]
	[Export]
	public float Gravity { get; set; } = 700;

	public bool IsFalling { get; protected set; } = false;

	public void HandleGravity(CharacterBody2D body, double delta)
	{
		if(!body.IsOnFloor())
			body.Velocity = body.Velocity + new Vector2(0, Gravity * (float)delta);
		
		IsFalling = body.Velocity.Y > 0 && !body.IsOnFloor();
	}
}
