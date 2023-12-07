Feature: AST parser - Exceptions

 Scenario Outline: Parser can detect various ways to interact with exceptions
  Given we parse 'Parsing/Exceptions/TestExceptions.cs'

  Then there are 2 classes
  And there is a class named '<ClassName>'
  And The class '<ClassName>' has <ClassDepCount> dependencies
  And The class '<ClassName>' uses '<DepName>' <DepCount> times
  And The class '<ClassName>' uses '<DepName>' as <DepType> <TypeCount> times

  Examples:
    | ClassName            | ClassDepCount | DepName               | DepCount | DepType             | TypeCount |
    | NegativeAgeException | 2             | Exception             | 1        | Extension           | 1         |
    | NegativeAgeException | 2             | string                | 1        | ParameterInjection  | 1         |
    | TestExceptions       | 7             | int                   | 3        | ParameterInjection  | 3         |
    | TestExceptions       | 7             | NegativeAgeException  | 2        | DirectInstantiation | 1         |
    | TestExceptions       | 7             | NegativeAgeException  | 2        | ThrownException     | 1         |
    | TestExceptions       | 7             | ArgumentException     | 2        | DirectInstantiation | 1         |
    | TestExceptions       | 7             | ArgumentException     | 2        | ThrownException     | 1         |
    | TestExceptions       | 7             | TestExceptions        | 1        | StaticInvocation    | 1         |
    | TestExceptions       | 7             | Exception             | 1        | CaughtException     | 1         |
    | TestExceptions       | 7             | Hidden                | 2        | ThrownException     | 1         |
    | TestExceptions       | 7             | Hidden                | 2        | CaughtException     | 1         |
    | TestExceptions       | 7             | DivideByZeroException | 1        | CaughtException     | 1         |