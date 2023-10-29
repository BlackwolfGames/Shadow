Feature: AST parser - SimpleCalls
	
	Scenario: Parser can detect static calls
		Given we parse 'Calls/TestCallConsole.cs'
		Then there is 1 class
		Then there is a class named 'TestCallConsole'
		And The class 'TestCallConsole' has 1 dependency 
		And The class 'TestCallConsole' uses 'Console' 1 time 
		And The class 'TestCallConsole' uses 'Console' as StaticInvocation 1 time 
		
	
	Scenario: Parser can detect multiple calls with 'using static'
		Given we parse 'Calls/TestMultipleCalls.cs'
		Then there is 1 class
		Then there is a class named 'TestMultipleCalls'
		And The class 'TestMultipleCalls' has 1 dependency 
		And The class 'TestMultipleCalls' uses 'Console' 3 times
		And The class 'TestMultipleCalls' uses 'Console' as StaticInvocation 3 times 
		
	Scenario: Parser finds calls within lambdas and local functions
		Given we parse 'Calls/TestCallInLambda.cs'
		Then there is 1 class
		Then there is a class named 'TestCallInLambda'
		And The class 'TestCallInLambda' has 4 dependencies
		And The class 'TestCallInLambda' uses 'Action' 2 times
		And The class 'TestCallInLambda' uses 'Action' as InstanceInvocation 1 times 
		And The class 'TestCallInLambda' uses 'Action' as VariableDeclaration 1 times 
		And The class 'TestCallInLambda' uses 'TestCallInLambda' 2 times
		And The class 'TestCallInLambda' uses 'TestCallInLambda' as StaticInvocation 1 times 
		And The class 'TestCallInLambda' uses 'TestCallInLambda' as InstanceInvocation 1 times 
		And The class 'TestCallInLambda' uses 'Console' 3 times
		And The class 'TestCallInLambda' uses 'Console' as StaticInvocation 3 times 