// See https://aka.ms/new-console-template for more information

using CalculatorApp;

Console.WriteLine("Hello, World!");
var a = new Integers(10, 10);
var b = new Integers(11, 10);
Console.WriteLine(a);
Console.WriteLine(b);
var c = a - b;
// Console.WriteLine(a);
// Console.WriteLine(b);
// c.Neg();
Console.WriteLine(c);
Console.WriteLine(c + b);
// Console.WriteLine(new Integers(10, 2) - new Integers(9,2));
// Console.WriteLine((new Integers("1011",2)+new Integers("0011",2)).ToString());
// Console.WriteLine((new Integers(211,10 ) + new Integers(49,10)).ToString());