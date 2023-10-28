Feature: AST parser - Inheritance
			
	Scenario: Parser can read inheritance, and ignores transient dependencies
		Given we parse 'Inheritance/TestInheritance.cs'
		Then there are 4 classes
		Then there is a class named 'Inheritance.TestInheritance'
		Then there is a class named 'Inheritance.ITestInterface'
		Then there is a class named 'Inheritance.TestAbstractBase'
		Then there is a class named 'Inheritance.TestDerived'
		And The class 'Inheritance.TestInheritance' has 1 dependency 
		And The class 'Inheritance.TestInheritance' uses 'Inheritance.ITestInterface' as Implementation 1 time
		And The class 'Inheritance.TestAbstractBase' has 1 dependency 
		And The class 'Inheritance.TestAbstractBase' uses 'Inheritance.ITestInterface' as Implementation 1 time
		And The class 'Inheritance.TestDerived' has 1 dependencies 
		And The class 'Inheritance.TestDerived' uses 'Inheritance.TestAbstractBase' as Extension 1 time