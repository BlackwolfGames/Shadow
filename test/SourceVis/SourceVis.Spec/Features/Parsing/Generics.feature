Feature: AST parser - Generics

    Scenario Outline: Parser can detect various ways to interact with generics
        Given we parse 'Parsing/Generics/TestGenerics.cs'

        Then there are 4 classes
        And there is a class named '<ClassName>'
        And The class '<ClassName>' has <ClassDepCount> dependencies
        And The class '<ClassName>' uses '<DepName>' <DepCount> times
        And The class '<ClassName>' uses '<DepName>' as <DepType> <TypeCount> times

        Examples:
          | ClassName            | ClassDepCount | DepName                   | DepCount | DepType             | TypeCount |
          | IGenericInterface<T> | 1             | T                         | 3        | ParameterInjection  | 1         |
          | IGenericInterface<T> | 1             | T                         | 3        | ReturnType          | 1         |
          | IGenericInterface<T> | 1             | T                         | 3        | Property            | 1         |
          | TestGenerics         | 3             | IGenericInterface<string> | 1        | Implementation      | 1         |
          | TestGenerics         | 3             | string                    | 3        | ParameterInjection  | 1         |
          | TestGenerics         | 3             | string                    | 3        | GenericClass        | 1         |
          | TestGenerics         | 3             | string                    | 3        | Property            | 1         |
          | TestGenerics         | 3             | NotSupportedException     | 1        | DirectInstantiation | 1         |
          | GenericClass<T>      | 2             | T                         | 2        | ParameterInjection  | 1         |
          | GenericClass<T>      | 2             | T                         | 2        | ReturnType          | 1         |
          | GenericClass<T>      | 2             | U                         | 2        | ParameterInjection  | 1         |
          | GenericClass<T>      | 2             | U                         | 2        | ReturnType          | 1         |
          | GenericsUsage        | 4             | GenericClass<int>         | 3        | VariableDeclaration | 1         |
          | GenericsUsage        | 4             | GenericClass<int>         | 3        | InstanceInvocation  | 2         |
          | GenericsUsage        | 4             | int                       | 1        | GenericClass        | 1         |
          | GenericsUsage        | 4             | double                    | 1        | GenericMethod       | 1         |
          | GenericsUsage        | 4             | TestGenerics              | 2        | VariableDeclaration | 1         |
          | GenericsUsage        | 4             | TestGenerics              | 2        | InstanceInvocation  | 1         |