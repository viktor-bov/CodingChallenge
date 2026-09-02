# AM Machine Design Patterns & Principles

This document accompanies the code in the `DesignPatternsAndPrinciples` project.

---

## Part 1A — Creational Pattern Analysis

The existing model is three simple, stateless base machines (`LowPowerMachine`,
`MediumPowerMachine`, `HighPowerMachine`) that each return a fixed cost and description.

| Pattern | Applicability | Pros | Cons | Limitations | Recommendation |
|---------|---------------|------|------|-------------|----------------|
| **Factory Method** | Centralise creation of the concrete machines behind a single selector returning `AmMachine`. | Decouples callers from concrete constructors; single place to add new machine types; simple and idiomatic. | Slight indirection; an extra type. | Adds little value if objects are always created directly and never vary. | **Good fit** — small, low-cost, matches the "select a base machine" requirement. Implemented here. |
| **Abstract Factory** | Provide families of related products (e.g. machine + tooling + calibration kit per region/product line). | Great when several related products must be created consistently as a family. | Heavyweight for a single product type; many interfaces. | Overkill when there is only one product (the machine). | **Poor fit** — there are no product *families* here. |
| **Builder** | Assemble a machine step-by-step, especially once optional features exist. | Excellent for complex, multi-part configuration; readable fluent API. | Unnecessary ceremony for three fixed machines with no assembly steps. | Best when construction has many optional steps/ordering. | **Partial fit** — not needed for the base machines; the Decorator (Part 2) already handles optional composition. A builder could wrap it later. |
| **Prototype** | Clone a pre-configured machine instead of constructing anew. | Useful when object creation is expensive or configuration is copied often. | Machines are cheap, stateless value-like objects; cloning adds no benefit. | Needs meaningful copy semantics to be worthwhile. | **Poor fit** — creation is trivial. |
| **Singleton** | Ensure a single shared instance of a machine/factory. | Good for a single shared, stateless service. | Machines are distinct purchasable products, not a single shared instance; hinders testability if overused. | Only for genuinely single-instance resources. | **Poor fit** for machines. The factory here is a static class (a pragmatic single access point) rather than a stateful singleton. |

**Recommendation:** Use the **Factory Method** for creating base machines — it is the only
creational pattern that genuinely reduces coupling here without adding unnecessary complexity.

---

## Part 1B — Implemented Creational Pattern

`AmMachineFactory.Create(MachinePower power)` returns the correct `AmMachine`
concrete type. Callers depend only on the `AmMachine` abstraction and a simple enum.

---

## Part 2A — Why Inheritance Fails for Optional Features

Modelling each machine + feature combination as a subclass causes:

- **Class explosion** — with 3 base machines and 5 optional features, every subset would
  require its own class (3 × 2^5 = 96 classes), growing exponentially with each new feature.
- **Difficult maintenance** — a price change to one feature must be edited in many subclasses.
- **Tight coupling** — feature logic is baked into machine types, so features cannot vary
  independently of machines.
- **Hard to extend** — adding one feature roughly doubles the number of required subclasses.
- **Principle violations** — breaks the Open/Closed Principle and favours inheritance over
  composition.

---

## Part 2B — Selected Pattern: Decorator

The **Decorator** pattern solves the optional-feature problem. Each optional feature is a
decorator that both *wraps* and *is an* `AmMachine`, delegating to the wrapped instance and
adding its own cost/description. Features can therefore be composed dynamically in any
combination at runtime.

New features are added by creating one new decorator class — no existing code changes and
no combinatorial subclasses.

### Class diagram

```
				 AmMachine (abstract)
				 ├─ Description
				 └─ Cost()
					 ▲
		┌────────────┼───────────────┐
		│            │               │
 LowPowerMachine MediumPowerMachine HighPowerMachine
		│
		│            MachineDecorator (abstract, wraps AmMachine)
		└───────────────────▲
							│
		  ┌─────────┬───────┼──────────────┬───────────────┐
	   QuadLaser  Reduced  Powder      Thermal          Photodiodes
				  Build    Recirculation Imaging
				  Volume   System      Camera

Composition example:
  PowderRecirculationSystem( QuadLaser( ReducedBuildVolume( MediumPowerMachine ) ) )
```

---

## Part 2C — Two Design Principles Demonstrated

1. **Open/Closed Principle (OCP)** — Classes are open for extension but closed for
   modification. New optional features are added by writing a new `MachineDecorator`
   subclass; no existing machine or feature class is modified.

2. **Favour Composition over Inheritance** — Behaviour and cost are built by *composing*
   decorators around a machine at runtime instead of encoding every configuration in a
   deep inheritance hierarchy. This keeps features independent and reusable across all
   machine types. (This also supports the **Single Responsibility Principle**: each
   decorator is responsible for exactly one feature's cost and description.)

---

## Part 2D — Runtime-Configurable Features (Data-Driven Catalog)

**Requirement:** features must be added, removed, or re-priced *without recompiling* the
code, sourcing the list of available features and their current cost at runtime (e.g. from
JSON or a database).

The original decorators (`QuadLaser`, `Photodiodes`, ...) hardcode their name and cost in a
compiled class, so any change requires a code change and rebuild. To remove that constraint
a small **data-driven layer** was added on top of the existing Decorator pattern. The typed
decorators are left untouched, so all previous behaviour and tests remain valid.

### New building blocks (in `Features/`)

| Type | Responsibility |
|------|----------------|
| `FeatureDefinition` (record) | Data-only description of a feature: `Key`, `Name`, `Cost`. Separates feature *data* from *behaviour*. |
| `IFeatureCatalog` (interface) | Abstraction that supplies all available features at runtime (`GetAll`) and resolves one by key (`TryGet`). **The extension seam** to swap for JSON/DB/config. |
| `InMemoryFeatureCatalog` | Mock `IFeatureCatalog` backed by a hardcoded list; also accepts an injected `IEnumerable<FeatureDefinition>` so a real source only needs to produce the list. |
| `FeatureDecorator` | A single **generic** `MachineDecorator` driven by a `FeatureDefinition`, so new features need no new subclass — they are pure data. |

### Factory extension

A new overload composes a machine from feature keys resolved through the catalog:

```csharp
AmMachine Create(MachinePower power, IFeatureCatalog catalog, params IEnumerable<string> featureKeys)
```

### Usage

```csharp
IFeatureCatalog catalog = new InMemoryFeatureCatalog(); // later: JsonFeatureCatalog / SqlFeatureCatalog

foreach (var f in catalog.GetAll())
    Console.WriteLine($"{f.Name}: {f.Cost:C}");

AmMachine machine = AmMachineFactory.Create(
    MachinePower.Medium,
    catalog,
    "reduced-build-volume", "quad-laser", "powder-recirculation");
```

### Design principles reinforced

- **Dependency Inversion Principle (DIP)** — callers depend only on the `IFeatureCatalog`
  abstraction, so the concrete data source (mock, JSON, DB, config service) can be swapped
  with no changes to machine/decorator code and **no recompilation**.
- **Open/Closed Principle** — a new data source is a new `IFeatureCatalog` implementation;
  existing code is not modified.
- **Single Responsibility** — `FeatureDefinition` holds data, `IFeatureCatalog` supplies it,
  `FeatureDecorator` applies it. Registering `IFeatureCatalog` in a DI container is the
  natural next step so the source is selected by configuration.

### Updated class diagram (feature layer)

```
  IFeatureCatalog ──► FeatureDefinition (Key, Name, Cost)
        ▲                      │
        │                      │ drives
 InMemoryFeatureCatalog        ▼
 (JSON / DB / config …)  FeatureDecorator : MachineDecorator
```

---

## Part 2E — Required Test

`MediumMachine_WithReducedVolume_QuadLaser_PowderRecirculation_Costs932000` asserts:

```
Medium Power Machine       £550,000
Reduced Build Volume        £75,000
Quad Laser                 £225,000
Powder Recirculation        £82,000
						   --------
Total                      £932,000
```
