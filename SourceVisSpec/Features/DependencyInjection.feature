Feature: AST parser - DependencyInjection

    Scenario: Parser tracks parameters, variable declarations and return values
        Given we parse 'Injection/TestDependencyInjection.cs'
        When we prefix classnames with 'Injection.'
        And we prefix dependencies with 'Injection.'
        Then there are 7 classes
        Then there is a class named 'DependsOnStruct'
        Then there is a class named 'IDependsOnInterface'
        Then there is a class named 'DependsOnClass'
        And The class 'DependsOnClass' has 1 dependency 
        And The class 'DependsOnClass' uses 'IDependsOnInterface' 1 time
        
        Then there is a class named 'TestConstructorInjection'
        And The class 'TestConstructorInjection' has 3 dependencies
        And The class 'TestConstructorInjection' uses 'DependsOnClass' as ParameterInjection 1 time
        And The class 'TestConstructorInjection' uses 'IDependsOnInterface' as ParameterInjection 1 time
        And The class 'TestConstructorInjection' uses 'DependsOnStruct' as ParameterInjection 1 time
        
        Then there is a class named 'TestFunctionInject'
        And The class 'TestFunctionInject' has 4 dependencies
        And The class 'TestFunctionInject' uses 'DependsOnClass' 1 times
        And The class 'TestFunctionInject' uses 'DependsOnClass' as ParameterInjection 1 time
        
        And The class 'TestFunctionInject' uses 'IDependsOnInterface' 1 times
        And The class 'TestFunctionInject' uses 'IDependsOnInterface' as ParameterInjection 1 time
        
        And The class 'TestFunctionInject' uses 'DependsOnStruct' 1 times
        And The class 'TestFunctionInject' uses 'DependsOnStruct' as ParameterInjection 1 time
        
        When we prefix dependencies with 'System.'
        Then The class 'TestFunctionInject' uses 'NotSupportedException' 1 times
        And The class 'TestFunctionInject' uses 'NotSupportedException' as DirectInstantiation 1 time
        When we prefix dependencies with 'Injection.'

        Then there is a class named 'TestFunctionReturn'
        And The class 'TestFunctionReturn' has 3 dependencies
        And The class 'TestFunctionReturn' uses 'DependsOnClass' 3 times
        And The class 'TestFunctionReturn' uses 'DependsOnClass' as ReturnType 1 time
        And The class 'TestFunctionReturn' uses 'DependsOnClass' as DirectInstantiation 2 times
        
        And The class 'TestFunctionReturn' uses 'IDependsOnInterface' 1 times
        And The class 'TestFunctionReturn' uses 'IDependsOnInterface' as ReturnType 1 time
        
        And The class 'TestFunctionReturn' uses 'DependsOnStruct' 1 times
        And The class 'TestFunctionReturn' uses 'DependsOnStruct' as ReturnType 1 time

        Then there is a class named 'TestVariableDeclarations'
        And The class 'TestVariableDeclarations' has 3 dependencies
        And The class 'TestVariableDeclarations' uses 'DependsOnClass' 4 times
        And The class 'TestVariableDeclarations' uses 'DependsOnClass' as VariableDeclaration 2 times
        And The class 'TestVariableDeclarations' uses 'DependsOnClass' as DirectInstantiation 1 times
        And The class 'TestVariableDeclarations' uses 'DependsOnClass' as ImplicitConversion 1 times
        
        And The class 'TestVariableDeclarations' uses 'IDependsOnInterface' 2 times
        And The class 'TestVariableDeclarations' uses 'IDependsOnInterface' as VariableDeclaration 2 times
        
        And The class 'TestVariableDeclarations' uses 'DependsOnStruct' 2 times
        And The class 'TestVariableDeclarations' uses 'DependsOnStruct' as VariableDeclaration 2 times
