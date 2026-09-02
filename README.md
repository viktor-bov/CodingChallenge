# Coding Challenge

This coding challenge consists of two main projects:

1. **DesignPatternsAndPrinciples**
2. **AlloyOptimisation**

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **.NET SDK 10.0** (or newer patch in the 10.0 line)
- **Visual Studio 2026** (latest update recommended) with the .NET workload installed
- **Git**

## Getting Started

To get a local copy of the project up and running, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/viktor-bov/CodingChallenge.git
   cd CodingChallenge
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Build the solution:

   ```bash
   dotnet build
   ```

## Running the Projects

You can run each project from its project directory or by using the project file path.

### DesignPatternsAndPrinciples

To run the Design Patterns and Principles project, use the following command:

```bash
dotnet run --project ./DesignPatternsAndPrinciples/DesignPatternsAndPrinciples.csproj
```

### AlloyOptimisation

To run the Alloy Optimisation project, use the following command:

```bash
dotnet run --project ./AlloyOptimisation/AlloyOptimisation.csproj
```

> If your local folder structure differs, adjust the project paths accordingly.

## Running Tests

If test projects are included, you can run the tests with the following command:

```bash
dotnet test
```

## Notes

- The target framework for this workspace is **.NET 10**.
- For performance-sensitive benchmarking, use the `Release` configuration:

  ```bash
  dotnet build -c Release
  ```

## Troubleshooting

If you encounter issues, consider the following troubleshooting steps:

- If SDK errors occur, verify the installed SDKs:

  ```bash
  dotnet --list-sdks
  ```

- If restore or build fails after package changes, clean the solution and retry:

  ```bash
  dotnet clean
  dotnet restore
  dotnet build
  ```

## Contributing
If you would like to contribute to this project, please fork the repository and submit a pull request. We welcome contributions that improve the functionality and performance of the projects.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
