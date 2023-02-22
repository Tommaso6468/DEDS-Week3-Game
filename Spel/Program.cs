namespace Main;

public class Program
{
    public static char[,] bord = new char[7, 7];
    public static Stapel<char[,]> stapelBord = new Stapel<char[,]>();

    public static void Main(string[] args)
    {
        SetStartOpstelling();
        PrintBord();
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
    }

    public static bool IsSpelAfgelopen()
    {
        // Afgelopen als:
        // - 1 speler geen stukken meer heeft
        // - Beide spelers kunnen geen geldige zet doen
        // Als afgelopen -> print winnaar en return true

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
        Console.Write("\n");
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
            Console.BackgroundColor = default;
            Console.ForegroundColor = default;
            Console.Write("\n");
        }
    }

    public static bool DoeZet(char speler, int vanX, int vanY, int naarX, int naarY)
    {
        // check of zet mogelijk is anders return false
        if (IsPosBuitenSpel(vanX, vanY)) return false;
        if (IsPosBuitenSpel(naarX, naarY)) return false;

        if (bord[vanX, vanY] != speler) return false;

        //check of krijgmogelijkezetten de naar zet bevat

        // get afstand van zet en check of dit geldig is


        // voeg bord toe aan stapel
        stapelBord.Duw(bord);

        // set bord met nieuwe zet dmv afstand van zet

        // check omgeving van nieuwe locatie voor infecteren

        return true;
    }

    public static Dictionary<int, int> KrijgMogelijkeZetten(char speler, int posX, int posY)
    {
        Dictionary<int, int> mogelijkeZetten = new Dictionary<int, int>();

        if (IsPosBuitenSpel(posX, posY)) return mogelijkeZetten;

        // 1 ver

        //links
        if (posX - 1 >= 0 && bord[posX - 1, posY] != speler) mogelijkeZetten.Add(posX - 1, posY);
        //rechts
        if (posX + 1 < bord.GetLength(1) && bord[posX + 1, posY] != speler) mogelijkeZetten.Add(posX + 1, posY);     
        //boven
        if (posY - 1 >= 0 && bord[posX, posY - 1] != speler) mogelijkeZetten.Add(posX, posY - 1);      
        //onder
        if (posY + 1 <= bord.GetLength(0) && bord[posX, posY + 1] != speler) mogelijkeZetten.Add(posX, posY + 1);
        //linksboven
        if (posX - 1 >= 0 && posY - 1 >= 0 && bord[posX - 1, posY - 1] != speler) mogelijkeZetten.Add(posX - 1, posY - 1);
        //linksonder
        if (posX - 1 >= 0 && posY + 1 < bord.GetLength(0) && bord[posX - 1, posY + 1] != speler) mogelijkeZetten.Add(posX - 1, posY + 1);
        //rechtsboven
        if (posX + 1 < bord.GetLength(1) && posY - 1 >= 0 && bord[posX + 1, posY - 1] != speler) mogelijkeZetten.Add(posX + 1, posY - 1);
        //rechtsonder
        if (posX + 1 < bord.GetLength(1) && posY + 1 < bord.GetLength(0) && bord[posX + 1, posY + 1] != speler) mogelijkeZetten.Add(posX + 1, posY + 1);
        

        // 2 ver

        //boven
        if (posY - 2 >= 0 && bord[posX, posY - 2] != speler) mogelijkeZetten.Add(posX, posY - 2);
        //onder
        if (posY + 2 < bord.GetLength(0) && bord[posX, posY + 2] != speler) mogelijkeZetten.Add(posX, posY + 2);
        //links
        if (posX - 2 >= 0 && bord[posX - 2, posY] != speler) mogelijkeZetten.Add(posX - 2, posY);
        //rechts
        if (posX + 2 < bord.GetLength(1) && bord[posX + 2, posY] != speler) mogelijkeZetten.Add(posX + 2, posY);
        

        return mogelijkeZetten;
    }

    public static bool IsPosBuitenSpel(int posX, int posY)
    {
        if (posX < 0 || posX > 6) return true;
        if (posY < 0 || posY > 6) return true;
        return false;
    }
}