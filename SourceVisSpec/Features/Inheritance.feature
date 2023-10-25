Feature: AST parser - Inheritance
	Simple calculator for adding two numbers

	Scenario: Parser can detect static calls
		Given we parse 'Inheritance/TestCallMultipleClass.cs'
		Then there are 2 classes
		Then there is a class named 'HelloWorld.TestCallMultipleClass1'
		Then there is a class named 'HelloWorld.TestCallMultipleClass2'
		And The class 'HelloWorld.TestCallMultipleClass1' has 1 dependency 
		And The class 'HelloWorld.TestCallMultipleClass1' uses 'System.Console'
		And The class 'HelloWorld.TestCallMultipleClass2' has 1 dependency 
		And The class 'HelloWorld.TestCallMultipleClass2' uses 'System.Console'