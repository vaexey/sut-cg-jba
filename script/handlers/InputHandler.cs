using Godot;
using System;

public partial class InputHandler : Node
{
	public float HorizontalInput { get; set; } = 0;

	public override void _Process(double delta)
	{
		HorizontalInput = Input.GetAxis("move_left", "move_right");
	}

	public bool GetJumpPressed() => Input.IsActionJustPressed("move_jump");
	public bool GetJumpReleased() => Input.IsActionJustReleased("move_jump");
}
