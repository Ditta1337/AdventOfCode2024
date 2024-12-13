int: maxA;
int: maxB;
int: x1;
int: x2;
int: y1;
int: y2;
int: t1;
int: t2;

var int: a;
var int: b;

constraint a >= 0 /\ b >= 0;
constraint a * x1 + b * x2 == t1;
constraint a * y1 + b * y2 == t2;
constraint a <= maxA;
constraint b <= maxB;

solve :: int_search([a, b], first_fail, indomain_min) minimize a * 3 + b;

output [
    show(a), " ", show(b),
];