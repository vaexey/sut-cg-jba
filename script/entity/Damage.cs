public class Damage
{
    public Entity Source;
    public double FlatValue = 0;
    public double PercentageValue = 0;
    public DamageFlags Flags = 0;
    public CrowdControlEffect[] ApplyCC = [];

    public Damage(Entity source, double flat = 0)
    {
        Source = source;
        FlatValue = flat;
    }
}