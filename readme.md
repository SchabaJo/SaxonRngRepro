# Saxonica SaxonCS-HE 13 RNG repro

This shows that the XSL `random-number-generator` in SaxonCS-HE 13 is not deterministic between runs it is is seeded with `xs:dateTime`.

The XSLT document contains 3 functions to generate random numbers (integer with 5 digits):

1. `random-currentDateTime` which uses `current-dateTime()` as seed
2. `random-constDateTime` which uses a constant `xs:dateTime` as seed
3. `random-stringDateTime` which uses `string(current-dateTime())` as seed

The current DateTime is pinned using `Saxon.Hej.Controller.setCurrentDateTime()`.

All functions essentially use the same timestamp: `2025-02-16T09:35:47Z`.

During the same execution, the RNG is perfectly deterministic:

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

On the next run, the results are different (except for `random-stringDateTime`):

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
