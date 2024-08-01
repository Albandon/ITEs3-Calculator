
using System.Collections.Specialized;

namespace CalculatorApp;
public class Integers {
    private readonly List<int> _number = [];
    private readonly bool _isComplement;

    public Integers() {
        _number.Add(0);
        GetBase = 10;
    }

    /// <summary>
    /// Constructs integer based on decimal number and converts it into specified one
    /// </summary>
    /// <param name="number">decimal representation</param>
    /// <param name="toBase">desired base to convert to</param>
    public Integers(long number, int toBase = 10) {
        _isComplement = number < 0; //if negative then complement base
        number = Math.Abs(number);
        while (number > 0) {
            _number.Add((int)(number % toBase));
            number /= toBase;
        }
        GetBase = toBase;
        _number.Add(0);
        if (_isComplement) _number = Neg()._number;
        // _number.Add(_isComplement? toBase - 1: 0);
        // if (_base == 10) return;
        // PadBinNumber();
    }

    public Integers(string number, int inBase, bool isComplement = false) {
        // validate if it's a number to max base 16
        // add interpretation of negative numbers
        // if (number.Last() >= inBase / 2) isComplement = true;   
        var dec = Convert.ToInt32(number, inBase);
        _isComplement = isComplement;
        while (dec > 0) {
            _number.Add(dec % inBase);
            dec /= inBase;
        }
        GetBase = inBase; 
        // if (_isComplement) _number = Neg()._number;
    }

    public Integers(IEnumerable<int> number, int toBase, bool isComplement = false) {
        _isComplement = isComplement;
        _number = number.ToList();
        GetBase = toBase;
        // if (isComplement && _Base != 2) _Number = Neg()._Number;
        // PadBinNumber();
    }
    
    public static Integers operator+ (Integers a, Integers b) { // introduce custom Exception
        if (a.GetBase != b.GetBase) throw new ArgumentException("Different Bases");
        var memory = 0;
        var product = new List<int>();
        var numBase = a.GetBase;
        for (int i = 0; i < Math.Max(a._number.Count,b._number.Count); i++) {
            var result = a._number.ElementAtOrDefault(i) + b._number.ElementAtOrDefault(i) + memory;
            product.Add(result % numBase);
            memory = result / numBase;
        }
        // var overflow = Mod(memory, numBase);
        // if (overflow > 0) product.Add(overflow);
        return new Integers(product,numBase);
    }

    public static Integers operator -(Integers a, Integers b) {
        if (a.GetBase != b.GetBase) throw new ArgumentException("Different Bases");
        var memory = 0;
        var product = new List<int>();
        var numBase = a.GetBase;
        var negative = false;
        for (int i = 0; i < Math.Max(a.GetNumberLength, b.GetNumberLength); i++) {
            var result =  a._number.ElementAtOrDefault(i) - b._number.ElementAtOrDefault(i) - memory;
            memory = result < 0 ? 1 : 0;
            product.Add(Mod(result,numBase));
        }
        if (memory == 1) negative = true;
        return new Integers(PopZeros(product), numBase, negative);
    }

    public static Integers operator *(Integers a, Integers b) { // works (changed some logic)
        if (a.GetBase != b.GetBase) throw new ArgumentException("Different Bases");
        var cA = new List<int>(a._number);
        var cB = new List<int>(b._number); 
        var countA = a.GetNumberLength;
        var countB = b.GetNumberLength;
        for (int i = 0; i < countA; i++) {
            cA.Add(a._isComplement? a.GetBase - 1: 0);
        }
        for (int i = 0; i < countB; i++) {
            cB.Add(b._isComplement? b.GetBase - 1: 0);
        }
        var memory = 0;
        var product = new int[cA.Count+cB.Count];
        var negative = a._isComplement != b._isComplement;
        for (int i = 0; i < cB.Count; i++) {
            for (int j = 0; j < cA.Count; j++) {
                var result = product[i + j] + cA.ElementAtOrDefault(j) 
                             * cB.ElementAtOrDefault(i) + memory;
                memory = result / a.GetBase;
                product[i + j] = result % a.GetBase;
            }
        }

        product = product[..^(countA+countB+1)];
        return new Integers(PopZeros(product), a.GetBase, negative);
    }
    public override string ToString() {
        char[] numSymbol = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
        var temp = _number;
        var str = "";
        if (GetBase == 10 && _isComplement) {
            temp = Neg()._number;
            str = "-";
        }
        if (GetBase != 10) str += _isComplement ? numSymbol[GetBase - 1] : numSymbol[0];
        str += PadString(str); // not ideal but works
        for (int i = temp.Count - 1; i >= 0; i--) {
            if (GetBase == 2 && (i + 1) % 4 == 0 ) str += " "; 
            var curSymbol = temp[i];
            str += numSymbol[curSymbol];
        }
        return str;
    }
    private Integers Neg() =>
        MaxValue(GetBase, GetNumberLength) - this + new Integers(1,GetBase);

    public Integers ConvertTo(int toBase) { // conversion from negative isn't ideal
        long dec = 0;
        var neg = _isComplement?Neg()._number : _number;
        for (int i = 0; i < neg.Count; i++) {
            dec += neg[i] * (int)Math.Pow(GetBase, i);
        }
        if (_isComplement) dec *= -1;
        return new Integers(dec, toBase); 
    }
    
    private static Integers MaxValue(int @base, int length) {
        var temp = new int[length];
        Array.Fill(temp,@base-1);
        return new Integers(temp, @base);
    }

    public static Integers operator ^(Integers a, Integers b) {
        if (a.GetBase != b.GetBase) throw new ArgumentException("Different Base");
        var cA = a.ConvertTo(2);
        var cB = b.ConvertTo(2);
        var count = Math.Max(cA._number.Count, cB._number.Count);
        var temp = new int [count];
        for (int i = 0; i < count; i++) {
            temp[i] = cA._number.ElementAtOrDefault(i) ^ cB._number.ElementAtOrDefault(i);
        }
        return new Integers(temp, cA.GetBase).ConvertTo(a.GetBase);
    }
    public int GetNumberLength => _number.Count;
    public int GetBase { get; }
    
    private string PadString(string s) {
        char[] numSymbol = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
        var str = "";
        if (GetBase != 2) return "";
        var count = s.Length % 4;
        if (count == 0) return "";
        for (int i = 0; i < 4 - count; i++) {
            str += _isComplement ? numSymbol[GetBase - 1] : numSymbol[0];
        }
        return str;
    }
    private static int[] PopZeros(IEnumerable<int> a) {
        var temp = a.ToArray();
        while (temp[^1]==0 && temp.Length>1) {
            temp = temp[..^1];
        }
        return temp;
    }

    private static int Mod(int a, int b) =>
        (a % b + b) % b;
}