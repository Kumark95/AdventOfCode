# TODO

1. Optimize Part 2
    - Currently it uses brute-force and runs for 4m before returning the result
    - Ranges do not overlap
    - Ranges are mostly contiguous, in some cases the gap is > 1
    - WIP: Collapse the seed ranges to destination ranges instead of individual values
