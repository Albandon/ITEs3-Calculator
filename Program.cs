// See https://aka.ms/new-console-template for more information

using CalculatorApp;

Console.WriteLine("Hello, World!");

// var a = new Integers(10, 2);
// var b = new Integers(11, 2);
// Console.WriteLine(a);
// Console.WriteLine(b);
// var c = a - b;
// Console.WriteLine(c.ConvertTo(16)); //conversion from u2 to dec and u8 == bad
// Console.WriteLine(c + b);
// var d = new Integers("A109", 16);
// Console.WriteLine(d);
// Console.WriteLine(d.ConvertTo(10));
// Console.WriteLine(d.ConvertTo(8));
// Console.WriteLine(d.ConvertTo(2));
// // Console.WriteLine(new Integers(-121311,16));
// var v = new Integers(-19, 10);
// Console.WriteLine(v);
Console.WriteLine(new Integers(12512)*new Integers(3));
Console.WriteLine(new Integers(12512,16)*new Integers(3,16));
Console.WriteLine(new Integers(12512,8)*new Integers(3,8));
Console.WriteLine(new Integers(12512,2)*new Integers(3,2));
