namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Base single-laser 400W machine.
    /// </summary>
    public class MediumPowerMachine : AmMachine
    {
        public override string Description => "Medium Power Machine (400W)";

        public override decimal Cost() => 550_000m;
    }
}
