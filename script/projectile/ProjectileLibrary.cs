using Godot;

public sealed class ProjectileLibrary
{
    public class PackedProjectile<T> where T : SimpleProjectile
    {
        private PackedScene Scene;

        public PackedProjectile(PackedScene scene)
        {
            Scene = scene;
        }

        public T Make()
        {
            return Scene.Instantiate<T>();
        }
    }

    private static PackedProjectile<T> CC<T>(string id) where T : SimpleProjectile
    {
        return new PackedProjectile<T>(
            ResourceLoader.Load<PackedScene>($"res://script/projectile/types/{id}.tscn")
        );
    }

    public static PackedProjectile<T> Get<T>() where T : SimpleProjectile
    {
        return CC<T>(typeof(T).Name);
    }
    
    public static PackedProjectile<SimpleProjectile> SimpleProjectile => Get<SimpleProjectile>();
    public static PackedProjectile<AutoJodlerProjectile> AutoJodlerProjectile => Get<AutoJodlerProjectile>();
    public static PackedProjectile<EggThrowProjectile> EggThrowProjectile => Get<EggThrowProjectile>();
}