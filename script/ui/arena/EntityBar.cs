using Godot;
using System;
using System.Linq;

public partial class EntityBar : Control
{
    [ExportSubgroup("Nodes")]
    [Export] public Entity Entity { get; set; }

    [Export] public Label Title { get; set; }
    [Export] public Label RIP { get; set; }
    [Export] public Label Silenced { get; set; }
    [Export] public Label Crippled { get; set; }
    [Export] public Label Slowed { get; set; }
    [Export] public Control CastingGroup { get; set; }
    [Export] public ProgressBar CastingBar { get; set; }

    public override void _Process(double delta)
    {
        CastingGroup.Visible = Entity.IsCasting;

        if (Entity.IsCasting)
        {
            var ability = Entity.Abilities.All.Where(a => a.IsCasting).First();

            CastingBar.Value = 1 - ability.CastTimeLeft / ability.CastTime;
        }

        Silenced.Visible = Entity.IsAlive && Entity.IsSilenced;
        Crippled.Visible = Entity.IsAlive && (Entity.IsCrippledHorizontally || Entity.IsCrippledVertically);
        Slowed.Visible = Entity.IsAlive && (Entity.PassiveAttributes.Speed(Entity.Stamina.Value) < Entity.PassiveAttributes.BaseSpeed);

        RIP.Visible = !Entity.IsAlive;

        // Title.Text = $"{((Player)Entity.Parent2D).Id}";
        var id = ((Player)Entity.Parent2D).Id;

        Title.Text = id == Multiplayer.GetUniqueId()
            ? "You"
            : "Enemy";
    }
}
