# Notes

- The initial approach was to keep a hashset of the positions reached, but this does not scale for Part 2
- Drawing the map, we can see that each step, the garden plots (".") change its state from reached ("O") to not reached (".") and the other way around forming a diamond shape

Step 0: 1 plot (Starting position)
```
.....
.....
..O..
.....
.....
```

Step 1: 4 plot
```
.....
..O..
.O.O.
..O..
.....
```

Step 2: 9 plot
```
..O..
.O.O.
O.O.O
.O.O.
..O..
```

- To get the number of garden plots, without any rocks, we can use `(stepNumber + 1)^2`
- To get the actual number we need to remove the rocks present in each step
