Feature: AST parser - Delegates

    Scenario Outline: Parser can detect various ways to interact with delegates/events
        Given we parse 'Parsing/Delegates/TestDelegates.cs'

        Then there are 3 classes
        And there is a class named '<ClassName>'
        And The class '<ClassName>' has <ClassDepCount> dependencies
        And The class '<ClassName>' uses '<DepName>' <DepCount> times
        And The class '<ClassName>' uses '<DepName>' as <DepType> <TypeCount> times

        Examples:
          | ClassName       | ClassDepCount | DepName          | DepCount | DepType                  | TypeCount |
          | DelegateExample | 2             | string           | 1        | ParameterInjection       | 1         |
          | DelegateExample | 2             | DelegateExample  | 1        | DelegateDeclaration      | 1         |
          | MyClass         | 3             | string           | 3        | ParameterInjection       | 3         |
          | MyClass         | 3             | Console          | 2        | StaticInvocation         | 2         |
          | MyClass         | 3             | DelegateExample  | 3        | VariableDeclaration      | 1         |
          | MyClass         | 3             | DelegateExample  | 3        | InstanceInvocation       | 1         |
          | MyClass         | 3             | DelegateExample  | 3        | DelegateInvocation       | 1         |
          | DelegateTest    | 5             | MyClass          | 3        | DirectInstantiation      | 1         |
          | DelegateTest    | 5             | MyClass          | 3        | VariableDeclaration      | 1         |
          | DelegateTest    | 5             | MyClass          | 3        | InstanceInvocation       | 1         |
          | DelegateTest    | 5             | DelegateExample  | 14       | VariableDeclaration      | 2         |
          | DelegateTest    | 5             | DelegateExample  | 14       | InstanceInvocation       | 2         |
          | DelegateTest    | 5             | DelegateExample  | 14       | DelegateInvocation       | 2         |
          | DelegateTest    | 5             | DelegateExample  | 14       | SubscribesToDelegate     | 6         |
          | DelegateTest    | 5             | DelegateExample  | 14       | UnsubscribesFromDelegate | 2         |
          | DelegateTest    | 5             | DelegateExample? | 2        | SubscribesToEvent        | 1         |
          | DelegateTest    | 5             | DelegateExample? | 2        | UnsubscribesFromEvent    | 1         |
          | DelegateTest    | 5             | string           | 4        | ParameterInjection       | 4         |
          | DelegateTest    | 5             | Console          | 5        | StaticInvocation         | 5         |