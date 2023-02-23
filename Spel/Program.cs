namespace Main;

public class Program
{
    public static char[,] bord = new char[7, 7];
    public static Stapel<char[,]> stapelBord = new Stapel<char[,]>();
    public static char beurt = 'H';
    public static bool tegenComputer = false;

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("(1) Tegen andere speler \n(2) Tegen computer");
            var input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Ongeldige input");
                continue;
            }
            if (input.Equals("1"))
            {
                SpelTegenbeurt();
                break;
            }
            if (input.Equals("2"))
            {
                tegenComputer = true;
                SpelTegenComputer();
                break;
            }
        }
    }

    private static void SpelTegenComputer()
    {
        SetStartOpstelling();
        PrintBord();

        while (!IsSpelAfgelopen())
        {
            if (beurt == 'B')
            {
                Console.WriteLine($"Beurt: {beurt} (computer)");

                List<Tuple<int, int>> eigenPosities = new List<Tuple<int, int>>();

                for (int i = 0; i < bord.GetLength(0); i++)
                {
                    for (int j = 0; j < bord.GetLength(1); j++)
                    {
                        var waarde = bord[i, j];
                        if (waarde == 'B')
                        {
                            eigenPosities.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }

                Tuple<int, int, int, int, int> besteZet = null;

                foreach (var eigenPositie in eigenPosities)
                {

                    var mogelijkeZettenVoorPositie = KrijgMogelijkeZetten(eigenPositie.Item1, eigenPositie.Item2);
                    foreach (var mogelijkeZet in mogelijkeZettenVoorPositie)
                    {
                        int aantalMogelijkeInfecties = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (i == 0 && j == 0) continue;
                                if (IsPosBuitenSpel(mogelijkeZet.Item1 + i, mogelijkeZet.Item2 + j)) continue;
                                if (bord[mogelijkeZet.Item1 + i, mogelijkeZet.Item2 + j] == 'H')
                                {
                                    aantalMogelijkeInfecties++;
                                }
                            }
                        }

                        if (besteZet == null || aantalMogelijkeInfecties > besteZet.Item5)
                        {
                            besteZet = new Tuple<int, int, int, int, int>(eigenPositie.Item1, eigenPositie.Item2, mogelijkeZet.Item1, mogelijkeZet.Item2, aantalMogelijkeInfecties);
                        }
                    }
                }

                if (besteZet == null)
                {
                    Console.WriteLine("Kan geen zet doen");
                    beurt = (beurt == 'H') ? 'B' : 'H';
                    continue;
                }

                if (DoeZet(besteZet.Item1, besteZet.Item2, besteZet.Item3, besteZet.Item4))
                {
                    PrintBord();
                }
                else
                {
                    PrintBord();
                    Console.WriteLine("Ongeldige zet");
                    beurt = (beurt == 'H') ? 'B' : 'H';
                    continue;
                }

                beurt = (beurt == 'H') ? 'B' : 'H';
                continue;
            }

            if (DoeSpelerBeurt()) beurt = (beurt == 'H') ? 'B' : 'H';
        }
    }

    private static void SpelTegenbeurt()
    {
        SetStartOpstelling();
        PrintBord();

        var beurt = 'H';
        while (!IsSpelAfgelopen())
        {
            if (DoeSpelerBeurt()) beurt = (beurt == 'H') ? 'B' : 'H';
        }
    }

    private static bool DoeSpelerBeurt()
    {
        Console.WriteLine($"Beurt: {beurt}");
        Console.WriteLine("Van welke positie wil je een zet doen? x,y of typ undo om de laatste zet ongedaan te maken");
        KrijgLocatieInput(out var vanY, out var vanX);

        if (vanY == -1 && vanX == -1)
        {
            var oudBord = stapelBord.Pak();
            Console.WriteLine(tegenComputer);
            if (tegenComputer) oudBord = stapelBord.Pak();
            if (oudBord == default)
            {
                PrintBord();
                Console.WriteLine("Kan niet undo doen, er is nog geen zet gedaan");
                return false;
            }
            bord = oudBord;
            PrintBord();
            beurt = (beurt == 'H') ? 'B' : 'H';
            return false;
        }

        if (IsPosBuitenSpel(vanY, vanX))
        {
            PrintBord();
            Console.WriteLine("Ongeldige zet");
            return false;
        }

        Console.WriteLine("Naar welke positie wil je een zet doen? (x, y)");
        KrijgLocatieInput(out var naarY, out var naarX);

        if (DoeZet(vanY, vanX, naarY, naarX))
        {
            PrintBord();
        }
        else
        {
            PrintBord();
            Console.WriteLine("Ongeldige zet");
            return false;
        }
        return true;
    }

    private static bool KrijgLocatieInput(out int x, out int y)
    {
        string input = Console.ReadLine();

        if (input == null)
        {
            Console.WriteLine("Ongeldige input");
            x = 0;
            y = 0;
            return false;
        }

        if (input.ToLower() == "undo")
        {
            x = -1;
            y = -1;
            return true;
        }

        if (input == null || !input.Contains(","))
        {
            Console.WriteLine("Ongeldige input");
            x = 0;
            y = 0;
            return false;
        }

        string[] parts = input.Split(',');
        if (parts.Length != 2 || !int.TryParse(parts[0], out y) || !int.TryParse(parts[1], out x))
        {
            Console.WriteLine("Ongeldige input");
            x = 0;
            y = 0;
            return false;
        }

        return true;
    }


    public static void SetStartOpstelling()
    {
        for (int i = 0; i < bord.GetLength(0); i++)
        {
            for (int j = 0; j < bord.GetLength(1); j++)
            {
                bord[i, j] = default;
            }
        }

        bord[0, 5] = 'B';
        bord[0, 6] = 'B';
        bord[1, 5] = 'B';
        bord[1, 6] = 'B';

        bord[5, 0] = 'H';
        bord[6, 0] = 'H';
        bord[5, 1] = 'H';
        bord[6, 1] = 'H';

        stapelBord.Duw((char[,])bord.Clone());
    }

    public static bool IsSpelAfgelopen()
    {
        var aantalB = 0;
        var aantalH = 0;

        for (int i = 0; i < bord.GetLength(0); i++)
        {
            for (int j = 0; j < bord.GetLength(1); j++)
            {
                var waarde = bord[i, j];
                if (waarde == 'B') aantalB++;
                if (waarde == 'H') aantalH++;
            }
        }

        if (aantalB != 0 || aantalH != 0) return false;

        if (aantalH == 0) Console.WriteLine("BaggySweater wint!");
        if (aantalB == 0) Console.WriteLine("Hoodie wint!");

        return true;
    }

    public static void PrintBord()
    {
        Console.Write("\n ");
        for (int i = 0; i < bord.GetLength(1); i++)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + i + " ");
        }
        Console.Write("x \n");
        for (int i = 0; i < bord.GetLength(0); i++)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(i);
            for (int j = 0; j < bord.GetLength(1); j++)
            {
                if (bord[i, j] == default)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("   ");
                }
                else if (bord[i, j] == 'B')
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" B ");
                }
                else if (bord[i, j] == 'H')
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" H ");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\n");
        }

        Console.WriteLine("y");
    }

    public static bool DoeZet(int vanY, int vanX, int naarY, int naarX)
    {
        if (IsPosBuitenSpel(vanY, vanX)) return false;
        if (IsPosBuitenSpel(naarY, naarX)) return false;

        if (bord[vanY, vanX] != beurt) return false;
        if (bord[naarY, naarX] == beurt) return false;

        var mogelijkeZetten = KrijgMogelijkeZetten(vanY, vanX);
        if (!mogelijkeZetten.Contains(new Tuple<int, int>(naarY, naarX))) return false;

        stapelBord.Duw((char[,])bord.Clone());

        if (Math.Abs(naarY - vanY) <= 1 && Math.Abs(naarX - vanX) <= 1)
        {
            bord[naarY, naarX] = beurt;
        }
        else
        {
            bord[naarY, naarX] = beurt;
            bord[vanY, vanX] = default;
        }

        for (int i = naarY - 1; i <= naarY + 1; i++)
        {
            for (int j = naarX - 1; j <= naarX + 1; j++)
            {
                if (i >= 0 && i < bord.GetLength(1) && j >= 0 && j < bord.GetLength(0))
                {
                    if (bord[i, j] != default && bord[i, j] != beurt)
                    {
                        bord[i, j] = beurt;
                    }
                }
            }
        }

        return true;
    }



    public static List<Tuple<int, int>> KrijgMogelijkeZetten(int posY, int posX)
    {
        List<Tuple<int, int>> mogelijkeZetten = new List<Tuple<int, int>>();

        var beurt = bord[posY, posX];

        if (IsPosBuitenSpel(posY, posX)) return mogelijkeZetten;

        for (int i = posY - 2; i <= posY + 2; i++)
        {
            for (int j = posX - 2; j <= posX + 2; j++)
            {
                if (i >= 0 && i < bord.GetLength(1) && j >= 0 && j < bord.GetLength(0))
                {
                    if (bord[i, j] == default)
                    {
                        mogelijkeZetten.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
        }

        return mogelijkeZetten;
    }

    public static bool IsPosBuitenSpel(int posY, int posX)
    {
        if (posY < 0 || posY > 6) return true;
        if (posX < 0 || posX > 6) return true;
        return false;
    }
}