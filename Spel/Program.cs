namespace Main;

public class Program
{
    public static char[,] bord = new char[7, 7];
    public static Stapel<char[,]> stapelBord = new Stapel<char[,]>();

    public static void Main(string[] args)
    {
        SetStartOpstelling();
        PrintBord();

        var beurt = 'H';

        while (!IsSpelAfgelopen())
        {
            Console.WriteLine("Beurt: " + beurt);
            Console.WriteLine("Van welke positie wil je een zet doen? x,y of typ undo om de laatste zet ongedaan te maken");
            KrijgLocatieInput(out var vanY, out var vanX);

            if (vanY == -1 && vanX == -1)
            {
                var oudBord = stapelBord.Pak();
                if (oudBord == default)
                {
                    PrintBord();
                    Console.WriteLine("Kan niet undo doen, er is nog geen zet gedaan");
                    continue;
                }
                bord = oudBord;
                PrintBord();
                beurt = (beurt == 'H') ? 'B' : 'H';
                continue;
            }

            if (IsPosBuitenSpel(vanY, vanX))
            {
                PrintBord();
                Console.WriteLine("Ongeldige zet");
                continue;
            }

            Console.WriteLine("Naar welke positie wil je een zet doen? (x, y)");
            KrijgLocatieInput(out var naarY, out var naarX);

            if (DoeZet(vanY, vanX, naarY, naarX, beurt))
            {
                PrintBord();
                beurt = (beurt == 'H') ? 'B' : 'H';
            }
            else
            {
                PrintBord();
                Console.WriteLine("Ongeldige zet");
            }
        }
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
        if (parts.Length != 2 || !int.TryParse(parts[0], out x) || !int.TryParse(parts[1], out y))
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
        Console.Write("y \n");
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

        Console.WriteLine("x");
    }

    public static bool DoeZet(int vanY, int vanX, int naarY, int naarX, char speler)
    {
        if (IsPosBuitenSpel(vanY, vanX)) return false;
        if (IsPosBuitenSpel(naarY, naarX)) return false;

        if (bord[vanY, vanX] != speler) return false;
        if (bord[naarY, naarX] == speler) return false;

        var mogelijkeZetten = KrijgMogelijkeZetten(vanY, vanX);
        if (!mogelijkeZetten.Contains(new Tuple<int, int>(naarY, naarX))) return false;

        stapelBord.Duw((char[,])bord.Clone());

        if (Math.Abs(naarY - vanY) <= 1 && Math.Abs(naarX - vanX) <= 1)
        {
            bord[naarY, naarX] = speler;
        }
        else
        {
            bord[naarY, naarX] = speler;
            bord[vanY, vanX] = default;
        }

        for (int i = naarY - 1; i <= naarY + 1; i++)
        {
            for (int j = naarX - 1; j <= naarX + 1; j++)
            {
                if (i >= 0 && i < bord.GetLength(1) && j >= 0 && j < bord.GetLength(0))
                {
                    if (bord[i, j] != default && bord[i, j] != speler)
                    {
                        bord[i, j] = speler;
                    }
                }
            }
        }

        return true;
    }



    public static List<Tuple<int, int>> KrijgMogelijkeZetten(int posY, int posX)
    {
        List<Tuple<int, int>> mogelijkeZetten = new List<Tuple<int, int>>();

        var speler = bord[posY, posX];

        if (IsPosBuitenSpel(posY, posX)) return mogelijkeZetten;

        // 1 ver

        //links
        if (posY - 1 >= 0 && bord[posY - 1, posX] != speler) mogelijkeZetten.Add(Tuple.Create(posY - 1, posX));
        //rechts
        if (posY + 1 < bord.GetLength(1) && bord[posY + 1, posX] != speler) mogelijkeZetten.Add(Tuple.Create(posY + 1, posX));
        //boven
        if (posX - 1 >= 0 && bord[posY, posX - 1] != speler) mogelijkeZetten.Add(Tuple.Create(posY, posX - 1));
        //onder
        if (posX + 1 < bord.GetLength(0) && bord[posY, posX + 1] != speler) mogelijkeZetten.Add(Tuple.Create(posY, posX + 1));
        //linksboven
        if (posY - 1 >= 0 && posX - 1 >= 0 && bord[posY - 1, posX - 1] != speler) mogelijkeZetten.Add(Tuple.Create(posY - 1, posX - 1));
        //linksonder
        if (posY - 1 >= 0 && posX + 1 < bord.GetLength(0) && bord[posY - 1, posX + 1] != speler) mogelijkeZetten.Add(Tuple.Create(posY - 1, posX + 1));
        //rechtsboven
        if (posY + 1 < bord.GetLength(1) && posX - 1 >= 0 && bord[posY + 1, posX - 1] != speler) mogelijkeZetten.Add(Tuple.Create(posY + 1, posX - 1));
        //rechtsonder
        if (posY + 1 < bord.GetLength(1) && posX + 1 < bord.GetLength(0) && bord[posY + 1, posX + 1] != speler) mogelijkeZetten.Add(Tuple.Create(posY + 1, posX + 1));

        // 2 ver

        //boven
        if (posX - 2 >= 0 && bord[posY, posX - 2] != speler) mogelijkeZetten.Add(Tuple.Create(posY, posX - 2));
        //onder
        if (posX + 2 < bord.GetLength(0) && bord[posY, posX + 2] != speler) mogelijkeZetten.Add(Tuple.Create(posY, posX + 2));
        //links
        if (posY - 2 >= 0 && bord[posY - 2, posX] != speler) mogelijkeZetten.Add(Tuple.Create(posY - 2, posX));
        //rechts
        if (posY + 2 < bord.GetLength(1) && bord[posY + 2, posX] != speler) mogelijkeZetten.Add(Tuple.Create(posY + 2, posX));

        return mogelijkeZetten;
    }

    public static bool IsPosBuitenSpel(int posY, int posX)
    {
        if (posY < 0 || posY > 6) return true;
        if (posX < 0 || posX > 6) return true;
        return false;
    }
}