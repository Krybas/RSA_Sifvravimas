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
        Console.WriteLine("Ivesti skaiciai tinka");
        Console.WriteLine("-----------------------------");
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

}


Console.Write("Iveskite teksta: ");
string x = Console.ReadLine();

BigInteger n = p * q;
BigInteger fi = (p - 1) * (q - 1);

BigInteger e = fE(fi);
BigInteger d = fD(e, fi);

Console.WriteLine("Ar norite issaugoti faile arba nuskaityti is failo? (Taip/Ne)");
string choice = Console.ReadLine();
switch (choice.ToLower())
{
    case "taip":

        Console.WriteLine("Pasirinkimas taip");
        Console.WriteLine("-----------------------------");

        Console.WriteLine("Ar norite irasyti ir nuskaityti ar tik nuskaityti? (Nuskaityti/irasyti)");
        string choice2 = Console.ReadLine();

        switch (choice2.ToLower())
        {
            case "nuskaityti":
                Console.WriteLine("Iveskite failo pavadinima: ");
                string Fname2 = Console.ReadLine();

                var result = ReadFromFile(Fname2);
                BigInteger[] encryptedTextFromFile = result.Item1;
                BigInteger nFromFile = result.Item2;

                string decryptedText = Decrypt(encryptedTextFromFile, nFromFile, d);
                Console.WriteLine("Desifruotas tekstas: " + decryptedText);
                Console.WriteLine("Failas yra nuskaitytas");
                Console.WriteLine("-----------------------------");
                break;
            case "irasyti":

                Console.WriteLine("Iveskite failo pavadinima: ");
                string Fname1 = Console.ReadLine();
                BigInteger[] encryptedF = Encrypt(x, n, e);
                WriteToFile(Fname1, encryptedF, n);
                Console.WriteLine("Failas yra irasytas");
                Console.WriteLine("-----------------------------");

                var r = ReadFromFile(Fname1);
                BigInteger[] TextFromFile = r.Item1;
                BigInteger nFile = r.Item2;

                string decrypted = Decrypt(TextFromFile, nFile, d);
                Console.WriteLine("Desifruotas tekstas: " + decrypted);
                break;
            default:
                Console.WriteLine("Nera tokio pasirinkimo");
                break;
        }
        break;
    case "ne":

        Console.WriteLine("Pasirinkimas ne");
        Console.WriteLine("-----------------------------");
        ;
        BigInteger[] encryptedC = Encrypt(x, n, e);
        Console.WriteLine("Uzsifruotas tekstas: " + string.Join(",", encryptedC));

        string decryptedC = Decrypt(encryptedC, n, d);
        Console.WriteLine("Desifruotas tekstas: " + decryptedC);
        break;

    default:
        Console.WriteLine("Nera tokio pasirinkimo");
        break;
}


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
    BigInteger fi0 = fi;
    BigInteger y = 0, x = 1;

    if (fi == 1)
        return 0;

    while (e > 1)
    {
        BigInteger q = e / fi;
        BigInteger t = fi;

        fi = e % fi;
        e = t;
        t = y;

        y = x - q * y;
        x = t;
    }

    if (x < 0)
        x += fi0;

    return x;
}
static BigInteger[] Encrypt(string text, BigInteger n, BigInteger e)
{
    byte[] bytes = Encoding.Unicode.GetBytes(text);
    BigInteger[] encrypted = new BigInteger[bytes.Length];

    for (int i = 0; i < bytes.Length; i++)
    {
        BigInteger m = new BigInteger(bytes[i]);
        encrypted[i] = BigInteger.ModPow(m, e, n);
    }

    return encrypted;
}

static string Decrypt(BigInteger[] encrypted, BigInteger n, BigInteger d)
{
    byte[] decryptedBytes = new byte[encrypted.Length];

    for (int i = 0; i < encrypted.Length; i++)
    {
        BigInteger decryptedBigInt = BigInteger.ModPow(encrypted[i], d, n);
        decryptedBytes[i] = (byte)decryptedBigInt;
    }

    return Encoding.Unicode.GetString(decryptedBytes);
}
bool IsPrime(BigInteger number)
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

static void WriteToFile(string filename, BigInteger[] encrypted, BigInteger n)
{
    string file = filename + ".txt";
    using (StreamWriter writer = new StreamWriter(file))
    {
        foreach (BigInteger num in encrypted)
        {
            writer.WriteLine(num.ToString());
        }
        writer.WriteLine(n.ToString());
    }
}

static Tuple<BigInteger[], BigInteger> ReadFromFile(string filename)
{
    string file = filename + ".txt";
    string[] lines = File.ReadAllLines(file);
    BigInteger[] encrypted = new BigInteger[lines.Length - 1];

    for (int i = 0; i < lines.Length - 1; i++)
    {
        encrypted[i] = BigInteger.Parse(lines[i]);
    }

    BigInteger nFromFile = BigInteger.Parse(lines[lines.Length - 1]);

    return Tuple.Create(encrypted, nFromFile);
}