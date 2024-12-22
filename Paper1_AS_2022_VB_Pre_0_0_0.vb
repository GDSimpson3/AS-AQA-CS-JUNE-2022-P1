' Skeleton Program for the AQA AS Summer 2022 examination
' This code should be used in conjunction with the Preliminary Material
' Written by the AQA Programmer Team
' Developed using Visual Studio Community 2017
' Version number 0.0.0

Imports System.IO

Module Module1
  Const EMPTY_STRING As String = ""
  Const SPACE As String = " "
  Const GRID_SIZE As Integer = 9

  Sub ResetDataStructures(PuzzleGrid(,) As Char, Puzzle() As String, Answer() As String, Solution() As String)
    For Line As Integer = 0 To GRID_SIZE * GRID_SIZE - 1
      Puzzle(Line) = EMPTY_STRING
    Next
    For Row As Integer = 0 To GRID_SIZE
      For Column As Integer = 0 To GRID_SIZE
        PuzzleGrid(Row, Column) = SPACE
      Next
    Next
    For Line = 0 To GRID_SIZE
      Solution(Line) = EMPTY_STRING
    Next
    For Line = 0 To 2 * GRID_SIZE * GRID_SIZE - 1
      Answer(Line) = EMPTY_STRING
    Next
  End Sub

  Function LoadPuzzleFile(PuzzleName As String, Puzzle() As String) As Boolean
    Dim OK As Boolean
    Try
      Dim Line As Integer = 0
      Dim FileIn As StreamReader = New StreamReader($"{PuzzleName}.txt")  ' using
      While Not FileIn.EndOfStream
        Dim CellInfo As String = FileIn.ReadLine()
        Puzzle(Line) = CellInfo
        Line += 1
      End While
      FileIn.Close()
      If Line = 0 Then
        Console.WriteLine("Puzzle file empty")
        OK = False
      Else
        OK = True
      End If
    Catch
      Console.WriteLine("Puzzle file does not exist")
      OK = False
    End Try
    Return OK
  End Function

  Function LoadSolution(PuzzleName As String, Solution() As String) As Boolean
    Dim OK As Boolean = True
    Try
      Dim FileIn As StreamReader = New StreamReader($"{PuzzleName}S.txt")
      For Line As Integer = 1 To GRID_SIZE
        Try
          Solution(Line) = FileIn.ReadLine()
          If Solution(Line).Length <> GRID_SIZE Then
            OK = False
            Console.WriteLine("File data error")
          End If
        Catch
          Console.WriteLine("File data error")
        End Try
      Next
      FileIn.Close()
    Catch
      Console.WriteLine("Solution file does not exist")
      OK = False
    End Try
    Return OK
  End Function

  Sub ResetAnswer(PuzzleName As String, Answer() As String)
    Answer(0) = PuzzleName
    Answer(1) = "0"
    Answer(2) = "0"
    For Line As Integer = 3 To 2 * GRID_SIZE * GRID_SIZE - 1
      Answer(Line) = EMPTY_STRING
    Next
  End Sub

  Function TransferPuzzleIntoGrid(PuzzleName As String, PuzzleGrid(,) As Char, Puzzle() As String, Answer() As String) As Boolean
    Dim Row, Column As Integer
    Dim Digit As Char
    Dim OK As Boolean = True
    Try
      Dim Line As Integer = 0
      Dim CellInfo As String = Puzzle(Line)
      While CellInfo <> EMPTY_STRING
        Row = Int32.Parse(CellInfo(0))
        Column = Int32.Parse(CellInfo(1))
        Digit = CellInfo(2)
        PuzzleGrid(Row, Column) = Digit
        Line += 1
        CellInfo = Puzzle(Line)
      End While
      PuzzleGrid(0, 0) = "X"
      ResetAnswer(PuzzleName, Answer)
    Catch
      Console.WriteLine("Error in puzzle file")
      OK = False
    End Try
    Return OK
  End Function

  Sub LoadPuzzle(PuzzleGrid(,) As Char, Puzzle() As String, Answer() As String, Solution() As String)
    ResetDataStructures(PuzzleGrid, Puzzle, Answer, Solution)
    Console.Write("Enter puzzle name to load: ")
    Dim PuzzleName As String = Console.ReadLine()
    Dim OK As Boolean = LoadPuzzleFile(PuzzleName, Puzzle)
    If OK Then
      OK = LoadSolution(PuzzleName, Solution)
    End If
    If OK Then
      OK = TransferPuzzleIntoGrid(PuzzleName, PuzzleGrid, Puzzle, Answer)
    End If
    If Not OK Then
      ResetDataStructures(PuzzleGrid, Puzzle, Answer, Solution)
    End If
  End Sub

  Sub TransferAnswerIntoGrid(PuzzleGrid(,) As Char, Answer() As String)
    Dim Row, Column As Integer
    Dim CellInfo As String
    Dim Digit As Char
    For Line As Integer = 3 To Int32.Parse(Answer(2)) + 2
      CellInfo = Answer(Line)
      Row = Int32.Parse(CellInfo(0))
      Column = Int32.Parse(CellInfo(1))
      Digit = CellInfo(2)
      PuzzleGrid(Row, Column) = Digit
    Next
  End Sub

  Sub LoadPartSolvedPuzzle(PuzzleGrid(,) As Char, Puzzle() As String, Answer() As String, Solution() As String)
    LoadPuzzle(PuzzleGrid, Puzzle, Answer, Solution)
    Try
      Dim PuzzleName As String = Answer(0)
      Dim FileIn As StreamReader = New StreamReader($"{PuzzleName}P.txt")
      Dim CellInfo As String = FileIn.ReadLine()
      If PuzzleName <> CellInfo Then
        Console.WriteLine("Partial solution file is corrupt")
      Else
        Dim Line As Integer = 0
        While CellInfo <> EMPTY_STRING
          Answer(Line) = CellInfo
          Line += 1
          CellInfo = FileIn.ReadLine()
        End While
      End If
      FileIn.Close()
      TransferAnswerIntoGrid(PuzzleGrid, Answer)
    Catch
      Console.WriteLine("Partial solution file does not exist")
    End Try
  End Sub

  Sub DisplayGrid(PuzzleGrid(,) As Char)
    Console.WriteLine()
    Console.WriteLine("   1   2   3   4   5   6   7   8   9  ")
    Console.WriteLine(" |===.===.===|===.===.===|===.===.===|")
    For Row As Integer = 1 To GRID_SIZE
      Console.Write($"{Row}|")
      For Column As Integer = 1 To GRID_SIZE
        If Column Mod 3 = 0 Then
          Console.Write($"{SPACE}{PuzzleGrid(Row, Column)}{SPACE}|")
        Else
          Console.Write($"{SPACE}{PuzzleGrid(Row, Column)}{SPACE}.")
        End If
      Next
      Console.WriteLine()
      If Row Mod 3 = 0 Then
        Console.WriteLine(" |===.===.===|===.===.===|===.===.===|")
      Else
        Console.WriteLine(" |...........|...........|...........|")
      End If
    Next
    Console.WriteLine()
  End Sub

  Sub SolvePuzzle(PuzzleGrid(,) As Char, Puzzle() As String, Answer() As String)
    Dim InputError As Boolean
    Dim Digit As Char
    Dim Row, Column As Integer
    DisplayGrid(PuzzleGrid)
    If PuzzleGrid(0, 0) <> "X" Then
      Console.WriteLine("No puzzle loaded")
    Else
      Console.WriteLine("Enter row column digit: ")
      Console.WriteLine("(Press Enter to stop)")
      Dim CellInfo As String = Console.ReadLine()
      While CellInfo <> EMPTY_STRING
        InputError = False
        If CellInfo.Length <> 3 Then
          InputError = True
        Else
          Digit = CellInfo(2)
          Try
            Row = Int32.Parse(CellInfo(0))
          Catch
            InputError = True
          End Try
          Try
            Column = Int32.Parse(CellInfo(1))
          Catch
            InputError = True
          End Try
          If (Digit < "1") Or (Digit > "9") Then
            InputError = True
          End If
        End If
        If InputError Then
          Console.WriteLine("Invalid input")
        Else
          PuzzleGrid(Row, Column) = Digit
          Answer(2) = (Int32.Parse(Answer(2)) + 1).ToString()
          Answer(Int32.Parse(Answer(2)) + 2) = CellInfo
          DisplayGrid(PuzzleGrid)
        End If
        Console.WriteLine("Enter row column digit: ")
        Console.WriteLine("(Press Enter to stop)")
        CellInfo = Console.ReadLine()
      End While
    End If
  End Sub

  Sub DisplayMenu()
    Console.WriteLine()
    Console.WriteLine("Main Menu")
    Console.WriteLine("=========")
    Console.WriteLine("L - Load new puzzle")
    Console.WriteLine("P - Load partially solved puzzle")
    Console.WriteLine("S - Solve puzzle")
    Console.WriteLine("C - Check solution")
    Console.WriteLine("K - Keep partially solved puzzle")
    Console.WriteLine("X - Exit")
    Console.WriteLine()
  End Sub

  Function GetMenuOption() As Char
    Dim Choice As String = EMPTY_STRING
    While Choice.Length <> 1
      Console.Write("Enter your choice: ")
      Choice = Console.ReadLine()
    End While
    Return Choice(0)
  End Function

  Sub KeepPuzzle(PuzzleGrid(,) As Char, Answer() As String)
    If PuzzleGrid(0, 0) <> "X" Then
      Console.WriteLine("No puzzle loaded")
    Else
      If Int32.Parse(Answer(2)) > 0 Then
        Dim PuzzleName As String = Answer(0)
        Dim FileOut As StreamWriter = New StreamWriter($"{PuzzleName}P.txt")
        For Line As Integer = 0 To (Int32.Parse(Answer(2)) + 2)
          FileOut.WriteLine(Answer(Line))
        Next
        FileOut.Close()
      Else
        Console.WriteLine("No answers to keep")
      End If
    End If
  End Sub

  Sub CheckSolution(PuzzleGrid(,) As Char, Answer() As String, Solution() As String, ByRef ErrorCount As Integer, ByRef Solved As Boolean)
    Dim Entry As Char
    ErrorCount = 0
    Solved = False
    Dim Correct As Boolean = True
    Dim Incomplete As Boolean = False
    For Row As Integer = 1 To GRID_SIZE
      For Column As Integer = 1 To GRID_SIZE
        Entry = PuzzleGrid(Row, Column)
        If Entry = SPACE Then
          Incomplete = True
        End If
        If Not ((Entry = Solution(Row)(Column - 1)) Or (Entry = SPACE)) Then
          Correct = False
          ErrorCount += 1
          Console.WriteLine($"You have made an error in row {Row} column {Column}")
        End If
      Next
    Next
    If Not Correct Then
      Console.WriteLine($"You have made {ErrorCount} error(s)")
    ElseIf Incomplete Then
      Console.WriteLine("So far so good, carry on")
    ElseIf Correct Then
      Solved = True
    End If
  End Sub

  Sub CalculateScore(Answer() As String, ErrorCount As Integer)
    Answer(1) = (Int32.Parse(Answer(1)) - ErrorCount).ToString()
  End Sub

  Sub DisplayResults(Answer() As String)
    If Int32.Parse(Answer(2)) > 0 Then
      Console.WriteLine($"Your score is {Answer(1)}")
      Console.WriteLine($"Your solution for {Answer(0)} was: ")
      For Line As Integer = 3 To Int32.Parse(Answer(2)) + 2
        Console.WriteLine(Answer(Line))
      Next
    Else
      Console.WriteLine("You didn't make a start")
    End If
  End Sub

  Sub NumberPuzzle()
    Dim Puzzle(GRID_SIZE * GRID_SIZE - 1) As String
    Dim PuzzleGrid(GRID_SIZE, GRID_SIZE) As Char
    Dim Solution(GRID_SIZE) As String
    Dim Answer(2 * GRID_SIZE * GRID_SIZE - 1) As String
    Dim Solved As Boolean
    Dim MenuOption As Char
    Dim ErrorCount, ResponseNumber As Integer
    Dim Rnd As Random = New Random()
    Dim Finished As Boolean = False
    ResetDataStructures(PuzzleGrid, Puzzle, Answer, Solution)
    While Not Finished
      DisplayMenu()
      MenuOption = GetMenuOption()
      Select Case MenuOption
        Case "L"
          LoadPuzzle(PuzzleGrid, Puzzle, Answer, Solution)
        Case "P"
          LoadPartSolvedPuzzle(PuzzleGrid, Puzzle, Answer, Solution)
        Case "K"
          KeepPuzzle(PuzzleGrid, Answer)
        Case "C"
          If PuzzleGrid(0, 0) <> "X" Then
            Console.WriteLine("No puzzle loaded")
          Else
            If CInt(Answer(2)) > 0 Then
              CheckSolution(PuzzleGrid, Answer, Solution, ErrorCount, Solved)
              CalculateScore(Answer, ErrorCount)
              If Solved Then
                Console.WriteLine("You have successfully solved the puzzle")
                Finished = True
              Else
                Console.WriteLine($"Your score so far is {Answer(1)}")
              End If
            Else
              Console.WriteLine("No answers to check")
            End If
          End If
        Case "S"
          SolvePuzzle(PuzzleGrid, Puzzle, Answer)
        Case "X"
          Finished = True
        Case Else
          ResponseNumber = Rnd.Next(1, 6)
          If ResponseNumber = 1 Then
            Console.WriteLine("Invalid menu option. Try again")
          ElseIf ResponseNumber = 2 Then
            Console.WriteLine("You did not choose a valid menu option. Try again")
          ElseIf ResponseNumber = 3 Then
            Console.WriteLine("Your menu option is not valid. Try again")
          ElseIf ResponseNumber = 4 Then
            Console.WriteLine("Only L, P, S, C, K or X are valid menu options. Try again")
          ElseIf ResponseNumber = 5 Then
            Console.WriteLine("Try one of L, P, S, C, K or X ")
          End If
      End Select
    End While
    If Answer(2) <> EMPTY_STRING Then
      DisplayResults(Answer)
    End If
  End Sub

  Sub Main()
    NumberPuzzle()
    Console.ReadLine()
  End Sub

End Module
