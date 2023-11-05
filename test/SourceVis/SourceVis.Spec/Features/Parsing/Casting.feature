Feature: AST parser - Casting
	Simple calculator for adding two numbers

	Scenario: Parser tracks type casting
		Given we parse 'Parsing/Casting/TestTypecasting.cs'
		Then there are 4 classes
		Then there is a class named 'TypeA'
		And The class 'TypeA' has 2 dependencies
		And The class 'TypeA' uses 'TypeB' 1 times
		And The class 'TypeA' uses 'TypeB' as ParameterInjection 1 times
		And The class 'TypeA' uses 'TypeA' 1 times
		And The class 'TypeA' uses 'TypeA' as ParameterInjection 1 times
		
		Then there is a class named 'TypeB'
		And The class 'TypeB' has 0 dependencies
		
		Then there is a class named 'TypeC'
		And The class 'TypeC' has 1 dependencies
		And The class 'TypeC' uses 'TypeB' 1 times
		And The class 'TypeC' uses 'TypeB' as Extension 1 times
		
		Then there is a class named 'TestTypecasting'
		And The class 'TestTypecasting' has 3 dependencies
		And The class 'TestTypecasting' uses 'TypeA' 5 times
		And The class 'TestTypecasting' uses 'TypeA' as SafeCast 0 times
		And The class 'TestTypecasting' uses 'TypeA' as TypeCast 1 times
		And The class 'TestTypecasting' uses 'TypeA' as ImplicitConversion 1 times
		And The class 'TestTypecasting' uses 'TypeA' as DirectInstantiation 1 times
		And The class 'TestTypecasting' uses 'TypeA' as VariableDeclaration 2 times
		
		And The class 'TestTypecasting' uses 'TypeB' 3 times
		And The class 'TestTypecasting' uses 'TypeB' as SafeCast 0 times
		And The class 'TestTypecasting' uses 'TypeB' as TypeCast 1 times
		And The class 'TestTypecasting' uses 'TypeB' as ImplicitConversion 0 times
		And The class 'TestTypecasting' uses 'TypeB' as DirectInstantiation 0 times
		And The class 'TestTypecasting' uses 'TypeB' as VariableDeclaration 2 times
		
		And The class 'TestTypecasting' uses 'TypeC' 2 times
		And The class 'TestTypecasting' uses 'TypeC' as SafeCast 1 times
		And The class 'TestTypecasting' uses 'TypeC' as TypeCast 0 times
		And The class 'TestTypecasting' uses 'TypeC' as ImplicitConversion 0 times
		And The class 'TestTypecasting' uses 'TypeC' as DirectInstantiation 0 times
		And The class 'TestTypecasting' uses 'TypeC' as VariableDeclaration 1 times
		