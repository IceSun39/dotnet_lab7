namespace QualityControl.Core;

public class RoboticArm : IDefectHandler
{
    private readonly List<Part> _removed = new();

    public IReadOnlyList<Part> Removed => _removed;

    public void OnDefectDetected(object? sender, PartEventArgs e) => _removed.Add(e.Part);
}

public class ControlSystem : IDefectHandler, IStatistics, IDescribable
{
    private readonly List<string> _journal = new();

    public DefectProcessing? Pipeline { get; set; }
    public int Total { get; private set; }
    public int Defects { get; private set; }
    public IReadOnlyList<string> Journal => _journal;

    public ControlSystem()
    {
        Pipeline += CountDefect;
        Pipeline += WriteJournal;
    }

    public void CountDefect(Part part) => Defects++;

    public void WriteJournal(Part part) => _journal.Add($"Брак: {part.Describe()}");

    public void OnDefectDetected(object? sender, PartEventArgs e) => Pipeline?.Invoke(e.Part);

    public void OnScanCompleted(object? sender, PartEventArgs e) => Total++;

    public void Reset()
    {
        Total = 0;
        Defects = 0;
        _journal.Clear();
    }

    public string Describe() => $"Перевірено: {Total}, браку: {Defects}";
}
