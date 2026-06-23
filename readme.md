# Saxonica SaxonCS-HE 13 RNG repro

## Summary
This repository reproduces an issue in SaxonCS-HE 13 where `random-number-generator()` appears to produce different
results across runs when seeded with `xs:dateTime`, even when the seed value is fixed.

## Expected behavior
Using the same seed value should produce the same random sequence across separate executions.

## Actual behavior
When the seed is provided as `xs:dateTime`, the generated value changes between runs. When the same timestamp is
provided as a string seed, the generated value remains stable between runs. This means that the same logical timestamp
value leads to different results depending on whether it is passed as `xs:dateTime` or as a string.

## Setup
- .NET 10.0
- SaxonCS-HE 13.0
- OS: Windows 11 25H2 x64

## Reproduction
1. Run `dotnet run`.
2. Note the generated values.
3. Run `dotnet run` again.
4. Compare the outputs.

## Test setup details
The XSLT document contains three variants:
1. `current-dateTime()` as seed (pinned by the host application)
2. constant `xs:dateTime` as seed
3. `string(current-dateTime())` as seed

To eliminate runtime clock differences, the current date/time is pinned using
`Saxon.Hej.Controller.setCurrentDateTime()`.

All variants effectively use the same timestamp value: `2025-02-16T09:35:47Z`

## Sample output
The values for `random-constDateTime` and `random-currentDateTime` change across runs, while `random-stringDateTime`
remains stable.

### Run 1
```
random-currentDateTime 1:	59300
random-currentDateTime 2:	59300
random-currentDateTime 3:	59300
random-constDateTime 1:		59300
random-constDateTime 2:		59300
random-constDateTime 3:		59300
random-stringDateTime 1:	59894
random-stringDateTime 2:	59894
random-stringDateTime 3:	59894
```

### Run 2
```
random-currentDateTime 1:	52993
random-currentDateTime 2:	52993
random-currentDateTime 3:	52993
random-constDateTime 1:		52993
random-constDateTime 2:		52993
random-constDateTime 3:		52993
random-stringDateTime 1:	59894
random-stringDateTime 2:	59894
random-stringDateTime 3:	59894
```

## Additional context
For comparison, the same scenario previously behaved deterministically in SaxonHE10Net31Api by Martin Honnen. After
migrating the project to .NET 10.0, all tests were still passing. The changed behavior appeared only after switching to
the official Saxon 13 library.

I understand that this older library is unrelated to SaxonCS and that its behavior is not necessarily intended here.
However, since the RNG is documented as deterministic, and since a string seed produces stable values across runs, I
would expect a constant `xs:dateTime` seed to behave consistently as well.
