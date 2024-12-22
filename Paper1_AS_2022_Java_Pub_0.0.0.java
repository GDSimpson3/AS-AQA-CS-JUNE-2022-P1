//  Skeleton Program for the AQA AS Summer 2022 examination
//  this code should be used in conjunction with the Preliminary Material
//  written by the AQA Programmer Team
//  developed in the NetBeans IDE 8.1 environment
//  Version number: 0.0.0

package aqa.numberpuzzle;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Random;

public class NumberPuzzle {

  final String EMPTY_STRING = "";
  final char SPACE = ' ';
  final int GRID_SIZE = 9;
  
  class IntReturn
  {
      public int value = 0;
  }
  
  class BooleanReturn
  {
      public boolean value = false;
  }

  void resetDataStructures(char[][] puzzleGrid, String[] puzzle, String[] answer, String[] solution) {
    for (int line = 0; line < GRID_SIZE * GRID_SIZE; line++) {
      puzzle[line] = EMPTY_STRING;
    }
    for (int row = 0; row <= GRID_SIZE; row++) {
      for (int column = 0; column <= GRID_SIZE; column++) {
        puzzleGrid[row][column] = SPACE;
      }
    }
    for (int line = 0; line <= GRID_SIZE; line++) {
      solution[line] = EMPTY_STRING;
    }
    for (int line = 0; line < 2 * GRID_SIZE * GRID_SIZE - 1; line++) {
      answer[line] = EMPTY_STRING;
    }
  }

  boolean loadPuzzleFile(String puzzleName, String[] puzzle) {
    int line;
    BufferedReader fileIn;
    String cellInfo;
    boolean ok;
    try {
      line = 0;
      fileIn = new BufferedReader(new FileReader(puzzleName + ".txt"));
      cellInfo = fileIn.readLine();
      while (cellInfo != null) {
        puzzle[line] = cellInfo;
        cellInfo = fileIn.readLine();
        line += 1;
      }
      fileIn.close();
      if (line == 0) {
        Console.writeLine("Puzzle file empty");
        ok = false;
      } else {
        ok = true;
      }
    } catch (Exception ex) {
      Console.writeLine("Puzzle file does not exist");
      ok = false;
    }
    return ok;
  }

  boolean loadSolution(String puzzleName, String[] solution) {
    BufferedReader fileIn;
    int line;
    boolean ok;
    ok = true;
    try {
      fileIn = new BufferedReader(new FileReader(puzzleName + "S.txt"));
      for (line = 1; line <= GRID_SIZE; line++) {
        solution[line] = SPACE + fileIn.readLine();
        solution[line] = solution[line].substring(0, solution[line].length()-1);
        if (solution[line].length() != GRID_SIZE) {
          ok = false;
          Console.writeLine("File data error");
        }
      }
      fileIn.close();
    } catch (Exception ex) {
      Console.writeLine("Solution file does not exist");
      ok = false;
    }
    return ok;
  }

  void resetAnswer(String puzzleName, String[] answer) {
    int line;
    answer[0] = puzzleName;
    answer[1] = "0";
    answer[2] = "0";
    for (line = 3; line < 2 * GRID_SIZE * GRID_SIZE - 1; line++) {
      answer[line] = EMPTY_STRING;
    }
  }

  boolean transferPuzzleIntoGrid(String puzzleName, char[][] puzzleGrid, String[] puzzle, String[] answer) {
    int line, row, column;
    String cellInfo;
    char digit;
    boolean ok;
    ok = true;
    try {
      line = 0;
      cellInfo = puzzle[line];
      while (!cellInfo.equals(EMPTY_STRING)) {
        row = Integer.parseInt(Character.toString(cellInfo.charAt(0)));
        column = Integer.parseInt(Character.toString(cellInfo.charAt(1)));
        digit = cellInfo.charAt(2);
        puzzleGrid[row][column] = digit;
        line += 1;
        cellInfo = puzzle[line];
      }
      puzzleGrid[0][0] = 'X';
      resetAnswer(puzzleName, answer);
    } catch (Exception ex) {
      Console.writeLine("Error in puzzle file");
      ok = false;
    }
    return ok;
  }

  void loadPuzzle(char[][] puzzleGrid, String[] puzzle, String[] answer, String[] solution) {
    String puzzleName;
    boolean ok;
    resetDataStructures(puzzleGrid, puzzle, answer, solution);
    Console.write("Enter puzzle name to load: ");
    puzzleName = Console.readLine();
    ok = loadPuzzleFile(puzzleName, puzzle);
    if (ok) {
      ok = loadSolution(puzzleName, solution);
    }
    if (ok) {
      ok = transferPuzzleIntoGrid(puzzleName, puzzleGrid, puzzle, answer);
    }
    if (!ok) {
      resetDataStructures(puzzleGrid, puzzle, answer, solution);
    }
  }

  void transferAnswerIntoGrid(char[][] puzzleGrid, String[] answer) {
    int line, row, column;
    String cellInfo;
    char digit;
    for (line = 3; line <= Integer.parseInt(answer[2]) + 2; line++) {
      cellInfo = answer[line];
      row = Integer.parseInt(Character.toString(cellInfo.charAt(0)));
      column = Integer.parseInt(Character.toString(cellInfo.charAt(1)));
      digit = cellInfo.charAt(2);
      puzzleGrid[row][column] = digit;
    }
  }

  void loadPartSolvedPuzzle(char[][] puzzleGrid, String[] puzzle, String[] answer, String[] solution) {
    String puzzleName, cellInfo;
    BufferedReader fileIn;
    int line;
    loadPuzzle(puzzleGrid, puzzle, answer, solution);
    puzzleName = answer[0];
    try {
      fileIn = new BufferedReader(new FileReader(puzzleName + "P.txt"));
      cellInfo = fileIn.readLine();
      if (!puzzleName.equals(cellInfo)) {
        Console.writeLine("Partial solution file is corrupt");
      } else {
        line = 0;
        while (cellInfo != null) {
          answer[line] = cellInfo;
          line += 1;
          cellInfo = fileIn.readLine();
        }
      }
      fileIn.close();
      transferAnswerIntoGrid(puzzleGrid, answer);
    } catch (Exception ex) {
      Console.writeLine("Partial solution file does not exist");
    }
  }

  void displayGrid(char[][] puzzleGrid) {
    int row, column;
    Console.writeLine();
    Console.writeLine("   1   2   3   4   5   6   7   8   9  ");
    Console.writeLine(" |===.===.===|===.===.===|===.===.===|");
    for (row = 1; row <= GRID_SIZE; row++) {
      Console.write(row + "|");
      for (column = 1; column <= GRID_SIZE; column++) {
        Console.write(SPACE + Character.toString(puzzleGrid[row][column]) + SPACE);
        if (column % 3 == 0) {
          Console.write("|");
        } else {
          Console.write(".");
        }
      }
      Console.writeLine();
      if (row % 3 == 0) {
        Console.writeLine(" |===.===.===|===.===.===|===.===.===|");
      } else {
        Console.writeLine(" |...........|...........|...........|");
      }
    }
    Console.writeLine();
  }

  void solvePuzzle(char[][] puzzleGrid, String[] puzzle, String[] answer) {
    String cellInfo;
    boolean inputError;
    char digit = ' ';
    int row = 0, column = 0;
    displayGrid(puzzleGrid);
    if (puzzleGrid[0][0] != 'X') {
      Console.writeLine("No puzzle loaded");
    } else {
      Console.writeLine("Enter row column digit: ");
      Console.writeLine("(Press Enter to stop)");
      cellInfo = Console.readLine();
      while (!cellInfo.equals(EMPTY_STRING)) {
        inputError = false;
        if (cellInfo.length() != 3) {
          inputError = true;
        } else {
          digit = cellInfo.charAt(2);
          try {
            row = Integer.parseInt(Character.toString(cellInfo.charAt(0)));
          } catch (Exception ex) {
            inputError = true;
          }
          try {
            column = Integer.parseInt(Character.toString(cellInfo.charAt(1)));
          } catch (Exception ex) {
            inputError = true;
          }
          if ((digit < '1') || (digit > '9')) {
            inputError = true;
          }
        }
        if (inputError) {
          Console.writeLine("Invalid input");
        } else {
          puzzleGrid[row][column] = digit;
          answer[2] = Integer.toString(Integer.parseInt(answer[2]) + 1);
          answer[Integer.parseInt(answer[2]) + 2] = cellInfo;
          displayGrid(puzzleGrid);
        }
        Console.writeLine("Enter row column digit: ");
        Console.writeLine("(Press Enter to stop)");
        cellInfo = Console.readLine();
      }
    }
  }

  void displayMenu() {
    Console.writeLine();
    Console.writeLine("Main Menu");
    Console.writeLine("=========");
    Console.writeLine("L - Load new puzzle");
    Console.writeLine("P - Load partially solved puzzle");
    Console.writeLine("S - Solve puzzle");
    Console.writeLine("C - Check solution");
    Console.writeLine("K - Keep partially solved puzzle");
    Console.writeLine("X - Exit");
    Console.writeLine();
  }

  char getMenuOption() {
    String choice;
    choice = EMPTY_STRING;
    while (choice.length() != 1) {
      Console.write("Enter your choice: ");
      choice = Console.readLine();
    }
    return choice.charAt(0);
  }

  void keepPuzzle(char[][] puzzleGrid, String[] answer) {
    String puzzleName;
    BufferedWriter fileOut;
    int line;
    if (puzzleGrid[0][0] != 'X') {
      Console.writeLine("No puzzle loaded");
    } else {
      if (Integer.parseInt(answer[2]) > 0) {
        puzzleName = answer[0];
        try {
          fileOut = new BufferedWriter(new FileWriter(puzzleName + "P.txt", false));
          for (line = 0; line <= (Integer.parseInt(answer[2]) + 2); line++) {
            fileOut.write(answer[line]);
            fileOut.newLine();
          }
          fileOut.close();
        } catch (Exception ex) {
          Console.writeLine("Cannot write to file");
        }
      } else {
        Console.writeLine("No answers to keep");
      }
    }
  }

  void checkSolution(char[][] puzzleGrid, String[] answer, String[] solution, IntReturn errorCount, BooleanReturn solved) {
    boolean correct, incomplete;
    int row, column;
    char entry;
    errorCount.value = 0;
    solved.value = false;
    correct = true;
    incomplete = false;
    for (row = 1; row <= GRID_SIZE; row++) {
      for (column = 1; column <= GRID_SIZE; column++) {
        entry = puzzleGrid[row][column];
        if (entry == SPACE) {
          incomplete = true;
        }
        if (!((entry == solution[row].charAt(column)) || (entry == SPACE))) {
          correct = false;
          errorCount.value += 1;
          Console.writeLine("You have made an error in row " + row + " column " + column);
        }
      }
    }
    if (!correct) {
      Console.writeLine("You have made " + errorCount.value + " error(s)");
    } else if (incomplete) {
        Console.writeLine("So far so good, carry on");
    } else if (correct) {
        solved.value = true;
    }
  }

  void calculateScore(String[] answer, int errorCount) {
    answer[1] = Integer.toString(Integer.parseInt(answer[1]) - errorCount);
  }

  void displayResults(String[] answer) {
    int line;
    if (Integer.parseInt(answer[2]) > 0) {
      Console.writeLine("Your score is " + answer[1]);
      Console.writeLine("Your solution for " + answer[0] + " was: ");
      for (line = 3; line < Integer.parseInt(answer[2]) + 3; line++) {
        Console.writeLine(answer[line]);
      }
    } else {
      Console.writeLine("You didn''t make a start");
    }
  }

  public NumberPuzzle() {
    String[] puzzle = new String[GRID_SIZE * GRID_SIZE];
    char[][] puzzleGrid = new char[GRID_SIZE + 1][GRID_SIZE + 1];
    String[] solution = new String[GRID_SIZE + 1];
    String[] answer = new String[2 * GRID_SIZE * GRID_SIZE - 1];
    boolean finished;
    char menuOption;
    int responseNumber;
    BooleanReturn solved = new BooleanReturn();
    IntReturn errorCount = new IntReturn();
    finished = false;
    resetDataStructures(puzzleGrid, puzzle, answer, solution);
    while (!finished) {
      displayMenu();
      menuOption = getMenuOption();
      switch (menuOption) {
        case 'L':
          loadPuzzle(puzzleGrid, puzzle, answer, solution);
          break;
        case 'P':
          loadPartSolvedPuzzle(puzzleGrid, puzzle, answer, solution);
          break;
        case 'K':
          keepPuzzle(puzzleGrid, answer);
          break;
        case 'C':
          if (puzzleGrid[0][0] != 'X') {
            Console.writeLine("No puzzle loaded");
          } else {
            if (Integer.parseInt(answer[2]) > 0) {
              checkSolution(puzzleGrid, answer, solution,errorCount,solved);
              calculateScore(answer, errorCount.value);
              if (solved.value) {
                Console.writeLine("You have successfully solved the puzzle");
                finished = true;
              } else {
                Console.writeLine("Your score so far is " + answer[1]);
              }
            } else {
              Console.writeLine("No answers to check");
            }
          }
          break;
        case 'S':
          solvePuzzle(puzzleGrid, puzzle, answer);
          break;
        case 'X':
          finished = true;
          break;
        default:
          Random rnd = new Random();
          responseNumber = rnd.nextInt(5) + 1;
          switch (responseNumber) {
            case 1:
              Console.writeLine("Invalid menu option. Try again");
              break;
            case 2:
              Console.writeLine("You did not choose a valid menu option. Try again");
              break;
            case 3:
              Console.writeLine("Your menu option is not valid. Try again");
              break;
            case 4:
              Console.writeLine("Only L, P, S, C, K or X are valid menu options. Try again");
              break;
            case 5:
              Console.writeLine("Try one of L, P, S, C, K or X ");
              break;
          }
      }
    }
    if (!answer[2].equals(EMPTY_STRING)) {
      displayResults(answer);
    }
  }

  public static void main(String[] args) {
    new NumberPuzzle();
  }

}
