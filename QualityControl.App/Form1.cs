using QualityControl.Core;

namespace QualityControl.App;

public partial class Form1 : Form
{
    private readonly QualityScanner _scanner = new(3);
    private readonly RoboticArm _arm = new();
    private readonly ControlSystem _control = new();

    public Form1()
    {
        InitializeComponent();

        _scanner.DefectDetected += _arm.OnDefectDetected;
        _scanner.DefectDetected += _control.OnDefectDetected;
        _scanner.DefectDetected += (s, e) => Log($"Брак: {e.Part.Describe()}");

        _scanner.ScanCompleted += _control.OnScanCompleted;
        _scanner.ScanCompleted += (s, e) => RefreshStats();

        _scanner.ScanStarted += (s, e) => Log($"Сканування: {e.Part.Describe()}");

        _scanner.DefectLimitExceeded += (s, e) => Log("Ліміт браку перевищено!");
        _scanner.DefectLimitExceeded += (s, e) => lblStats.ForeColor = Color.Firebrick;

        RefreshStats();
    }

    private void btnScan_Click(object? sender, EventArgs e)
    {
        var part = new Gear("Шестерня", 50, 0.1, 24) { Measured = (double)nudSize.Value };
        _scanner.Scan(part);
    }

    private void btnReset_Click(object? sender, EventArgs e)
    {
        _scanner.Reset();
        _control.Reset();
        lstLog.Items.Clear();
        lblStats.ForeColor = SystemColors.ControlText;
        RefreshStats();
    }

    private void Log(string text) => lstLog.Items.Add(text);

    private void RefreshStats() =>
        lblStats.Text = $"{_control.Describe()}, прибрано рукою: {_arm.Removed.Count}";
}
