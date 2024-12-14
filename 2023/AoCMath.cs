namespace AoC23;

public class AoCMath
{
    public static long LCM(List<long> numbers)
    {
        return numbers.Aggregate(LCM);
    }
    
    public static long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }

    // Use eudlicean algorithm to calculate gcd
    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}