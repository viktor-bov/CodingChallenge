using DesignPatternsAndPrinciples;

namespace DesignPatternsAndPrinciplesTests
{
    [Trait("Category", "Unit")]
    public class AmMachineTests
    {
        [Fact]
        public void Factory_CreatesExpectedBaseMachineCosts()
        {
            Assert.Equal(450_000m, AmMachineFactory.Create(MachinePower.Low).Cost());
            Assert.Equal(550_000m, AmMachineFactory.Create(MachinePower.Medium).Cost());
            Assert.Equal(750_000m, AmMachineFactory.Create(MachinePower.High).Cost());
        }

        [Fact]
        public void MediumMachine_WithReducedVolume_QuadLaser_PowderRecirculation_Costs932000()
        {
            // Arrange - Medium Power + Reduced Build Volume + Quad Laser + Powder Recirculation
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();
            AmMachine machine = AmMachineFactory.Create(
                MachinePower.Medium,
                catalog,
                "reduced-build-volume", "quad-laser", "powder-recirculation");

            // Act
            var totalCost = machine.Cost();

            // Assert
            Assert.Equal(932_000m, totalCost);
        }

        [Fact]
        public void ComposedMachine_DescriptionReflectsSelectedFeatures()
        {
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();
            AmMachine machine = AmMachineFactory.Create(
                MachinePower.Medium,
                catalog,
                "reduced-build-volume", "quad-laser", "powder-recirculation");

            var description = machine.Description;

            Assert.Contains("Medium Power Machine", description);
            Assert.Contains("Reduced Build Volume", description);
            Assert.Contains("Quad Laser", description);
            Assert.Contains("Powder Recirculation System", description);
        }

        [Fact]
        public void HighMachine_WithThermalCamera_AndPhotodiodes_Costs867000()
        {
            IFeatureCatalog catalog = new InMemoryFeatureCatalog();
            AmMachine machine = AmMachineFactory.Create(
                MachinePower.High,
                catalog,
                "thermal-imaging-camera", "photodiodes");

            Assert.Equal(867_000m, machine.Cost());
        }
    }
}
