namespace QualityControl.Core;

public delegate void DefectProcessing(Part part);

public class PartEventArgs : EventArgs
{
    public Part Part { get; }

    public PartEventArgs(Part part) => Part = part;
}

public class QualityScanner
{
    public int MaxDefects { get; }
    public int Scanned { get; private set; }
    public int Defects { get; private set; }

    public event EventHandler<PartEventArgs>? ScanStarted;
    public event EventHandler<PartEventArgs>? ScanCompleted;
    public event EventHandler<PartEventArgs>? DefectDetected;
    public event EventHandler? DefectLimitExceeded;

    public QualityScanner(int maxDefects = 3) => MaxDefects = maxDefects;

    public bool Scan(Part part)
    {
        ArgumentNullException.ThrowIfNull(part);
        var args = new PartEventArgs(part);

        ScanStarted?.Invoke(this, args);
        Scanned++;

        bool defective = part.IsDefective();
        if (defective)
        {
            Defects++;
            DefectDetected?.Invoke(this, args);
        }

        ScanCompleted?.Invoke(this, args);

        if (defective && Defects == MaxDefects)
            DefectLimitExceeded?.Invoke(this, EventArgs.Empty);

        return defective;
    }

    public void Reset()
    {
        Scanned = 0;
        Defects = 0;
    }
}
