// Skeleton Program for the AQA AS Summer 2022 examination
// this code should be used in conjunction with the Preliminary Material
// written by the AQA Programmer Team
// developed in a Free Pascal environment

// NOTE Debugger should be switched off

// Version number: 0.0.0

{$APPTYPE CONSOLE} {$R+}

uses SysUtils, StrUtils;

const
  EMPTY_STRING = '';
  SPACE = ' ';
  GRID_SIZE = 9;

type
  TPuzzle = array[0 .. (GRID_SIZE * GRID_SIZE - 1)] of string;
  TPuzzleGrid = array[0 .. GRID_SIZE, 0 .. GRID_SIZE] of char;
  TSolution = array[0 .. GRID_SIZE] of string;
  TAnswer = array[0 .. (2 * GRID_SIZE * GRID_SIZE - 1)] of string;

procedure ResetDataStructures(var PuzzleGrid: TPuzzleGrid; var Puzzle: TPuzzle; var Answer: TAnswer; var Solution: TSolution);
var
  Line, Row, Column: integer;
begin
  for Line := 0 to GRID_SIZE * GRID_SIZE - 1 do
    Puzzle[Line] := EMPTY_STRING;
  for Row := 0 to GRID_SIZE  do
    for Column := 0 to GRID_SIZE do
      PuzzleGrid[Row, Column] := SPACE;
  for Line := 0 to GRID_SIZE do
    Solution[Line] := EMPTY_STRING;
  for Line := 0 to 2 * GRID_SIZE * GRID_SIZE - 1 do
    Answer[Line] := EMPTY_STRING;
end;

function LoadPuzzleFile(PuzzleName: string; var Puzzle: TPuzzle): boolean;
var
  Line: integer;
  FileIn: text;
  CellInfo: string;
  OK: boolean;
begin
  try
    Line := 0;
    assignFile(FileIn, PuzzleName + '.txt');
    reset(FileIn);
    while not EOF(FileIn) do
    begin
      readln(FileIn, CellInfo);
      Puzzle[Line] := CellInfo;
      inc(Line);
    end;
    close(FileIn);
    if Line = 0 then
    begin
      writeln('Puzzle file empty');
      OK := False;
    end
    else
      OK := True;
  except
    writeln('Puzzle file does not exist');
    OK := False;
  end;
  LoadPuzzleFile := OK;
end;

function LoadSolution(PuzzleName: string; var Solution: TSolution): boolean;
var
  FileIn: text;
  Line: integer;
  OK: boolean;
begin
  OK := True;
  try
    assignFile(FileIn, PuzzleName + 'S.txt');
    reset(FileIn);
    for Line := 1 to GRID_SIZE do
    begin
      readln(FileIn, Solution[Line]);
      if length(Solution[Line]) <> GRID_SIZE then
      begin
        OK := False;
        writeln('File data error');
      end;
    end;
    close(FileIn);
  except
    writeln('Solution file does not exist');
    OK := False;
  end;
  LoadSolution := OK;
end;

procedure ResetAnswer(PuzzleName: string; var Answer: TAnswer);
var
  Line: integer;
begin
  Answer[0] := PuzzleName;
  Answer[1] := '0';
  Answer[2] := '0';
  for Line := 3 to 2 * GRID_SIZE * GRID_SIZE - 1 do
    Answer[Line] := EMPTY_STRING;
end;

function TransferPuzzleIntoGrid(PuzzleName: string; var PuzzleGrid: TPuzzleGrid; Puzzle: TPuzzle; var Answer: TAnswer): boolean;
var
  Line, Row, Column: integer;
  CellInfo: string;
  Digit: char;
  OK: boolean;
begin
  OK := True;
  try
    Line := 0;
    CellInfo := Puzzle[Line];
    while CellInfo <> EMPTY_STRING do
    begin
      Row := strToInt(CellInfo[1]);
      Column := strToInt(CellInfo[2]);
      Digit := CellInfo[3];
      PuzzleGrid[Row, Column] := Digit;
      inc(Line);
      CellInfo := Puzzle[Line];
    end;
    PuzzleGrid[0, 0] := 'X';
    ResetAnswer(PuzzleName, Answer);
  except
    writeln('Error in puzzle file');
    OK := False;
  end;
  TransferPuzzleIntoGrid := OK;
end;

procedure LoadPuzzle(var PuzzleGrid: TPuzzleGrid; var Puzzle: TPuzzle; var Answer: TAnswer; var Solution: TSolution);
var
  PuzzleName: string;
  OK: boolean;
begin
  ResetDataStructures(PuzzleGrid, Puzzle, Answer, Solution);
  write('Enter puzzle name to load: ');
  readln(PuzzleName);
  OK := LoadPuzzleFile(PuzzleName, Puzzle);
  if OK then
    OK := LoadSolution(PuzzleName, Solution);
  if OK then
    OK := TransferPuzzleIntoGrid(PuzzleName, PuzzleGrid, Puzzle, Answer);
  if not OK then
    ResetDataStructures(PuzzleGrid, Puzzle, Answer, Solution);
end;

procedure TransferAnswerIntoGrid(var PuzzleGrid: TPuzzleGrid; Answer: TAnswer);
var
  Line, Row, Column: integer;
  CellInfo: string;
  Digit: char;
begin
  for Line := 3 to strToInt(Answer[2]) + 2 do
  begin
    CellInfo := Answer[Line];
    Row := strToInt(CellInfo[1]);
    Column := strToInt(CellInfo[2]);
    Digit := CellInfo[3];
    PuzzleGrid[Row, Column] := Digit;
  end;
end;

procedure LoadPartSolvedPuzzle(var PuzzleGrid: TPuzzleGrid; var Puzzle: TPuzzle; var Answer: TAnswer; var Solution: TSolution);
var
  PuzzleName, CellInfo: string;
  FileIn: text;
  Line: integer;
begin
  LoadPuzzle(PuzzleGrid, Puzzle, Answer, Solution);
  try
    PuzzleName := Answer[0];
    assignFile(FileIn, PuzzleName + 'P.txt');
    reset(FileIn);
    readln(FileIn, CellInfo);
    if PuzzleName <> CellInfo then
      writeln('Partial solution file is corrupt')
    else
    begin
      Line := 0;
      while CellInfo <> EMPTY_STRING do
      begin
        Answer[Line] := CellInfo;
        inc(Line);
        readln(FileIn, CellInfo);
      end;
    end;
    closeFile(FileIn);
    TransferAnswerIntoGrid(PuzzleGrid, Answer);
  except
    writeln('Partial solution file does not exist');
  end;
end;

procedure DisplayGrid(PuzzleGrid: TPuzzleGrid);
var
  Row, Column: integer;
begin
  writeln;
  writeln('   1   2   3   4   5   6   7   8   9  ');
  writeln(' |===.===.===|===.===.===|===.===.===|');
  for Row := 1 to GRID_SIZE do
  begin
    write(Row, '|');
    for Column := 1 to GRID_SIZE do
      if Column mod 3 = 0 then
        write(SPACE + PuzzleGrid[Row, Column] + SPACE, '|')
      else
        write(SPACE + PuzzleGrid[Row, Column] + SPACE, '.');
    writeln;
    if Row mod 3 = 0 then
      writeln(' |===.===.===|===.===.===|===.===.===|')
    else
      writeln(' |...........|...........|...........|');
  end;
  writeln;
end;

procedure SolvePuzzle(var PuzzleGrid: TPuzzleGrid; Puzzle: TPuzzle; var Answer: TAnswer);
var
  CellInfo: string;
  InputError: boolean;
  Digit: char;
  Row, Column: integer;
begin
  DisplayGrid(PuzzleGrid);
  if PuzzleGrid[0, 0] <> 'X' then
    writeln('No puzzle loaded')
  else
  begin
    writeln('Enter row column digit: ');
    writeln('(Press Enter to stop)');
    readln(CellInfo);
    while CellInfo <> EMPTY_STRING do
    begin
      InputError := False;
      if length(CellInfo) <> 3 then
        InputError := True
      else
      begin
        Digit := CellInfo[3];
        try
          Row := strToInt(CellInfo[1]);
        except
          InputError := True;
        end;
        try
          Column := strToInt(CellInfo[2]);
        except
          InputError := True;
        end;
        if (Digit < '1') or (Digit > '9') then
          InputError := True;
      end;
      if InputError then
        writeln('Invalid input')
      else
      begin
        PuzzleGrid[Row, Column] := Digit;
        Answer[2] := intToStr(strToInt(Answer[2]) + 1);
        Answer[strToInt(Answer[2]) + 2] := CellInfo;
        DisplayGrid(PuzzleGrid);
      end;
      writeln('Enter row column digit: ');
      writeln('(Press Enter to stop)');
      readln(CellInfo);
    end;
  end;
end;

procedure DisplayMenu();
begin
  writeln;
  writeln('Main Menu');
  writeln('=========');
  writeln('L - Load new puzzle');
  writeln('P - Load partially solved puzzle');
  writeln('S - Solve puzzle');
  writeln('C - Check solution');
  writeln('K - Keep partially solved puzzle');
  writeln('X - Exit');
  writeln;
end;

function GetMenuOption(): char;
var
  Choice: string;
begin
  Choice := EMPTY_STRING;
  while length(Choice) <> 1 do
  begin
    write('Enter your choice: ');
    readln(Choice);
  end;
  GetMenuOption := Choice[1];
end;

procedure KeepPuzzle(PuzzleGrid: TPuzzleGrid; Answer: TAnswer);
var
  PuzzleName: string;
  FileOut: text;
  Line: integer;
begin
  if PuzzleGrid[0, 0] <> 'X' then
    writeln('No puzzle loaded')
  else
    if strToInt(Answer[2]) > 0 then
    begin
      PuzzleName := Answer[0];
      assignFile(FileOut, PuzzleName + 'P.txt');
      rewrite(FileOut);
      for Line := 0 to strToInt(Answer[2]) + 2 do
        writeln(FileOut, Answer[Line]);
      closeFile(FileOut);
    end
    else
      writeln('No answers to keep');
end;

procedure CheckSolution(PuzzleGrid: TPuzzleGrid; Answer: TAnswer; Solution: TSolution; var ErrorCount: integer; var Solved: boolean);
var
  Correct, Incomplete: boolean;
  Row, Column: integer;
  Entry: char;
begin
  ErrorCount := 0;
  Solved := False;
  Correct := True;
  Incomplete := False;
  for Row := 1 to GRID_SIZE do
    for Column := 1 to GRID_SIZE do
    begin
      Entry := PuzzleGrid[Row, Column];
      if Entry = SPACE then
        Incomplete := True;
      if not ((Entry = Solution[Row][Column]) or (Entry = SPACE)) then
      begin
        Correct := False;
        inc(ErrorCount);
        writeln('You have made an error in row ', Row, ' column ', Column);
      end;
    end;
  if not Correct then
    writeln('You have made ', ErrorCount, ' error(s)')
  else
  if Incomplete then
    writeln('So far so good, carry on')
  else
  if Correct then
    Solved := True;
end;

procedure CalculateScore(var Answer: TAnswer; ErrorCount: integer);
begin
  Answer[1] := intToStr(strToInt(Answer[1]) - ErrorCount);
end;

procedure DisplayResults(Answer: TAnswer);
var
  Line: integer;
begin
  if strToInt(Answer[2]) > 0 then
  begin
    writeln('Your score is ', Answer[1]);
    writeln('Your solution for ', Answer[0], ' was: ');
    for Line := 3 to strToInt(Answer[2]) + 2 do
      writeln(Answer[Line]);
  end
  else
    writeln('You didn''t make a start');
end;

procedure NumberPuzzle();
var
  Puzzle: TPuzzle;
  PuzzleGrid: TPuzzleGrid;
  Solution: TSolution;
  Answer: TAnswer;
  Finished, Solved: Boolean;
  MenuOption: char;
  ErrorCount, ResponseNumber: integer;
begin
  Finished := False;
  ResetDataStructures(PuzzleGrid, Puzzle, Answer, Solution);
  while not Finished do
  begin
    DisplayMenu();
    MenuOption := GetMenuOption();
    case MenuOption of
      'L':
        LoadPuzzle(PuzzleGrid, Puzzle, Answer, Solution);
      'P':
        LoadPartSolvedPuzzle(PuzzleGrid, Puzzle, Answer, Solution);
      'K':
        KeepPuzzle(PuzzleGrid, Answer);
      'C':
        if PuzzleGrid[0, 0] <> 'X' then
          writeln('No puzzle loaded')
        else
        begin
          if strToInt(Answer[2]) > 0 then
          begin
            CheckSolution(PuzzleGrid, Answer, Solution, ErrorCount, Solved);
            CalculateScore(Answer, ErrorCount);
            if Solved then
            begin
              writeln('You have successfully solved the puzzle');
              Finished := True;
            end
            else
              writeln('Your score so far is ', Answer[1]);
          end
          else
            writeln('No answers to check');
        end;
      'S':
        SolvePuzzle(PuzzleGrid, Puzzle, Answer);
      'X':
        Finished := True;
      else
      begin
        ResponseNumber := random(5) + 1;
        case ResponseNumber of
          1:
            writeln('Invalid menu option. Try again');
          2:
            writeln('You did not choose a valid menu option. Try again');
          3:
            writeln('Your menu option is not valid. Try again');
          4:
            writeln('Only L, P, S, C, K or X are valid menu options. Try again');
          5:
            writeln('Try one of L, P, S, C, K or X ');
        end;
      end;
    end;
  end;
  if Answer[2] <> EMPTY_STRING then
    DisplayResults(Answer);
end;

begin
  NumberPuzzle();
  readln;
end.
