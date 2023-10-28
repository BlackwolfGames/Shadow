Feature: AST parser - Class Types

	Scenario: Parser can read classes
		Given we parse 'Classes/TestClassTypes.cs'
		Then there are 4 class
		Then there is a class named 'HelloWorld.TestClass'
		Then there is a class named 'HelloWorld.TestStruct'
		Then there is a class named 'HelloWorld.ITestInterface'
		Then there is a class named 'HelloWorld.TestRecord'
		
	Scenario: Parser can detect multiple classes in one file
		Given we parse 'Classes/TestCallMultipleClass.cs'
		Then there are 2 classes
		Then there is a class named 'HelloWorld.TestCallMultipleClass1'
		Then there is a class named 'HelloWorld.TestCallMultipleClass2'
		And The class 'HelloWorld.TestCallMultipleClass1' has 1 dependency 
		And The class 'HelloWorld.TestCallMultipleClass1' uses 'System.Console' as StaticInvocation 1 time
		And The class 'HelloWorld.TestCallMultipleClass2' has 1 dependency 
		And The class 'HelloWorld.TestCallMultipleClass2' uses 'System.Console' as StaticInvocation 1 time