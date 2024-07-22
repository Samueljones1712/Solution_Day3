using System.Diagnostics.Metrics;
using System.Security.Cryptography;

public class Program
{
    public static void Main(string[] args)
    {
        char[,] engineData = GetMatrix(ReadFile("input.txt"));//Se selecciona el archivo de texto
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
                {//Si es un numero lo guardamos en cache
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
                //Si estamos en la ultima columna y es un numero lo sumamos
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
        {//Si al final del recorrido hay un numero lo sumamos
            result += int.Parse(cache);
        }
        Console.WriteLine("Parte 1:"+result);
        //Console.WriteLine(message);
        return false;
    }
   
    public static bool isBounds(int x, int y, int row, int col)
{
        if (y >= 0 && x >= 0 && x < row && y < col)
        {//Nos aseguramos que no se salga de los limites de la matriz
            return true;
        }
    return false;
}

public static bool isPiece(char[,] data, int i, int j)
{//Verifica que la pieza este proxima a un simbolo
    int row = data.GetLength(0);
    int col = data.GetLength(1);

    int up = i - 1;
    int down = i + 1;
    int left = j - 1;
    int right = j + 1;

    if (isBounds(up, j, row, col) && IsSymbol(data[up, j]))
    {//Si hay un simbolo arriba
            return true;
    }
    if (isBounds(down, j, row, col) && IsSymbol(data[down, j]))
    {//Si hay un simbolo abajo
        return true;
    }
    if (isBounds(i, left, row, col) && IsSymbol(data[i, left]))
    {//Si hay un simbolo a la izquierda
            return true;
    }
    if (isBounds(i, right, row, col) && IsSymbol(data[i, right]))
    {//Si hay un simbolo a la derecha
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
    {//Recorremos la matriz y buscamos las ruedas
        string cache = "";
        bool isNumber = false;
        bool isGear = false;
        string message = "";
        int countGears = 0;
        int result = 0;
        int[,] gears = new int[data.GetLength(0), data.GetLength(1)];
        int[,] gearsMirror = new int[data.GetLength(0), data.GetLength(1)];
        LinkedList<(int, int)> tempCounterGears = new LinkedList<(int, int)>();//Lista con X y Y del *

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (char.IsDigit(data[i, j]))
                {
                    if (getGears(data, i, j).Count > 0 || isNumber)
                    {//Si es un numero lo guardamos en cache
                        cache += data[i, j];//Se guarda el numero en cache
                        tempCounterGears = UnionLinkedList(tempCounterGears, getGears(data, i, j));//Se guardan las ruedas en la lista
                        isNumber = true;
                        isGear= true;
                    }
                    else
                    {
                        cache += data[i, j];
                    }
                }
                else
                {//Si no es un numero y ya habiamos guardado un numero
                    if (isNumber && cache != "")
                    {
                        message += cache + ",";
                        if (isGear)
                        {//Si es una rueda
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

                                gearsMirror[gear.Item1, gear.Item2]++;//Se suma la cantidad de veces que se repite la rueda
                            }
                            tempCounterGears.Clear();
                        }
                    }
                    cache = "";
                    isNumber = false;
                    isGear = false;
                }
                if (j == data.GetLength(1) - 1 && isNumber && cache != "")
                {//Si al final del recorrido hay un numero lo sumamos
                    message += cache + ",";
                    if (isGear)
                    {//Si es una rueda
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
                        tempCounterGears.Clear();//Limpiamos la lista de ruedas
                    }
                    cache = "";
                    isNumber = false;
                    isGear = false;
                    //En la matriz de gears sumamos el valor de la pieza en la posicion de la rueda

                }

            }
        }
       
        for (int i = 0; i < gears.GetLength(0); i++)
        {
            for (int j = 0; j < gears.GetLength(1); j++)
            {
                if (gearsMirror[i, j] == 1 || gearsMirror[i, j] == 0)
                {//Si la rueda no se repite o no existe
                    gears[i,j]=0;
                }
                else
                {
                    result+= gears[i, j];//Sumamos el valor de la rueda
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
        //Console.WriteLine(message);
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
            //Console.WriteLine("Arriba:" + data[up, j].ToString() + " [" + (up) + "][" + (j) + "] = " + data[i, j]);
            positionsGears.AddLast((up, j));
        }
        if (isBounds(down, j, row, col) && IsGear(data[down, j]))
        {
            //Console.WriteLine("Abajo:" + data[down, j].ToString() + " [" + (down) + "][" + (j) + "] = " + data[i, j]);
            positionsGears.AddLast((down, j));
        }
        if (isBounds(i, left, row, col) && IsGear(data[i, left]))
        {
            //Console.WriteLine("izquierda:" + data[i, left].ToString() + " [" + (i) + "][" + (left) + "] = " + data[i, j]);
            positionsGears.AddLast((i, left));
        }
        if (isBounds(i, right, row, col) && IsGear(data[i, right]))
        {
            //Console.WriteLine("Derecha:" + data[i, right].ToString() + " [" + (i) + "][" + (right) + "] = " + data[i, j]);
            positionsGears.AddLast((i, right));
        }
        if (isBounds(up, left, row, col) && IsGear(data[up, left]))
        {
            //Console.WriteLine("Arriba Izquierda:" + data[up, left].ToString() + " [" + (up) + "][" + (left) + "] = " + data[i, j]);
            positionsGears.AddLast((up, left));
        }
        if (isBounds(up, right, row, col) && IsGear(data[up, right]))
        {
            //Console.WriteLine("Arriba Derecha:" + data[up, right].ToString() + " [" + (up) + "][" + (right) + "] = " + data[i, j]);
            positionsGears.AddLast((up, right));
        }
        if (isBounds(down, left, row, col) && IsGear(data[down, left]))
        {
            //Console.WriteLine("Abajo Izquierda:" + data[down, left].ToString() + " [" + (down) + "][" + (left) + "] = " + data[i, j]);
            positionsGears.AddLast((down, left));
        }
        if (isBounds(down, right, row, col) && IsGear(data[down, right]))
        {
            //Console.WriteLine("Abajo Derecha:" + data[down, right].ToString() + " [" + (down) + "][" + (right) + "] = " + data[i, j]);
            positionsGears.AddLast((down, right));
        }

        return positionsGears;
    }
    public static LinkedList<(int, int)> UnionLinkedList(LinkedList<(int, int)> origin, LinkedList<(int, int)> newGears)
    {
        //Antes de guardar la nueva lista de ruedas debemos validar que no se repitan
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
        return !char.IsDigit(letter) && letter != '.' && letter != '\0' && letter != ' ';//Si no es un numero, un punto o un espacio
    }

    public static bool IsGear(char letter)
    {//Si es un *
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
            {//Si las palabras no tienen el mismo tamanio
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
    {//Busca las palabras en el archivo de texto
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
    /*
   public static bool isPiece(char[,] data, int i, int j)
   {//Si estamos en i=0 no hay nada arriba
       int up = i-1;
       int down = i+1;
       int left = j-1;
       int right = j+1;

       if (i == 0)
       {
           up = data.GetLength(0)-1;
       }
       if (j == 0) 
       {
           left = data.GetLength(1)-1;
       }
       if (i == data.GetLength(0) - 1)
       {
           down = 0;
       }
       if (j == data.GetLength(1) - 1)
       {
           right = 0;
       }
       // i-1 j / i+1 j / i j-1 / i j+1
       // i-1 j-1 / i-1 j+1 / i+1 j-1 / i+1 j+1
       if (IsSymbol(data[i, right]))
       {
           Console.WriteLine("Derecha:"+ data[i, right].ToString() + " [" + (i + 1 )+ "][" +(right + 1) + "]");
           return true;
       }
       if (IsSymbol(data[i, left]))
       {
           Console.WriteLine("Izquierda:"+ data[i, left].ToString() + " [" + (i + 1) + "][" + (left + 1) + "]");
           return true;
       }
       if (IsSymbol(data[up, j]))
       {
           Console.WriteLine("Arriba:"+ data[up, j].ToString() + " ["+(up + 1 )+ "]["+ (j + 1) + "]");
           return true;
       }
       if (IsSymbol(data[down, j]))
       {
           Console.WriteLine("Abajo:"+ data[down, j].ToString() + " [" + (down + 1) + "][" + (j + 1) + "]");
           return true;
       }
       if (IsSymbol(data[up, right]))
       {
           Console.WriteLine("Arriba Derecha:"+ data[up, right].ToString() + " [" + (up + 1) + "][" + (right + 1) + "]");
           return true;
       }
       if (IsSymbol(data[up, left]))
       {
           Console.WriteLine("Arriba Izquierda:"+ data[up, left].ToString() + " [" + (up + 1) + "][" + (left + 1) + "]");
           return true;
       }
       if (IsSymbol(data[down, right]))
       {
           Console.WriteLine("Abajo Derecha:"+ data[down, right].ToString() + " [" + (down + 1) + "][" + (right + 1) + "]");
           return true;
       }
       if (IsSymbol(data[down, left]))
       {
           Console.WriteLine("Abajo Izquierda:"+ data[down, left].ToString() + " [" + (down + 1) + "][" + (left + 1) + "]");
           return true;
       }

       //Hay que validar la J
       return false;
   }
   */
}
