using Godot;

// Required: Area2D action bound to OnAreaCollisionStart
public partial class WallbangProjectile : SimpleProjectile
{
    public virtual void OnAreaCollisionStart(Node2D collision)
    {
        if (!Multiplayer.IsServer()) return;

        OnCollisionRaw([collision]);
    }
    
}