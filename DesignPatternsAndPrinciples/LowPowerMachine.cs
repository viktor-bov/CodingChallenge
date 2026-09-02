namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Base single-laser 200W machine.
    /// </summary>
    public class LowPowerMachine : AmMachine
    {
        public override string Description => "Low Power Machine (200W)";

        public override decimal Cost() => 450_000m;
    }
}
