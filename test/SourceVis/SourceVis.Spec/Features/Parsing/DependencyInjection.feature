Feature: AST parser - DependencyInjection

    Scenario Outline: Parser tracks parameters, variable declarations and return values
        Given we parse 'Parsing/Injection/TestDependencyInjection.cs'
        Then there are 7 classes

        And there is a class named '<ClassName>'
        And The class '<ClassName>' has <ClassDepCount> dependencies
        And The class '<ClassName>' uses '<DepName>' <DepCount> times
        And The class '<ClassName>' uses '<DepName>' as <DepType> <TypeCount> times

        Examples:
          | ClassName           | ClassDepCount | DepName               | DepCount | DepType             | TypeCount |
          | DependsOnStruct     | 1             | int                   | 1        | VariableDeclaration | 1         |
          | IDependsOnInterface | 1             | DependsOnStruct       | 1        | ReturnType          | 1         |
          | DependsOnClass      | 3             | NotSupportedException | 1        | DirectInstantiation | 1         |
          | DependsOnClass      | 3             | IDependsOnInterface   | 1        | Implementation      | 1         |
          | DependsOnClass      | 3             | DependsOnStruct       | 1        | ReturnType          | 1         |

          | TestConstructorInjection | 3 | DependsOnClass      | 1 | ParameterInjection | 1 |
          | TestConstructorInjection | 3 | IDependsOnInterface | 1 | ParameterInjection | 1 |
          | TestConstructorInjection | 3 | DependsOnStruct     | 1 | ParameterInjection | 1 |

          | TestFunctionInject | 4 | DependsOnClass        | 1 | ParameterInjection  | 1 |
          | TestFunctionInject | 4 | IDependsOnInterface   | 1 | ParameterInjection  | 1 |
          | TestFunctionInject | 4 | DependsOnStruct       | 1 | ParameterInjection  | 1 |
          | TestFunctionInject | 4 | NotSupportedException | 2 | DirectInstantiation | 1 |
          | TestFunctionInject | 4 | NotSupportedException | 2 | ThrownException     | 1 |

          | TestFunctionReturn | 3 | DependsOnClass      | 4 | ReturnType          | 1 |
          | TestFunctionReturn | 3 | DependsOnClass      | 4 | DirectInstantiation | 2 |
          | TestFunctionReturn | 3 | DependsOnClass      | 4 | Property            | 1 |
          | TestFunctionReturn | 3 | IDependsOnInterface | 2 | ReturnType          | 1 |
          | TestFunctionReturn | 3 | IDependsOnInterface | 2 | Property            | 1 |
          | TestFunctionReturn | 3 | DependsOnStruct     | 2 | ReturnType          | 1 |
          | TestFunctionReturn | 3 | DependsOnStruct     | 2 | Property            | 1 |

          | TestVariableDeclarations | 3 | DependsOnClass      | 4 | VariableDeclaration | 2 |
          | TestVariableDeclarations | 3 | DependsOnClass      | 4 | DirectInstantiation | 1 |
          | TestVariableDeclarations | 3 | DependsOnClass      | 4 | ImplicitConversion  | 1 |
          | TestVariableDeclarations | 3 | IDependsOnInterface | 2 | VariableDeclaration | 2 |
          | TestVariableDeclarations | 3 | DependsOnStruct     | 2 | VariableDeclaration | 2 |

    #
    #
    #        Then there is a class named 'DependsOnStruct'
    #        Then there is a class named 'IDependsOnInterface'
    #        Then there is a class named 'DependsOnClass'
    #        And The class 'DependsOnClass' has 1 dependency
    #        And The class 'DependsOnClass' uses 'IDependsOnInterface' 1 time
    #
    #        Then there is a class named 'TestConstructorInjection'
    #        And The class 'TestConstructorInjection' has 3 dependencies
    #        And The class 'TestConstructorInjection' uses 'DependsOnClass' as ParameterInjection 1 time
    #        And The class 'TestConstructorInjection' uses 'IDependsOnInterface' as ParameterInjection 1 time
    #        And The class 'TestConstructorInjection' uses 'DependsOnStruct' as ParameterInjection 1 time
    #
    #        Then there is a class named 'TestFunctionInject'
    #        And The class 'TestFunctionInject' has 4 dependencies
    #        And The class 'TestFunctionInject' uses 'DependsOnClass' 1 times
    #        And The class 'TestFunctionInject' uses 'DependsOnClass' as ParameterInjection 1 time
    #
    #        And The class 'TestFunctionInject' uses 'IDependsOnInterface' 1 times
    #        And The class 'TestFunctionInject' uses 'IDependsOnInterface' as ParameterInjection 1 time
    #
    #        And The class 'TestFunctionInject' uses 'DependsOnStruct' 1 times
    #        And The class 'TestFunctionInject' uses 'DependsOnStruct' as ParameterInjection 1 time
    #
    #        Then The class 'TestFunctionInject' uses 'NotSupportedException' 2 times
    #        And The class 'TestFunctionInject' uses 'NotSupportedException' as DirectInstantiation 1 time
    #        And The class 'TestFunctionInject' uses 'NotSupportedException' as ThrownException 1 time
    #
    #        Then there is a class named 'TestFunctionReturn'
    #        And The class 'TestFunctionReturn' has 3 dependencies
    #        And The class 'TestFunctionReturn' uses 'DependsOnClass' 3 times
    #        And The class 'TestFunctionReturn' uses 'DependsOnClass' as ReturnType 1 time
    #        And The class 'TestFunctionReturn' uses 'DependsOnClass' as DirectInstantiation 2 times
    #
    #        And The class 'TestFunctionReturn' uses 'IDependsOnInterface' 1 times
    #        And The class 'TestFunctionReturn' uses 'IDependsOnInterface' as ReturnType 1 time
    #
    #        And The class 'TestFunctionReturn' uses 'DependsOnStruct' 1 times
    #        And The class 'TestFunctionReturn' uses 'DependsOnStruct' as ReturnType 1 time
    #
    #        Then there is a class named 'TestVariableDeclarations'
    #        And The class 'TestVariableDeclarations' has 3 dependencies
    #        And The class 'TestVariableDeclarations' uses 'DependsOnClass' 4 times
    #        And The class 'TestVariableDeclarations' uses 'DependsOnClass' as VariableDeclaration 2 times
    #        And The class 'TestVariableDeclarations' uses 'DependsOnClass' as DirectInstantiation 1 times
    #        And The class 'TestVariableDeclarations' uses 'DependsOnClass' as ImplicitConversion 1 times
    #
    #        And The class 'TestVariableDeclarations' uses 'IDependsOnInterface' 2 times
    #        And The class 'TestVariableDeclarations' uses 'IDependsOnInterface' as VariableDeclaration 2 times
    #
    #        And The class 'TestVariableDeclarations' uses 'DependsOnStruct' 2 times
    #        And The class 'TestVariableDeclarations' uses 'DependsOnStruct' as VariableDeclaration 2 times