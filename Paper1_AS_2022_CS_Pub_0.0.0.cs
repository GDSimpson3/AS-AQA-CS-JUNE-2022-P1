// Skeleton Program for the AQA AS Summer 2022 examination
// this code should be used in conjunction with the Preliminary Material
// written by the AQA Programmer Team
// developed in VS2019 using .net framework

// Version number: 0.0.0

using System;
using System.IO;

namespace NumberPuzzle
{
  class Program
  {
    const string EMPTY_STRING = "";
    const char SPACE = ' ';
    const int GRID_SIZE = 9;

    private static void ResetDataStructures(char[,] puzzleGrid, string[] puzzle, string[] answer, string[] solution)
    {
      for (int line = 0; line < GRID_SIZE * GRID_SIZE; line++)
      {
        puzzle[line] = EMPTY_STRING;
      }
      for (int row = 0; row <= GRID_SIZE; row++)
      {
        for (int column = 0; column <= GRID_SIZE; column++)
        {
          puzzleGrid[row, column] = SPACE;
        }
      }
      for (int line = 0; line <= GRID_SIZE; line++)
      {
        solution[line] = EMPTY_STRING;
      }
      for (int line = 0; line < 2 * GRID_SIZE * GRID_SIZE; line++)
      {
        answer[line] = EMPTY_STRING;
      }
    }

    private static bool LoadPuzzleFile(string puzzleName, string[] puzzle)
    {
      bool ok;
      try
      {
        int line = 0;
        string cellInfo = "";
        using (StreamReader fileIn = new StreamReader(puzzleName + ".txt"))
        {
          while (!fileIn.EndOfStream)
          {
            cellInfo = fileIn.ReadLine();
            puzzle[line] = cellInfo;
            line++;
          }
        }
        if (line == 0)
        {
          Console.WriteLine("Puzzle file empty");
          ok = false;
        }
        else
        {
          ok = true;
        }
      }
      catch (Exception)
      {
        Console.WriteLine("Puzzle file does not exist");
        ok = false;
      }
      return ok;
    }

    private static bool LoadSolution(string puzzleName, string[] solution)
    {
      bool ok = true;
      try
      {
        using (StreamReader fileIn = new StreamReader(puzzleName + "S.txt"))
        {
          for (int line = 1; line <= GRID_SIZE; line++)
          {
            solution[line] = SPACE + fileIn.ReadLine();
            if (solution[line].Length != GRID_SIZE + 1)
            {
              ok = false;
              Console.WriteLine("File data error");
            }
          }
        }
      }
      catch (Exception)
      {
        Console.WriteLine("Solution file does not exist");
        ok = false;
      }
      return ok;
    }

    private static void ResetAnswer(string puzzleName, string[] answer)
    {
      answer[0] = puzzleName;
      answer[1] = "0";
      answer[2] = "0";
      for (int line = 3; line < 2 * GRID_SIZE * GRID_SIZE; line++)
      {
        answer[line] = EMPTY_STRING;
      }
    }

    private static bool TransferPuzzleIntoGrid(string puzzleName, char[,] puzzleGrid, string[] puzzle, string[] answer)
    {
      bool ok = true;
      try
      {
        int line = 0;
        string cellInfo = puzzle[line];
        while (cellInfo != EMPTY_STRING)
        {
          int row = Convert.ToInt32(cellInfo[0].ToString());
          int column = Convert.ToInt32(cellInfo[1].ToString());
          char digit = cellInfo[2];
          puzzleGrid[row, column] = digit;
          line++;
          cellInfo = puzzle[line];
        }
        puzzleGrid[0, 0] = 'X';
        ResetAnswer(puzzleName, answer);
      }
      catch (Exception)
      {
        Console.WriteLine("Error in puzzle file");
        ok = false;
      }

      return ok;
    }

    private static void LoadPuzzle(char[,] puzzleGrid, string[] puzzle, string[] answer, string[] solution)
    {
      ResetDataStructures(puzzleGrid, puzzle, answer, solution);
      Console.Write("Enter puzzle name to load: ");
      string puzzleName = Console.ReadLine();
      bool ok = LoadPuzzleFile(puzzleName, puzzle);
      if (ok)
      {
        ok = LoadSolution(puzzleName, solution);
      }
      if (ok)
      {
        ok = TransferPuzzleIntoGrid(puzzleName, puzzleGrid, puzzle, answer);
      }
      if (!ok)
      {
        ResetDataStructures(puzzleGrid, puzzle, answer, solution);
      }
    }

    private static void TransferAnswerIntoGrid(char[,] puzzleGrid, string[] answer)
    {
      string cellInfo = "";
      int row, column;
      char digit;
      for (int line = 3; line < Convert.ToInt32(answer[2].ToString()) + 3; line++)
      {
        cellInfo = answer[line];
        row = Convert.ToInt32(cellInfo[0].ToString());
        column = Convert.ToInt32(cellInfo[1].ToString());
        digit = cellInfo[2];
        puzzleGrid[row, column] = digit;
      }
    }

    private static void LoadPartSolvedPuzzle(char[,] puzzleGrid, string[] puzzle, string[] answer, string[] solution)
    {
      LoadPuzzle(puzzleGrid, puzzle, answer, solution);
      try
      {
        string puzzleName = answer[0];
        using (StreamReader fileIn = new StreamReader(puzzleName + "P.txt"))
        {
          string cellInfo = fileIn.ReadLine();
          if (puzzleName != cellInfo)
          {
            Console.WriteLine("Partial solution file is corrupt");
          }
          else
          {
            int line = 0;
            while (cellInfo != null)
            {
              answer[line] = cellInfo;
              line++;
              cellInfo = fileIn.ReadLine();
            }
          }
        }
        TransferAnswerIntoGrid(puzzleGrid, answer);
      }
      catch (Exception)
      {
        Console.WriteLine("Partial solution file does not exist");
      }
    }

    private static void DisplayGrid(char[,] puzzleGrid)
    {
      Console.WriteLine();
      Console.WriteLine("   1   2   3   4   5   6   7   8   9  ");
      Console.WriteLine(" |===.===.===|===.===.===|===.===.===|");
      for (int row = 1; row < GRID_SIZE + 1; row++)
      {
        Console.Write($"{row}|");
        for (int column = 1; column <= GRID_SIZE; column++)
        {
          if (column % 3 == 0)
          {
            Console.Write($"{SPACE}{puzzleGrid[row, column]}{SPACE}|");
          }
          else
          {
            Console.Write($"{SPACE}{puzzleGrid[row, column]}{SPACE}.");
          }
        }
        Console.WriteLine();
        if (row % 3 == 0)
        {
          Console.WriteLine(" |===.===.===|===.===.===|===.===.===|");
        }
        else
        {
          Console.WriteLine(" |...........|...........|...........|");
        }
      }
      Console.WriteLine();
    }

    private static void SolvePuzzle(char[,] puzzleGrid, string[] puzzle, string[] answer)
    {
      int row = 0, column = 0;
      char digit = ' ';
      DisplayGrid(puzzleGrid);
      if (puzzleGrid[0, 0] != 'X')
      {
        Console.WriteLine("No puzzle loaded");
      }
      else
      {
        Console.WriteLine("Enter row column digit: ");
        Console.WriteLine("(Press Enter to stop)");
        string cellInfo = Console.ReadLine();
        while (cellInfo != EMPTY_STRING)
        {
          bool inputError = false;
          if (cellInfo.Length != 3)
          {
            inputError = true;
          }
          else
          {
            digit = cellInfo[2];
            try
            {
              row = Convert.ToInt32(cellInfo[0].ToString());
            }
            catch (Exception)
            {
              inputError = true;
            }
            try
            {
              column = Convert.ToInt32(cellInfo[1].ToString());
            }
            catch (Exception)
            {
              inputError = true;
            }
            if (digit < '1' || digit > '9')
            {
              inputError = true;
            }
          }
          if (inputError)
          {
            Console.WriteLine("Invalid input");
          }
          else
          {
            puzzleGrid[row, column] = digit;
            answer[2] = (Convert.ToInt32(answer[2]) + 1).ToString();
            answer[Convert.ToInt32(answer[2]) + 2] = cellInfo;
            DisplayGrid(puzzleGrid);
          }
          Console.WriteLine("Enter row column digit: ");
          Console.WriteLine("(Press Enter to stop)");
          cellInfo = Console.ReadLine();
        }
      }
    }

    private static void DisplayMenu()
    {
      Console.WriteLine();
      Console.WriteLine("Main Menu");
      Console.WriteLine("=========");
      Console.WriteLine("L - Load new puzzle");
      Console.WriteLine("P - Load partially solved puzzle");
      Console.WriteLine("S - Solve puzzle");
      Console.WriteLine("C - Check solution");
      Console.WriteLine("K - Keep partially solved puzzle");
      Console.WriteLine("X - Exit");
      Console.WriteLine();
    }

    private static char GetMenuOption()
    {
      string choice = EMPTY_STRING;
      while (choice.Length != 1)
      {
        Console.Write("Enter your choice: ");
        choice = Console.ReadLine();
      }
      return choice[0];
    }

    private static void KeepPuzzle(char[,] puzzleGrid, string[] answer)
    {
      if (puzzleGrid[0, 0] != 'X')
      {
        Console.WriteLine("No puzzle loaded");
      }
      else
      {
        if (Convert.ToInt32(answer[2].ToString()) > 0)
        {
          string puzzleName = answer[0];
          using (StreamWriter fileOut = new StreamWriter(puzzleName + "P.txt"))
          {
            for (int line = 0; line < Convert.ToInt32(answer[2].ToString()) + 3; line++)
            {
              fileOut.WriteLine(answer[line]);
            }
          }
        }
        else
        {
          Console.WriteLine("No answers to keep");
        }
      }
    }

    private static void CheckSolution(char[,] puzzleGrid, string[] answer, string[] solution, ref int errorCount, ref bool solved)
    {
      bool correct = true, incomplete = false;
      char entry;
      errorCount = 0;
      solved = false;
      for (int row = 1; row <= GRID_SIZE; row++)
      {
        for (int column = 1; column <= GRID_SIZE; column++)
        {
          entry = puzzleGrid[row, column];
          if (entry == SPACE)
          {
            incomplete = true;
          }
          if (!((entry == solution[row][column]) || (entry == SPACE)))
          {
            correct = false;
            errorCount++;
            Console.WriteLine($"You have made an error in row {row} column {column}");
          }
        }
      }
      if (!correct)
      {
        Console.WriteLine($"You have made {errorCount} error(s)");
      }
      else if (incomplete)
      {
        Console.WriteLine("So far so good, carry on");
      }
      else if (correct)
      {
        solved = true;
      }
    }

    private static void CalculateScore(string[] answer, int errorCount)
    {
      answer[1] = (Convert.ToInt32(answer[1]) - errorCount).ToString();
    }

    private static void DisplayResults(string[] answer)
    {
      if (Convert.ToInt32(answer[2]) > 0)
      {
        Console.WriteLine($"Your score is {answer[1]}");
        Console.WriteLine($"Your solution for {answer[0]} was: ");
        for (int line = 3; line < Convert.ToInt32(answer[2]) + 3; line++)
        {
          Console.WriteLine(answer[line]);
        }
      }
      else
      {
        Console.WriteLine("You didn't make a start");
      }
    }

    private static void NumberPuzzle()
    {
      Random rnd = new Random();
      string[] puzzle = new string[GRID_SIZE * GRID_SIZE];
      char[,] puzzleGrid = new char[GRID_SIZE + 1, GRID_SIZE + 1];
      string[] solution = new string[GRID_SIZE + 1];
      string[] answer = new string[2 * GRID_SIZE * GRID_SIZE];
      bool finished = false, solved = false;
      int errorCount = 0;
      char menuOption = ' ';
      ResetDataStructures(puzzleGrid, puzzle, answer, solution);
      while (!finished)
      {
        DisplayMenu();
        menuOption = GetMenuOption();
        if (menuOption == 'L')
        {
          LoadPuzzle(puzzleGrid, puzzle, answer, solution);
        }
        else if (menuOption == 'P')
        {
          LoadPartSolvedPuzzle(puzzleGrid, puzzle, answer, solution);
        }
        else if (menuOption == 'K')
        {
          KeepPuzzle(puzzleGrid, answer);
        }
        else if (menuOption == 'C')
        {
          if (puzzleGrid[0, 0] != 'X')
          {
            Console.WriteLine("No puzzle loaded");
          }
          else
          {
            if (Convert.ToInt32(answer[2].ToString()) > 0)
            {
              CheckSolution(puzzleGrid, answer, solution, ref errorCount, ref solved);
              CalculateScore(answer, errorCount);
              if (solved)
              {
                Console.WriteLine("You have successfully solved the puzzle");
                finished = true;
              }
              else
              {
                Console.WriteLine($"Your score so far is {answer[1]}");
              }
            }
            else
            {
              Console.WriteLine("No answers to check");
            }
          }
        }
        else if (menuOption == 'S')
        {
          SolvePuzzle(puzzleGrid, puzzle, answer);
        }
        else if (menuOption == 'X')
        {
          finished = true;
        }
        else
        {
          int responseNumber = rnd.Next(1, 6);
          if (responseNumber == 1)
          {
            Console.WriteLine("Invalid menu option. Try again");
          }
          else if (responseNumber == 2)
          {
            Console.WriteLine("You did not choose a valid menu option. Try again");
          }
          else if (responseNumber == 3)
          {
            Console.WriteLine("Your menu option is not valid. Try again");
          }
          else if (responseNumber == 4)
          {
            Console.WriteLine("Only L, P, S, C, K or X are valid menu options. Try again");
          }
          else if (responseNumber == 5)
          {
            Console.WriteLine("Try one of L, P, S, C, K or X ");
          }
        }
      }
      if (answer[2] != EMPTY_STRING)
      {
        DisplayResults(answer);
      }
    }

    static void Main(string[] args)
    {
      NumberPuzzle();
      Console.ReadLine();
    }
  }
}
