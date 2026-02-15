# CSharp Prompt Snippets

Prompts for intelligent C# code refactoring, along with their clickable calls. Hope these help accelerate your meaningful projects, happy building!

## Overview

This repo provides prompt templates designed to work with VS Code's Copilot integration. The primary focus is reducing complexity in C# methods through intelligent extraction and refactoring, resulting in cleaner, more maintainable code.

The project includes a sample ASP.NET Core 9.0 API with a higher complexity method, to try as reference and applying these techniques to your own projects.

## Quick Start

### Prerequisites

- .NET 9.0 SDK or later
- VS Code with Copilot extension
- Windows, macOS, or Linux

#### Quick Access
**Clickable links to use the prompt directly in VS Code Copilot Chat:**
```
[![Run in VS Code](https://img.shields.io/badge/VS_Code_%7C_promptu-Prompt_Name-0098FF?style=flat-square&logo=githubcopilot&logoColor=white)](https://vscode.dev/redirect?url=vscode://ms-promptu.promptu?prompt=gh:tinkeringDev/CSharpPromptSnippets/.github/prompts/refactor-method-complexity-reduce.prompt.md&input=methodName:Get,complexityThreshold:10)

[![Run in VS Code](https://img.shields.io/badge/VS_Code_Insiders_%7C_promptu-Prompt_Name-24bfa5?style=flat-square&logo=githubcopilot&logoColor=white)](https://vscode.dev/redirect?url=vscode-insiders://ms-promptu.promptu?prompt=gh:tinkeringDev/CSharpPromptSnippets/.github/prompts/refactor-method-complexity-reduce.prompt.md&input=methodName:Get,complexityThreshold:10)
```

### Running the Application

```bash
# Clone and navigate to the project
cd CSharpPromptSnippets

# Restore dependencies and run
dotnet run

# The API will be available at:
# HTTP:  http://localhost:5210
# HTTPS: https://localhost:7105
```

Access the Swagger UI at `http://localhost:5210/` to explore the weather forecast endpoint.

## Available Prompts

All prompts are located in `.github/prompts/` and can be used with VS Code Copilot. Each prompt includes detailed step-by-step instructions and validation procedures.

### Refactor Method to Reduce Cognitive Complexity

**File:** `refactor-method-complexity-reduce.prompt.md`

Intelligently extracts logic from complex methods into focused helper methods to reduce cognitive complexity.

#### Parameters
- `methodName` - The method to refactor (e.g., `Get`)
- `complexityThreshold` - Target complexity level (e.g., `10`)

#### Example Usage
```
/refactor-method-complexity-reduce methodName=Get, complexityThreshold=10
```

#### What It Does
- Analyzes nested conditionals and complex boolean expressions
- Extracts validation, transformation, and utility logic into separate methods
- Replaces if-else chains with cleaner patterns (switch expressions, guards)
- Generates meaningful, descriptive helper method names
- Preserves all original functionality and behavior

#### Key Features
- Evaluates cognitive complexity using SonarQube metrics
- Suggests making methods `static` when they don't need instance state
- Maintains proper encapsulation and access levels
- Extracts related logic into cohesive units
- Includes comprehensive post-refactoring validation steps

> **Note:** The `WeatherForecastController.Get()` method is a good demonstration case, starting with cognitive complexity of 34, to try out the prompt's refactoring capabilities.

## Project Structure

```
CSharpPromptSnippets/
├── Controllers/
│   └── WeatherForecastController.cs    # Sample API controller with high complexity
├── .github/prompts/                    # VS Code Copilot prompt templates
│   └── refactor-method-complexity-reduce.prompt.md
├── Properties/
│   └── launchSettings.json
├── Program.cs                          # Application entry point with Swagger
├── WeatherForecast.cs                  # Data model
├── CSharpPromptSnippets.csproj         # Project file (.NET 9.0)
├── appsettings.json                    # Configuration
└── README.md                           # This file
```

## Using Copilot Prompts

### Integration with VS Code

1. **Open a C# file** in VS Code where you want to apply the refactor prompt
2. **Reference the prompt** in Copilot Chat (e.g., "@workspace refactor-method-complexity-reduce")
3. **Fill in parameters** when prompted (e.g., methodName=Get, complexityThreshold=10)
4. **Review the suggestions** and apply changes incrementally

## Refactoring Example

### Before: Original Method (Cognitive Complexity: 34)
```csharp
public IEnumerable<WeatherForecast> Get()
{
    var forecasts = new List<WeatherForecast>();

    for (int i = 1; i <= 5; i++)
    {
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(i));
        var temperatureC = Random.Shared.Next(-20, 55);
        var summary = Summaries[Random.Shared.Next(Summaries.Length)];

        if (temperatureC < 0)
        {
            summary = "Freezing";
            if (date.Month == 12 || date.Month == 1)
            {
                summary += " - Winter";
            }
        }
        else if (temperatureC < 10)
        {
            summary = "Chilly";
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                summary += " - Start of the Week";
            }
        }
        else if (temperatureC > 30)
        {
            summary = "Hot";
            if (temperatureC > 40)
            {
                summary += " - Extreme Heat";
                if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    summary += " - Weekend Incoming";
                }
            }
        }
        // ... more nested conditions
    }
    // ... extreme temperature logging
    return forecasts;
}
```

### After: Refactored (Cognitive Complexity: 2-3)
```csharp
public IEnumerable<WeatherForecast> Get()
{
    var forecasts = new List<WeatherForecast>();

    for (int i = 1; i <= 5; i++)
    {
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(i));
        var temperatureC = Random.Shared.Next(-20, 55);
        var summary = BuildWeatherSummary(date, temperatureC);

        forecasts.Add(new WeatherForecast
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summary
        });
    }

    LogExtremeTemperatures(forecasts);
    return forecasts;
}

private static string BuildWeatherSummary(DateOnly date, int temperatureC)
{
    var summary = GetBaseSummary(temperatureC);
    summary = AddSeasonalSuffix(summary, date, temperatureC);
    summary = AddWeekdaySpecificSuffix(summary, date, temperatureC);
    summary = AddWeekendSuffix(summary, date);
    summary = AddHydrationWarning(summary, temperatureC);
    summary = AddSevereColdWarning(summary, date, temperatureC);
    return summary;
}

private static string GetBaseSummary(int temperatureC)
{
    return temperatureC switch
    {
        < 0 => "Freezing",
        < 10 => "Chilly",
        > 30 => "Hot",
        _ => Summaries[Random.Shared.Next(Summaries.Length)]
    };
}

// Additional helper methods extract each concern...
```

## API Reference

### Weather Forecast Endpoint

**GET** `/weatherforecast`

Returns a collection of weather forecasts for the next 5 days with dynamic summaries based on temperature and date.

**Response:**
```json
[
  {
    "date": "2026-02-16",
    "temperatureC": 8,
    "temperatureF": 46,
    "summary": "Chilly"
  },
  {
    "date": "2026-02-17",
    "temperatureC": -5,
    "temperatureF": 23,
    "summary": "Freezing - Winter"
  }
]
```

## Best Practices

- **Test incrementally** - Verify each refactoring change maintains original behavior
- **Run full test suites** - Execute all tests after applying prompt suggestions
- **Review generated code** - AI suggestions should always be validated before committing
- **Use static methods** - Mark helper methods `static` when they don't need instance state
- **Single responsibility** - Keep each extracted method focused on one concern
- **Meaningful names** - Use clear, intent-revealing names for helper methods

> **Note:** The validation checklist in each prompt ensures you verify compilation, test results (failed=0), and complexity metrics after refactoring.

## Architecture Notes

- **Framework:** ASP.NET Core 9.0 with Nullable Reference Types enabled
- **API Documentation:** Swagger (Swashbuckle) integration for interactive API exploration
- **Configuration:** Environment-specific settings via `appsettings.json` and `appsettings.Development.json`
- **Launch Profiles:** Both HTTP and HTTPS endpoints configured
- **Logging:** Built-in structured logging with categorized log levels

## Requirements

- .NET 9.0 SDK
- VS Code (with Copilot extension recommended)
- 100+ MB disk space for dependencies

## Resources

- [Microsoft: Code Metrics](https://docs.microsoft.com/en-us/visualstudio/code-quality/code-metrics-values)
- [Cognitive Complexity Analysis](https://www.sonarsource.com/blog/cognitive-complexity-because-testability-understandability/)
- [Roslyn Analyzers](https://github.com/dotnet/roslyn)
- [VS Code Copilot Documentation](https://docs.github.com/en/copilot/tutorials/customization-library/prompt-files/your-first-prompt-file)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)

## Related Files

- **LICENSE** - MIT License (see LICENSE file)
- **.gitignore** - Standard .NET project exclusions
- **CSharpPromptSnippets.csproj** - Project configuration and dependencies

---

Built with a focus on code quality automation and intelligent refactoring assistance through AI-powered prompt engineering.
