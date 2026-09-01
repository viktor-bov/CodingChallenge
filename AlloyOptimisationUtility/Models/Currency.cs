namespace AlloyOptimisationUtility.Models
{
    public sealed class Currency
    {
        public Currency(string code, string symbol)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        public string Code { get; }

        public string Symbol { get; }

        public static Currency Gbp { get; } = new Currency("GBP", "£");

        public override string ToString() => Code;
    }
}
