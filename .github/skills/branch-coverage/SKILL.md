---
name: branch-coverage
description: Reference guide for identifying branches in C# code and measuring branch coverage with coverlet/cobertura. Use when planning tests or measuring coverage.
---

# Branch Coverage Reference

## What Counts as a Branch

Every decision point creates two or more branches that must be tested:

| Construct | Branches | Test requirement |
|-----------|----------|-----------------|
| `if (cond)` without `else` | 2 (true + false) | Test both when condition is true AND when false (fall-through) |
| `if / else` | 2 | Test both paths |
| `if / else if / else` | N+1 | Test each condition + the final else |
| `switch` | N cases + default | Test each case, including `default` |
| `?.` (null-conditional) | 2 (null + non-null) | Test with null AND non-null receiver |
| `??` (null-coalescing) | 2 (null + non-null) | Test when left side is null AND when it has a value |
| `cond ? a : b` (ternary) | 2 | Test both outcomes |
| `&&` / `||` (short-circuit) | 2+ (short-circuit + full evaluation) | Test cases that trigger short-circuit AND full evaluation |
| `try/catch` | 2 (normal + exception) | Test success path AND the exception path |
| `foreach` / `while` / `for` | 2 (zero iterations + one-or-more iterations) | Test with empty collection AND non-empty collection |

## How to Measure

Run coverage after implementation and refactoring:

```bash
dotnet test <TestProject> \
  --collect:"XPlat Code Coverage" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
```

Find the coverage file:
```bash
# Windows (PowerShell)
Get-ChildItem -Recurse -Filter coverage.cobertura.xml | Select-Object FullName

# Linux/macOS
find . -name "coverage.cobertura.xml" -path "*/TestResults/*"
```

To generate a human-readable report (if `reportgenerator` is installed):
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool 2>/dev/null
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:TextSummary
cat coveragereport/Summary.txt
```

Parse the cobertura XML for branch coverage: look for `branch-rate` attribute on the target class/package. Multiply by 100 for percentage.
