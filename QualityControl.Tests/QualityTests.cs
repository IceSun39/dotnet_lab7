using Microsoft.VisualStudio.TestTools.UnitTesting;
using QualityControl.Core;

namespace QualityControl.Tests;

[TestClass]
public class PartTests
{
    [TestMethod]
    public void Gear_InTolerance_IsNotDefective()
    {
        var gear = new Gear("G", 50, 0.1, 20) { Measured = 50.05 };
        Assert.IsFalse(gear.IsDefective());
    }

    [TestMethod]
    public void Gear_OutOfTolerance_IsDefective()
    {
        var gear = new Gear("G", 50, 0.1, 20) { Measured = 50.5 };
        Assert.IsTrue(gear.IsDefective());
    }

    [TestMethod]
    public void LongShaft_HasStricterTolerance()
    {
        var shortShaft = new Shaft("S", 20, 0.1, 100) { Measured = 20.07 };
        var longShaft = new Shaft("S", 20, 0.1, 600) { Measured = 20.07 };
        Assert.IsFalse(shortShaft.IsDefective());
        Assert.IsTrue(longShaft.IsDefective());
    }
}

[TestClass]
public class EventTests
{
    private static Gear Bad() => new("bad", 50, 0.1, 20) { Measured = 51 };
    private static Gear Good() => new("good", 50, 0.1, 20);

    [TestMethod]
    public void DefectDetected_ReachesAllSubscribers()
    {
        var scanner = new QualityScanner();
        var arm = new RoboticArm();
        var control = new ControlSystem();
        scanner.DefectDetected += arm.OnDefectDetected;
        scanner.DefectDetected += control.OnDefectDetected;

        scanner.Scan(Bad());

        Assert.AreEqual(1, arm.Removed.Count);
        Assert.AreEqual(1, control.Defects);
    }

    [TestMethod]
    public void GoodPart_DoesNotRaiseDefectDetected()
    {
        var scanner = new QualityScanner();
        int count = 0;
        scanner.DefectDetected += (s, e) => count++;

        scanner.Scan(Good());

        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public void ScanStarted_ComesBeforeScanCompleted()
    {
        var scanner = new QualityScanner();
        var order = new List<string>();
        scanner.ScanStarted += (s, e) => order.Add("started");
        scanner.ScanCompleted += (s, e) => order.Add("completed");

        scanner.Scan(Good());

        CollectionAssert.AreEqual(new[] { "started", "completed" }, order);
    }

    [TestMethod]
    public void ScanCompleted_ReachesAllSubscribers()
    {
        var scanner = new QualityScanner();
        var control = new ControlSystem();
        int counter = 0;
        scanner.ScanCompleted += control.OnScanCompleted;
        scanner.ScanCompleted += (s, e) => counter++;

        scanner.Scan(Good());
        scanner.Scan(Bad());

        Assert.AreEqual(2, control.Total);
        Assert.AreEqual(2, counter);
    }

    [TestMethod]
    public void DefectLimitExceeded_RaisedOnceForAllSubscribers()
    {
        var scanner = new QualityScanner(2);
        int a = 0, b = 0;
        scanner.DefectLimitExceeded += (s, e) => a++;
        scanner.DefectLimitExceeded += (s, e) => b++;

        scanner.Scan(Bad());
        scanner.Scan(Bad());
        scanner.Scan(Bad());

        Assert.AreEqual(1, a);
        Assert.AreEqual(1, b);
    }

    [TestMethod]
    public void Unsubscribed_HandlerGetsNothing()
    {
        var scanner = new QualityScanner();
        var arm = new RoboticArm();
        scanner.DefectDetected += arm.OnDefectDetected;
        scanner.DefectDetected -= arm.OnDefectDetected;

        scanner.Scan(Bad());

        Assert.AreEqual(0, arm.Removed.Count);
    }

    [TestMethod]
    public void Scan_Null_Throws()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new QualityScanner().Scan(null!));
    }
}

[TestClass]
public class ControlSystemTests
{
    [TestMethod]
    public void Pipeline_HasTwoMethods_AndRunsBoth()
    {
        var control = new ControlSystem();
        Assert.AreEqual(2, control.Pipeline!.GetInvocationList().Length);

        control.Pipeline(new Gear("G", 50, 0.1, 20));

        Assert.AreEqual(1, control.Defects);
        Assert.AreEqual(1, control.Journal.Count);
    }

    [TestMethod]
    public void Pipeline_MethodCanBeRemoved()
    {
        var control = new ControlSystem();
        control.Pipeline -= control.WriteJournal;

        control.Pipeline!(new Gear("G", 50, 0.1, 20));

        Assert.AreEqual(1, control.Defects);
        Assert.AreEqual(0, control.Journal.Count);
    }

    [TestMethod]
    public void ControlSystem_ImplementsThreeInterfaces()
    {
        var control = new ControlSystem();
        Assert.IsInstanceOfType<IDefectHandler>(control);
        Assert.IsInstanceOfType<IStatistics>(control);
        Assert.IsInstanceOfType<IDescribable>(control);
    }
}
