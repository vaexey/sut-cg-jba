using System;

public class AttributeValue
{
    protected Func<double> GetMax;
    
    public double Percentage { get; set; }

    public AttributeValue(Func<double> max, double def = 1.0)
    {
        GetMax = max;
        Percentage = def;
    }

    public AttributeValue(double max = 1.0, double def = 1.0)
        : this(max: () => max, def: def) {}

    public double Max {
        get {
            return GetMax();
        }
    }

    public double Value {
        get {
            return Percentage * Max;
        }
        set {
            Percentage = value / Max;
        }
    }

    public static implicit operator double(AttributeValue attr)
    {
        return attr.Value;
    }

    public override string ToString()
    {
        return Value + "";
    }
}