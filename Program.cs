using System.Diagnostics.Metrics;
using System.Security.Cryptography;

public class Program
{
    public static void Main(string[] args)
    {
        char[,] engineData = GetMatrix(ReadFile("input.txt")); 
        Part1(engineData); 
        Part2(engineData); 
    }

    public static bool Part1(char[,] data)
    {
        int result = 0;
        string cache = "";
        bool isNumber = false;
        string message = "";

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (char.IsDigit(data[i, j]))
                { 
                    if (isPiece(data, i, j) || isNumber)
                    {
                        cache += data[i, j];
                        isNumber = true;
                    }
                    else
                    {
                        cache += data[i, j];
                    }
                }
                else
                {
                    if (isNumber)
                    {
                        message += cache + ",";
                        result += int.Parse(cache);
                    }
                    cache = "";
                    isNumber = false;
                }
  
                if (j==data.GetLength(1)-1 && isNumber) 
                {
                    message += cache + ",";
                    result += int.Parse(cache);
                    cache = "";
                    isNumber = false;
                }

            }
        }
        if (isNumber)
        { 
            result += int.Parse(cache);
        }
        Console.WriteLine("Parte 1:"+result);
 
        return false;
    }
   
    public static bool isBounds(int x, int y, int row, int col)
{
        if (y >= 0 && x >= 0 && x < row && y < col)
        { 
            return true;
        }
    return false;
}

public static bool isPiece(char[,] data, int i, int j)
{ 
    int row = data.GetLength(0);
    int col = data.GetLength(1);

    int up = i - 1;
    int down = i + 1;
    int left = j - 1;
    int right = j + 1;

    if (isBounds(up, j, row, col) && IsSymbol(data[up, j]))
    { 
            return true;
    }
    if (isBounds(down, j, row, col) && IsSymbol(data[down, j]))
    { 
        return true;
    }
    if (isBounds(i, left, row, col) && IsSymbol(data[i, left]))
    { 
            return true;
    }
    if (isBounds(i, right, row, col) && IsSymbol(data[i, right]))
    { 
            return true;
    }
    if (isBounds(up, left, row, col) && IsSymbol(data[up, left]))
    {
        return true;
    }
    if (isBounds(up, right, row, col) && IsSymbol(data[up, right]))
    {
        return true;
    }
    if (isBounds(down, left, row, col) && IsSymbol(data[down, left]))
    {
        return true;
    }
    if (isBounds(down, right, row, col) && IsSymbol(data[down, right]))
    {
        return true;
    }

    return false;
}
    public static bool Part2(char[,] data)
    { 
        string cache = "";
        bool isNumber = false;
        bool isGear = false;
        string message = "";
        int countGears = 0;
        int result = 0;
        int[,] gears = new int[data.GetLength(0), data.GetLength(1)];
        int[,] gearsMirror = new int[data.GetLength(0), data.GetLength(1)];
        LinkedList<(int, int)> tempCounterGears = new LinkedList<(int, int)>(); 

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (char.IsDigit(data[i, j]))
                {
                    if (getGears(data, i, j).Count > 0 || isNumber)
                    { 
                        cache += data[i, j]; 
                        tempCounterGears = UnionLinkedList(tempCounterGears, getGears(data, i, j)); 
                        isNumber = true;
                        isGear= true;
                    }
                    else
                    {
                        cache += data[i, j];
                    }
                }
                else
                { 
                    if (isNumber && cache != "")
                    {
                        message += cache + ",";
                        if (isGear)
                        { 
                            foreach (var gear in tempCounterGears)
                            {
                                if (gears[gear.Item1, gear.Item2] == 0)
                                {
                                    gears[gear.Item1, gear.Item2] = int.Parse(cache);
                                }
                                else
                                {
                                    gears[gear.Item1, gear.Item2] = gears[gear.Item1, gear.Item2] * int.Parse(cache);
                                }

                                gearsMirror[gear.Item1, gear.Item2]++; 
                            }
                            tempCounterGears.Clear();
                        }
                    }
                    cache = "";
                    isNumber = false;
                    isGear = false;
                }
                if (j == data.GetLength(1) - 1 && isNumber && cache != "")
                { 
                    message += cache + ",";
                    if (isGear)
                    { 
                        foreach (var gear in tempCounterGears)
                        {
                            if(gears[gear.Item1, gear.Item2] == 0)
                            {
                                gears[gear.Item1, gear.Item2] = int.Parse(cache);
                            }
                            else
                            {
                               gears[gear.Item1, gear.Item2] = gears[gear.Item1, gear.Item2]*int.Parse(cache);
                            }
                            
                            gearsMirror[gear.Item1, gear.Item2]++;
                        }
                        tempCounterGears.Clear(); 
                    }
                    cache = "";
                    isNumber = false;
                    isGear = false;
 
                }

            }
        }
       
        for (int i = 0; i < gears.GetLength(0); i++)
        {
            for (int j = 0; j < gears.GetLength(1); j++)
            {
                if (gearsMirror[i, j] == 1 || gearsMirror[i, j] == 0)
                { 
                    gears[i,j]=0;
                }
                else
                {
                    result+= gears[i, j]; 
                }
            }
        }
        message = "";
        for (int i = 0; i < gears.GetLength(0); i++)
        {
            for (int j = 0; j < gears.GetLength(1); j++)
            {
                if (gearsMirror[i, j] == 1 || gearsMirror[i, j] == 0)
                {
                    message += "[-]";
                }
                else
                {
                    message += "[" + gears[i, j] + "]";
                }
            }
            message += "\n";
        } 
        Console.WriteLine("Parte 2:" + result);
        return false;
    }
    public static LinkedList<(int, int)> getGears(char[,] data, int i, int j)
    {
        int row = data.GetLength(0);
        int col = data.GetLength(1);
        LinkedList<(int, int)> positionsGears = new LinkedList<(int, int)>();

        int up = i - 1;
        int down = i + 1;
        int left = j - 1;
        int right = j + 1;

        if (isBounds(up, j, row, col) && IsGear(data[up, j]))
        {
            positionsGears.AddLast((up, j));
        }
        if (isBounds(down, j, row, col) && IsGear(data[down, j]))
        {
            positionsGears.AddLast((down, j));
        }
        if (isBounds(i, left, row, col) && IsGear(data[i, left]))
        {
            positionsGears.AddLast((i, left));
        }
        if (isBounds(i, right, row, col) && IsGear(data[i, right]))
        {
            positionsGears.AddLast((i, right));
        }
        if (isBounds(up, left, row, col) && IsGear(data[up, left]))
        {
            positionsGears.AddLast((up, left));
        }
        if (isBounds(up, right, row, col) && IsGear(data[up, right]))
        {
            positionsGears.AddLast((up, right));
        }
        if (isBounds(down, left, row, col) && IsGear(data[down, left]))
        {
            positionsGears.AddLast((down, left));
        }
        if (isBounds(down, right, row, col) && IsGear(data[down, right]))
        {
            positionsGears.AddLast((down, right));
        }

        return positionsGears;
    }
    public static LinkedList<(int, int)> UnionLinkedList(LinkedList<(int, int)> origin, LinkedList<(int, int)> newGears)
    {
        if (newGears.Count>0)
        {
            if(origin.Count == 0)
            {
                return newGears;
            }
            foreach (var gear in newGears)
            {
                if (!origin.Contains(gear))
                {
                    origin.AddLast(gear);
                }
            }
        }
        return origin;
    }
    public static bool IsSymbol(char letter)
    {
        return !char.IsDigit(letter) && letter != '.' && letter != '\0' && letter != ' ';
    }

    public static bool IsGear(char letter)
    {
        return letter == '*';
    }
    public static char[,] GetMatrix(LinkedList<string> listWords)
    {
        int rowCount = listWords.Count;
        int colCount = listWords.First.Value.Length;
        int i = 0;
        char[,] matrix = new char[rowCount, colCount];
        
        foreach (string word in listWords)
        {
            if (word.Length != colCount)
            {
                throw new ArgumentException("Error de tamanos");
            }

            for (int j = 0; j < word.Length; j++)
            {
                matrix[i, j] = word[j];
            }
            i++;
        }

        return matrix;
    }
    public static LinkedList<string> ReadFile(string fileName)
    {
        string file = @"C:\Users\samue\Downloads\Profesion\Advent of Code\Day-3\Solution_Day3\Resources\" + fileName;
        LinkedList<string> listWords = new LinkedList<string>();
        Console.WriteLine("Reading File using File.ReadAllText()");

        if (File.Exists(file))
        {
            StreamReader Textfile = new StreamReader(file);
            string line;

            while ((line = Textfile.ReadLine()) != null)
            {
                listWords.AddLast(line);
            }

            Textfile.Close();

        }
        return listWords;
    }
}
