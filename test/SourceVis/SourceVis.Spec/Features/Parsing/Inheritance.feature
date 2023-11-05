Feature: AST parser - Inheritance
			
	Scenario: Parser can read inheritance, and ignores transient dependencies
		Given we parse 'Parsing/Inheritance/TestInheritance.cs'
		Then there are 4 classes
		Then there is a class named 'TestInheritance'
		Then there is a class named 'ITestInterface'
		Then there is a class named 'TestAbstractBase'
		Then there is a class named 'TestDerived'
		And The class 'TestInheritance' has 1 dependency 
		And The class 'TestInheritance' uses 'ITestInterface' as Implementation 1 time
		And The class 'TestAbstractBase' has 1 dependency 
		And The class 'TestAbstractBase' uses 'ITestInterface' as Implementation 1 time
		And The class 'TestDerived' has 1 dependencies 
		And The class 'TestDerived' uses 'TestAbstractBase' as Extension 1 time