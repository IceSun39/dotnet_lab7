namespace QualityControl.Core;

public interface IScannable
{
    bool IsDefective();
}

public interface IDescribable
{
    string Describe();
}

public interface IDefectHandler
{
    void OnDefectDetected(object? sender, PartEventArgs e);
}

public interface IStatistics
{
    int Total { get; }
    int Defects { get; }
    void Reset();
}
