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

	public bool GetAbilityBasic() => Input.IsActionJustPressed("ability_basic");
	public bool GetAbilityBasicPrevious() => Input.IsActionJustPressed("ability_basic_prev");
	public bool GetAbilityBasicNext() => Input.IsActionJustPressed("ability_basic_next");
	public bool GetAbilityComplex1() => Input.IsActionJustPressed("ability_complex1");
	public bool GetAbilityComplex2() => Input.IsActionJustPressed("ability_complex2");
	public bool GetAbilityComplex3() => Input.IsActionJustPressed("ability_complex3");
	public bool GetAbilityGodlike() => Input.IsActionJustPressed("ability_godlike");

	public int GetAbilityBasicIndex()
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
