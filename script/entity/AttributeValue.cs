using System;
using Godot;

public class AttributeValue
{
    protected Func<double> GetMax;
    protected Func<double> GetMin;
    
    // Main variable
    public double Percentage {
        get {
            return _percentage;
        }
        set {
            if(AllowOutOfRange)
            {
                _percentage = value;
            } else {
                _percentage = Mathf.Clamp(value, 0, 1);
            }
        }
    }
    private double _percentage;

    // Value change over 1 second
    public double Regen { get; set; }

    // Value change over 1 second when out of range
    public double RegenOutOfRange { get; set; }

    // If value can be set out of range
    public bool AllowOutOfRange { get; set; }

    public AttributeValue(Func<double> min, Func<double> max, double def = 1.0, double regen = 0.0, double regenOutOfRange = 0.0, bool allowOutOfRange = false)
    {
        GetMin = min;
        GetMax = max;
        Percentage = def;
        Regen = regen;
        RegenOutOfRange = regenOutOfRange;
        AllowOutOfRange = allowOutOfRange;
    }

    public AttributeValue(Func<double> max, double def = 1.0, double regen = 0.0, double regenOutOfRange = 0.0, bool allowOutOfRange = false)
        : this(() => 0, max, def, regen, regenOutOfRange, allowOutOfRange) {}

    public AttributeValue(double min = 0.0, double max = 1.0, double def = 1.0, double regen = 0.0, double regenOutOfRange = 0.0, bool allowOutOfRange = false)
        : this(() => min, () => max, def, regen, regenOutOfRange, allowOutOfRange) {}

    
    // TODO: Make min value functional
    public double Min {
        get {
            return GetMin();
        }
    }

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

    public bool IsOutOfRange {
        get {
            return Percentage < 0 || Percentage > 1;
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

    public void Process(double delta)
    {
        double regen = Regen;

        if(IsOutOfRange)
            regen = RegenOutOfRange;
        
        // TODO: handle OOR better, eg. calculating two deltas
        // based on already applied value when one movetoward caps
        if(regen > 0)
        {
            Value = Mathf.MoveToward(
                Value, 
                Max, 
                regen * delta
			);
        } else {
            Value = Mathf.MoveToward(
                Value, 
                Min, 
                -regen * delta
			);
        }
    }
}