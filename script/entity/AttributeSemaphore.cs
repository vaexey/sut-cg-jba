public class AttributeSemaphore
{
    protected int Counter { get; set; } = 0;

    public AttributeSemaphore(int startValue = 0) => Counter = startValue;
    public AttributeSemaphore(bool startValue = false) : this(startValue ? 1 : 0) { }

    public static implicit operator bool(AttributeSemaphore sem) => sem.Counter > 0;
    public static implicit operator int(AttributeSemaphore sem) => sem.Counter;

    public static implicit operator AttributeSemaphore(int value) => new(value);
    public static implicit operator AttributeSemaphore(bool value) => new(value);

    // public void Add() => Counter++;
    // public void Remove() => Counter--;
}