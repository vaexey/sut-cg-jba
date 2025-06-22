using Godot;
using System;

public partial class InputHandler : Node2D
{
	[Export]
	public virtual float HorizontalInput { get; set; } = 0;

	[Export]
	public virtual Vector2 PointingAt { get; set; } = Vector2.Zero;

	public override void _Process(double delta)
	{
		HorizontalInput = Input.GetAxis("move_left", "move_right");
		PointingAt = GetGlobalMousePosition();
	}

	public virtual bool GetJumpPressed() => Input.IsActionJustPressed("move_jump");
	public virtual bool GetJumpReleased() => Input.IsActionJustReleased("move_jump");

	public virtual bool GetAbilityBasic() => Input.IsActionJustPressed("ability_basic");
	public virtual bool GetAbilityBasicPrevious() => Input.IsActionJustPressed("ability_basic_prev");
	public virtual bool GetAbilityBasicNext() => Input.IsActionJustPressed("ability_basic_next");
	public virtual bool GetAbilityComplex1() => Input.IsActionJustPressed("ability_complex1");
	public virtual bool GetAbilityComplex2() => Input.IsActionJustPressed("ability_complex2");
	public virtual bool GetAbilityComplex3() => Input.IsActionJustPressed("ability_complex3");
	public virtual bool GetAbilityGodlike() => Input.IsActionJustPressed("ability_godlike");

	public virtual int GetAbilityBasicIndex()
	{
		for (int i = 0; i < 10; i++)
		{
			if (Input.IsActionJustPressed($"ability_basic_{i + 1}"))
			{
				return i;
			}
		}

		return -1;
	}
}
