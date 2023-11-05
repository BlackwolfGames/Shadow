Feature: AST parser - Oddities

    Scenario Outline: Parser can detect various ways to interact with generics
        Given we parse 'Parsing/Oddities/TestNestingAndAnnotations.cs'

        Then there are 2 classes
        And there is a class named '<ClassName>'
        And The class '<ClassName>' has <ClassDepCount> dependencies
        And The class '<ClassName>' uses '<DepName>' <DepCount> times
        And The class '<ClassName>' uses '<DepName>' as <DepType> <TypeCount> times

        Examples:
          | ClassName                 | ClassDepCount | DepName          | DepCount | DepType            | TypeCount |
          | TestNestingAndAnnotations | 3             | EmbeddedClass    | 1        | Nesting            | 1         |
          | TestNestingAndAnnotations | 3             | TestTagAttribute | 2        | Attribute          | 2         |
          | TestNestingAndAnnotations | 3             | string           | 1        | ParameterInjection | 1         |
          | TestTagAttribute          | 1             | Attribute        | 1        | Extension          | 1         |