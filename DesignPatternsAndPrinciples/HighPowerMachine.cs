namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Base single-laser 500W machine.
    /// </summary>
    public class HighPowerMachine : AmMachine
    {
        public override string Description => "High Power Machine (500W)";

        public override decimal Cost() => 750_000m;
    }
}
