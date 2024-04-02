using System.Numerics;
using System.Text;

Console.Write("Iveskite p: ");
BigInteger p = Convert.ToInt32(Console.ReadLine());

Console.Write("Iveskite q: ");
BigInteger q = Convert.ToInt32(Console.ReadLine());

bool value = true;
while (value)
{
    if (IsPrime(p) && IsPrime(q))
    {
        value = false;
    }
    else if (IsPrime(p) == false)
    {
        Console.WriteLine("p yra ne pirminis skaicius");
        Console.Write("Iveskite is naujo p: ");
        p = Convert.ToInt32(Console.ReadLine());
    }
    else if (IsPrime(q) == false)
    {
        Console.WriteLine("q yra ne pirminis skaicius");
        Console.Write("Iveskite is naujo q: ");
        q = Convert.ToInt32(Console.ReadLine());
    }
    Console.WriteLine("Ivesti skaiciai tinka");
} 

Console.Write("Iveskite teksta: ");
string x = Console.ReadLine();

BigInteger n = p * q;
BigInteger fi = (p - 1) * (q - 1);

BigInteger e = fE(fi);
BigInteger d = fD(e, fi);

BigInteger[] encrypted = Encrypt(x, n, e);

string decrypted = Decrypt(encrypted, n, d);

Console.WriteLine("-----------------------------");
Console.WriteLine("Originalus tekstas: " + x);
Console.WriteLine("Uzsifruotas tekstas: " + string.Join(",", encrypted));
Console.WriteLine("Desifruotas tekstas: " + decrypted);

BigInteger fE(BigInteger fi)
{
    for (BigInteger e = 2; e < fi; e++)
    {
        if (BigInteger.GreatestCommonDivisor(e, fi) == 1)
        {
            return e;
        }
    }
    throw new Exception("Klaida");
}

BigInteger fD(BigInteger e, BigInteger fi)
{
    BigInteger d = BigInteger.One;

    while ((d * e) % fi != BigInteger.One)
    {
        d++;
    }

    return d;
}

BigInteger[] Encrypt(string text, BigInteger n, BigInteger e)
{
    byte[] bytes = Encoding.Unicode.GetBytes(text);
    BigInteger[] encrypted = new BigInteger[bytes.Length];

    for (int i = 0; i < bytes.Length; i++)
    {
        encrypted[i] = BigInteger.ModPow(bytes[i], (int)e, n);
    }

    return encrypted;
}
string Decrypt(BigInteger[] encrypted, BigInteger n, BigInteger d)
{
    byte[] decryptedBytes = new byte[encrypted.Length];

    for (int i = 0; i < encrypted.Length; i++)
    {
        decryptedBytes[i] = (byte)BigInteger.ModPow(encrypted[i], (int)d, n);
    }

    return Encoding.Unicode.GetString(decryptedBytes);
}
static bool IsPrime(BigInteger number)
{
    if (number <= 1)
        return false;
    if (number == 2)
        return true;
    if (number % 2 == 0)
        return false;

    for (BigInteger i = 3; BigInteger.Pow(i, 2) <= number; i += 2)
    {
        if (number % i == 0)
            return false;
    }
    return true;
}