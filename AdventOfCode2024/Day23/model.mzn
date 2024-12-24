int: n;
array[1..n, 1..n] of bool: edges;

array[1..n] of var 0..1: x;

constraint forall(i in 1..n, j in 1..n where i < j) (
    not edges[i, j] -> x[i] + x[j] <= 1
);

var int: objective = sum(i in 1..n)(x[i]);

solve :: int_search(x, first_fail, dom_w_deg) maximize objective;

output [
    show(objective), "|", show([i | i in 1..n where fix(x[i]) = 1]),
];