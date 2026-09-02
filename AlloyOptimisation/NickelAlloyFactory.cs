using System.Text.Json;
using AlloyOptimisation.Data;
using AlloyOptimisationUtility.Models;

namespace AlloyOptimisation
{
    /// <summary>
    /// Builds the nickel-based <see cref="AlloySystem"/> to be optimised. Creep coefficients
    /// and concentration constraints are material-physics data resolved from configuration
    /// (currently a hardcoded JSON string, later Azure App Configuration or a database), while
    /// the element cost coefficients are external market data resolved through an
    /// <see cref="IElementCostRepository"/> (currently a mock, later a real database).
    /// </summary>
    public sealed class NickelAlloyFactory
    {
        public const double MaximumCost = 18.0;

        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        /// <summary>
        /// Material-physics parameters (creep coefficient and concentration constraints) per
        /// element symbol, stored as JSON. Kept as a property so the source can later be
        /// switched to Azure App Configuration or a database without changing the factory.
        /// </summary>
        private static string ElementParametersJson => """
            {
                "Cr": { "creepCoefficient": 2.0911350E+16, "minimum": 14.5, "maximum": 22.0, "step": 0.50 },
                "Co": { "creepCoefficient": 7.2380280E+16, "minimum": 0.0,  "maximum": 25.0, "step": 1.00 },
                "Nb": { "creepCoefficient": 1.0352738E+16, "minimum": 0.0,  "maximum": 1.5,  "step": 0.10 },
                "Mo": { "creepCoefficient": 8.9124547E+16, "minimum": 1.5,  "maximum": 6.0,  "step": 0.50 }
            }
            """;

        private readonly IElementCostRepository _costRepository;

        public NickelAlloyFactory(IElementCostRepository costRepository)
        {
            _costRepository = costRepository ?? throw new ArgumentNullException(nameof(costRepository));
        }

        public AlloySystem CreateSystem()
        {
            IReadOnlyDictionary<ElementSymbol, ElementParameters> parameters = LoadElementParameters();

            // Nickel is the base element and therefore has no creep coefficient or constraint.
            var nickel = CreateElement(ElementSymbol.Ni);

            var constraints = new List<ElementConstraint>(parameters.Count);
            foreach ((ElementSymbol symbol, ElementParameters parameter) in parameters)
            {
                var element = CreateElement(symbol, parameter.CreepCoefficient);
                constraints.Add(new ElementConstraint(element, parameter.Minimum, parameter.Maximum, parameter.Step));
            }

            return new AlloySystem(nickel, constraints.ToArray());
        }

        private static IReadOnlyDictionary<ElementSymbol, ElementParameters> LoadElementParameters()
        {
            var parametersBySymbol = JsonSerializer.Deserialize<Dictionary<ElementSymbol, ElementParameters>>(
                ElementParametersJson, JsonOptions);

            if (parametersBySymbol is null || parametersBySymbol.Count == 0)
            {
                throw new InvalidOperationException("Element parameters configuration is empty or invalid.");
            }

            return parametersBySymbol;
        }

        private Element CreateElement(ElementSymbol symbol, double? creepCoefficient = null)
        {
            double cost = _costRepository.GetCostPerKilogram(symbol);
            return new Element(symbol, cost, creepCoefficient);
        }
    }
}
