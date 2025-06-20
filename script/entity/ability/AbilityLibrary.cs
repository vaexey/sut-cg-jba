using System.Collections.Generic;
using Godot;

public sealed class AbilityLibrary
{
    public class PackedAbility
    {
        protected PackedScene Scene;

        public PackedAbility(PackedScene scene)
        {
            Scene = scene;
        }

        public T Make<T>() where T : Ability
        {
            return Scene.Instantiate<T>();
        }
    }

    public class PackedAbility<T> : PackedAbility where T : Ability
    {
        public PackedAbility(PackedScene scene) : base(scene)
        {}

        public T Make()
        {
            return Scene.Instantiate<T>();
        }
    }

    private static PackedAbility<T> CC<T>(string id) where T : Ability
    {
        return new PackedAbility<T>(
            ResourceLoader.Load<PackedScene>($"res://script/entity/ability/types/{id}.tscn")
        );
    }

    public static PackedAbility<T> Get<T>() where T : Ability
    {
        return CC<T>(typeof(T).Name);
    }

    public static PackedAbility<AutoJodlerAbility> AutoJodlerAbility => Get<AutoJodlerAbility>();
    public static PackedAbility<EggThrowAbility> EggThrowAbility => Get<EggThrowAbility>();

    public static IEnumerable<PackedAbility> All
    {
        get
        {
            yield return AutoJodlerAbility;
            yield return EggThrowAbility;
        }
    }
}