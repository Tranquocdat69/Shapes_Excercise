string _projectDirectory = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "");
string _filePath = $"{_projectDirectory}//result.txt";
List<string> _listShapes = File.ReadAllLines($"{_projectDirectory}//data.txt").ToList();
List<string> _listResult = new List<string>();
const string Rect = "RECT";
const string Circ = "CIRC";
const string Tria = "TRIA";
const int NumberOfUnitsRight = 5;
const int NumberOfUnitsUp = 2;
const int NumberOfUnitsLeft = 2;
decimal _tempX = Decimal.MaxValue;
string _leftMostSquare = String.Empty;

// Check each of shape to handle
HandleShapes(_listShapes, _listResult);

// 5.Double the width of the left-most square, turning into rectangle
ConvertLeftMostSquareToRectagle(_listResult);

// Write result to file
WriteToFile(_filePath, _listResult);

#region Methods
void HandleShapes(List<string> listShapes, List<string> listResult)
{
    for (int i = 0; i < listShapes.Count; i++)
    {
        string result = String.Empty;
        string shape = listShapes[i];
        string[] array = shape.Split(" ");

        if (shape.Contains(Rect))
            result = HandleRectagle(array);

        if (shape.Contains(Circ))
            result = HandleCircle(array);

        if (shape.Contains(Tria))
            result = HandleTriagle(array);

        listResult.Add(result);
    }
}

void ConvertLeftMostSquareToRectagle(List<string> list)
{
    for (int i = 0; i < list.Count; i++)
    {
        string shape = list[i];
        string[] array = shape.Split(" ");

        if (shape.Equals(_leftMostSquare))
        {

            string width = array[2];
            string height = array[3].Replace(")", "");
            string x = array[0].Replace($"{Rect}(", "");
            string y = array[1].Replace(",", "");

            decimal originWidth = ConvertToDecimal(width);
            decimal originHeight = ConvertToDecimal(height);
            decimal originX = ConvertToDecimal(x);
            decimal originY = ConvertToDecimal(y);

            decimal newWidth = originWidth * 2;

            string result = $"{Rect}({originX} {originY}, {newWidth} {originHeight})";
            list[i] = result;
            break;
        }
    }
}

string HandleRectagle(string[] array)
{
    string result = String.Empty;

    string width = array[2];
    string height = array[3].Replace(")", "");
    decimal originWidth = ConvertToDecimal(width);
    decimal originHeight = ConvertToDecimal(height);

    if (IsSquare(originWidth, originHeight))
    {
        string x = array[0].Replace($"{Rect}(", "");
        string y = array[1].Replace(",", "");
        decimal originX = ConvertToDecimal(x);
        decimal originY = ConvertToDecimal(y);
        // 1.Move square 5 units to right 
        decimal newX = originX + NumberOfUnitsRight;

        // 3.Move 2 units up and 2 units to the left
        decimal newY = originY + NumberOfUnitsUp;
        newX = newX - NumberOfUnitsLeft;

        result = $"{Rect}({newX} {newY}, {originWidth} {originHeight})";

        // Get left most square by get smallest value of x
        if (originX < _tempX)
        {
            _tempX = originX;
            _leftMostSquare = result;
        }
    }
    return result;
}

string HandleCircle(string[] array)
{
    string result = String.Empty;

    string x = array[0].Replace($"{Circ}(", "");
    string y = array[1].Replace(",", "");
    string radius = array[2].Replace(")", "");

    decimal originX = ConvertToDecimal(x);
    decimal originY = ConvertToDecimal(y);
    decimal originRadius = ConvertToDecimal(radius);

    // 2.Increase the diameter by 2 units
    decimal newRadius = originRadius + 1;

    // 3.Move 2 units up and 2 units to the left
    decimal newY = originY + NumberOfUnitsUp;
    decimal newX = originX - NumberOfUnitsLeft;

    result = $"{Circ}({newX} {newY}, {newRadius})";
    return result;
}

string HandleTriagle(string[] array)
{
    string result = String.Empty;

    string x1 = array[0].Replace($"{Tria}(", "");
    string y1 = array[1].Replace(",", "");

    string x2 = array[2];
    string y2 = array[3].Replace(",", "");

    string x3 = array[4];
    string y3 = array[5].Replace(")", "");

    decimal originX1 = ConvertToDecimal(x1);
    decimal originY1 = ConvertToDecimal(y1);

    decimal originX2 = ConvertToDecimal(x2);
    decimal originY2 = ConvertToDecimal(y2);

    decimal originX3 = ConvertToDecimal(x3);
    decimal originY3 = ConvertToDecimal(y3);

    // 3.Move 2 units up and 2 units to the left
    decimal newX1 = originX1 - NumberOfUnitsLeft;
    decimal newY1 = originY1 + NumberOfUnitsUp;

    decimal newX2 = originX2 - NumberOfUnitsLeft;
    decimal newY2 = originY2 + NumberOfUnitsUp;

    decimal newX3 = originX3 - NumberOfUnitsLeft;
    decimal newY3 = originY3 + NumberOfUnitsUp;

    result = $"{Tria}({newX1} {newY1}, {newX2} {newY2}, {newX3} {newY3})";
    return result;
}

void WriteToFile(string filePath, List<string> content) => File.WriteAllLines(filePath, content);

bool IsSquare(decimal w, decimal h) => w == h;

decimal ConvertToDecimal(string value) => decimal.Parse(value);
#endregion