using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace CalculatorApp;
public class Integers {
    private readonly List<int> _number = [];
    private readonly int _base;
    private readonly bool _isComplement = false;

    public Integers() {
        _number.Add(0);
        _base = 10;
    }

    /// <summary>
    /// Constructs integer based on decimal number and converts it into specified one
    /// </summary>
    /// <param name="number">decimal representation</param>
    /// <param name="toBase">desired base to convert to</param>
    public Integers(int number, int toBase) {
        _isComplement = number < 0; //if negative then complement base
        number = Math.Abs(number);
        while (number > 0) {
            _number.Add(number % toBase);
            number /= toBase;
        }
        _base = toBase;
        if (_base == 10) return;
        _number.Add(_isComplement? toBase - 1: 0);
        if (_isComplement) _number = Neg()._number;
        PadBinNumber();
    }

    public Integers(string number, int inBase, bool isComplement = false) {
        // validate if it's a number to max base 16
        // add interpretation of negative numbers
        _isComplement = isComplement;
        _base = inBase;
        for (int i = number.Length - 1; i >= 0; i--) {
            _number.Add(number[i] % inBase);
        }
        // if (_IsComplement) _Number = Neg()._Number;
    }

    public Integers(IEnumerable<int> number, int toBase, bool isComplement = false) {
        _isComplement = isComplement;
        _number = number.ToList();
        _base = toBase;
        // if (isComplement && _Base != 2) _Number = Neg()._Number;
        PadBinNumber();
    }
    
    public static Integers operator+ (Integers a, Integers b) { // introduce custom Exception
        if (a._base != b._base) throw new ArgumentException("Different Bases");
        var memory = 0;
        var product = new List<int>();
        var Base = a._base;
        for (int i = 0; i < Math.Max(a._number.Count,b._number.Count); i++) {
            var result = a._number.ElementAtOrDefault(i) + b._number.ElementAtOrDefault(i) + memory;
            product.Add(result % Base);
            memory = result / Base;
        }
        return new Integers(product,Base);
    }

    public static Integers operator -(Integers a, Integers b) { //problem with how we define default for negatives -> we don't xD
        // possible solution -> separate class for number containment with get method which would give you Element At or Default 
        // BASICALLY Class (or Record tbh) with List<int> Number and a few methods
        if (a._base != b._base) throw new ArgumentException("Different Bases");
        var memory = 0;
        var product = new List<int>();
        var Base = a._base;
        var negative = a._isComplement && b._isComplement;
        for (int i = 0; i < Math.Max(a.GetNumberLength, b.GetNumberLength); i++) {
            var result =  a._number.ElementAtOrDefault(i) - b._number.ElementAtOrDefault(i) - memory;
            memory = result < 0 ? 1 : 0;
            product.Add(Mod(result,Base));
        }
        if (memory == 1) negative = true;
        while (product[^1] == 0 && product.Count>1) {
            product = product[..^1]; //delete last element == pop xD
        }
        return new Integers(product, Base, negative);
    }
    public override string ToString() {
        char[] numSymbol = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
        var temp = _number;
        var str = "";
        if (_base == 10 && _isComplement) {
            temp = Neg()._number;
            str = "-";
        }
        for (int i = temp.Count - 1; i >= 0; i--) {
            if (_base == 2 && (i + 1) % 4 == 0 && (i + 1) != temp.Count) str += " "; 
            var curSymbol = temp[i];
            str += numSymbol[curSymbol];

        }
        return str;
    }
    
    public Integers Neg() =>
        MaxValue(GetBase, GetNumberLength) - this + new Integers(1,GetBase);
    
    private static Integers MaxValue(int @base, int length) {
        var temp = new int[length];
        Array.Fill(temp,@base-1);
        return new Integers(temp, @base);
    }

    public static Integers operator ^(Integers a, Integers b) {
        if (a._base != b._base) throw new ArgumentException("Different Base");
        var count = Math.Max(a._number.Count, b._number.Count);
        var temp = new int [count];
        for (int i = 0; i < count; i++) {
            temp[i] = a._number.ElementAtOrDefault(i) ^ b._number.ElementAtOrDefault(i);
        }
        return new Integers(temp, a._base);
    }
    public int GetNumberLength => _number.Count;
    public int GetBase => _base;

    private void PadBinNumber() {
        if (_base != 2) return;
        var count = _number.Count % 4;
        if (count == 0) return;
        for (int i = 0; i < 4 - count; i++) {
            _number.Add(_isComplement? _base - 1: 0);
        }
    }

    private static int Mod(int a, int b) =>
        (a % b + b) % b;
}