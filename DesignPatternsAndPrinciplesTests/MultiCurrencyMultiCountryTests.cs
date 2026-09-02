using DesignPatternsAndPrinciples;
using DesignPatternsAndPrinciples.Markets;
using DesignPatternsAndPrinciples.Pricing;

namespace DesignPatternsAndPrinciplesTests
{
    [Trait("Category", "Unit")]
    public class MultiCurrencyMultiCountryTests
    {
        [Fact]
        public void Quote_ConvertsGbpBasePriceIntoCountryDefaultCurrency()
        {
            IBaseMachineCatalog baseMachines = new InMemoryBaseMachineCatalog();
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();
            ICurrencyConverter converter = new InMemoryCurrencyConverter();

            AmMachine machine = AmMachineFactory.CreateForCountry(
                "medium-power-machine",
                baseMachines,
                catalog,
                Country.UnitedStates,
                "reduced-build-volume", "quad-laser", "powder-recirculation");

            MachineQuote quote = AmMachineFactory.Quote(machine, Country.UnitedStates, converter);

            // 932,000 GBP * 1.27 = 1,183,640 USD
            Assert.Equal(Currency.Gbp, quote.BasePrice.Currency);
            Assert.Equal(932_000m, quote.BasePrice.Amount);
            Assert.Equal(Currency.Usd, quote.LocalPrice.Currency);
            Assert.Equal(1_183_640m, quote.LocalPrice.Amount);
        }

        [Fact]
        public void Quote_InGbpCountry_ReturnsUnchangedBasePrice()
        {
            IBaseMachineCatalog baseMachines = new InMemoryBaseMachineCatalog();
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();
            ICurrencyConverter converter = new InMemoryCurrencyConverter();

            AmMachine machine = AmMachineFactory.CreateForCountry(
                "high-power-machine", baseMachines, catalog, Country.UnitedKingdom);

            MachineQuote quote = AmMachineFactory.Quote(machine, Country.UnitedKingdom, converter);

            Assert.Equal(Currency.Gbp, quote.LocalPrice.Currency);
            Assert.Equal(750_000m, quote.LocalPrice.Amount);
        }

        [Fact]
        public void Quote_HonoursExplicitQuoteCurrencyOverCountryDefault()
        {
            IBaseMachineCatalog baseMachines = new InMemoryBaseMachineCatalog();
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();
            ICurrencyConverter converter = new InMemoryCurrencyConverter();

            AmMachine machine = AmMachineFactory.CreateForCountry(
                "low-power-machine", baseMachines, catalog, Country.Germany);

            MachineQuote quote = AmMachineFactory.Quote(machine, Country.Germany, converter, Currency.Eur);

            // 450,000 GBP * 1.17 = 526,500 EUR
            Assert.Equal(Currency.Eur, quote.LocalPrice.Currency);
            Assert.Equal(526_500m, quote.LocalPrice.Amount);
        }

        [Fact]
        public void CreateForCountry_RejectsFeatureNotAvailableInMarket()
        {
            IBaseMachineCatalog baseMachines = new InMemoryBaseMachineCatalog();
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();

            // "high-power-export-pack" is restricted to the United States.
            var ex = Assert.Throws<ArgumentException>(() =>
                AmMachineFactory.CreateForCountry(
                    "high-power-machine", baseMachines, catalog, Country.Germany, "high-power-export-pack"));

            Assert.Contains("not available", ex.Message);
        }

        [Fact]
        public void CreateForCountry_AllowsCountryRestrictedFeatureInItsMarket()
        {
            IBaseMachineCatalog baseMachines = new InMemoryBaseMachineCatalog();
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();

            AmMachine machine = AmMachineFactory.CreateForCountry(
                "high-power-machine", baseMachines, catalog, Country.UnitedStates, "high-power-export-pack");

            // 750,000 + 48,000
            Assert.Equal(798_000m, machine.Cost());
        }

        [Fact]
        public void GetAvailableIn_ExcludesFeaturesRestrictedToOtherMarkets()
        {
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();

            var german = catalog.GetAvailableIn(Country.Germany);
            var american = catalog.GetAvailableIn(Country.UnitedStates);

            Assert.DoesNotContain(german, f => f.Key == "high-power-export-pack");
            Assert.Contains(american, f => f.Key == "high-power-export-pack");
        }
    }
}
