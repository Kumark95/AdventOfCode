# Notes

## Part 1
- The first versions did not take into account the full state and only stored the position and direction, which skipped a lot of valid paths
- Also the initial implementation of the GetNeighbours returned the oposite direction again
- In the new version I also remeved the initialization of the cost dictionary as with the new state including the steps, it was no longer feasible

## Part 2
- Only needed a few tweaks to introduce a minimum number of steps before turning to the sides
