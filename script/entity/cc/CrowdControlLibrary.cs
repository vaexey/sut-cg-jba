using Godot;

public sealed class CrowdControlLibrary
{
    public class PackedCrowdControl<T> where T : CrowdControlEffect
    {
        private PackedScene Scene;

        public PackedCrowdControl(PackedScene scene)
        {
            Scene = scene;
        }

        public T Make()
        {
            return Scene.Instantiate<T>();
        }
    }

    private static PackedCrowdControl<T> CC<T>(string id) where T : CrowdControlEffect
    {
        return new PackedCrowdControl<T>(
            ResourceLoader.Load<PackedScene>($"res://script/entity/cc/effects/{id}.tscn")
        );
    }

    public static PackedCrowdControl<T> Get<T>() where T : CrowdControlEffect
    {
        return CC<T>(typeof(T).Name);
    }

    public static PackedCrowdControl<HikingInstinctCC> HikingInstinct => Get<HikingInstinctCC>();
    public static PackedCrowdControl<WaterInPantsCC> WaterInPants => Get<WaterInPantsCC>();
    public static PackedCrowdControl<AppenzellerCC> Appenzeller => Get<AppenzellerCC>();
}