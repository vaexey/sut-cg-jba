using Godot;
using System;

public partial class Player : Area2D
{
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;


	private bool _jump = false;
	private double _jumpRemaining = 0;

    public override void _Ready()
    {
        base._Ready();

		ScreenSize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		var velocity = Vector2.Zero;	

		if(Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if(Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		velocity = velocity.Normalized() * Speed;

		if(!_jump && Position.Y >= ScreenSize.Y)
		{
			if(Input.IsActionPressed("move_jump"))
			{
				_jump = true;
				_jumpRemaining = 0.2;
			}
		}

		// gravity
		velocity.Y += 800;

		Position += velocity * (float)delta;

		if(_jump)
		{
			var jumpDelta = delta;

			if(_jumpRemaining - delta < 0)
			{
				jumpDelta = _jumpRemaining;
				_jump = false;
			}

			_jumpRemaining -= delta;

			Position += new Vector2(
				x: 0,
				y: -2000
			) * (float)jumpDelta;
		}

		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
    }
}
