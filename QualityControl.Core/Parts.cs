namespace QualityControl.Core;

public abstract class Part : IScannable, IDescribable
{
    public string Name { get; }
    public double Nominal { get; }
    public double Tolerance { get; }
    public double Measured { get; set; }

    protected Part(string name, double nominal, double tolerance)
    {
        Name = name;
        Nominal = nominal;
        Tolerance = tolerance;
        Measured = nominal;
    }

    public virtual bool IsDefective() => Math.Abs(Measured - Nominal) > Tolerance;

    public virtual string Describe() => $"{Name}: {Measured:F2} мм";
}

public class Gear : Part
{
    public int Teeth { get; }

    public Gear(string name, double nominal, double tolerance, int teeth)
        : base(name, nominal, tolerance)
    {
        Teeth = teeth;
    }
}

public class Shaft : Part
{
    public double Length { get; }

    public Shaft(string name, double nominal, double tolerance, double length)
        : base(name, nominal, tolerance)
    {
        Length = length;
    }

    public override bool IsDefective()
    {
        double limit = Length > 500 ? Tolerance / 2 : Tolerance;
        return Math.Abs(Measured - Nominal) > limit;
    }
}
