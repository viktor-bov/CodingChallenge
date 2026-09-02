namespace DesignPatternsAndPrinciples
{
    public abstract class AmMachine
    {
        public abstract string Description { get; }
        public abstract decimal Cost();
    }
}
