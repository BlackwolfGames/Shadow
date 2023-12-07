Feature: AST parser - Properties

 Scenario Outline: Parser can detect various types of property
  Given we parse 'Parsing/Calls/TestProperties.cs'

  Then there is 1 class
  And there is a class named '<ClassName>'
  And The class '<ClassName>' has <ClassDepCount> dependencies
  And The class '<ClassName>' uses '<DepName>' <DepCount> times
  And The class '<ClassName>' uses '<DepName>' as <DepType> <TypeCount> times

  Examples:
    | ClassName      | ClassDepCount | DepName | DepCount | DepType  | TypeCount |
    | TestProperties | 2             | int     | 1        | Property | 1         |
    | TestProperties | 2             | string  | 1        | Property | 1         |