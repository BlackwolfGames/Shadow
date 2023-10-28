Feature: AST parser - SimpleCalls
	
	Scenario: Parser can detect static calls
		Given we parse 'Calls/TestCallConsole.cs'
		Then there is 1 class
		Then there is a class named 'HelloWorld.TestCallConsole'
		And The class 'HelloWorld.TestCallConsole' has 1 dependency 
		And The class 'HelloWorld.TestCallConsole' uses 'System.Console' 1 time 
		And The class 'HelloWorld.TestCallConsole' uses 'System.Console' as StaticInvocation 1 time 
		
	
	Scenario: Parser can detect multiple calls with 'using static'
		Given we parse 'Calls/TestMultipleCalls.cs'
		Then there is 1 class
		Then there is a class named 'HelloWorld.TestMultipleCalls'
		And The class 'HelloWorld.TestMultipleCalls' has 1 dependency 
		And The class 'HelloWorld.TestMultipleCalls' uses 'System.Console' 3 times
		And The class 'HelloWorld.TestMultipleCalls' uses 'System.Console' as StaticInvocation 3 times 
		
	Scenario: Parser finds calls within lambdas and local functions
		Given we parse 'Calls/TestCallInLambda.cs'
		Then there is 1 class
		Then there is a class named 'HelloWorld.TestCallInLambda'
		And The class 'HelloWorld.TestCallInLambda' has 4 dependencies
		And The class 'HelloWorld.TestCallInLambda' uses 'System.Action' 2 times
		And The class 'HelloWorld.TestCallInLambda' uses 'System.Action' as InstanceInvocation 1 times 
		And The class 'HelloWorld.TestCallInLambda' uses 'System.Action' as VariableDeclaration 1 times 
		And The class 'HelloWorld.TestCallInLambda' uses 'HelloWorld.TestCallInLambda' 2 times
		And The class 'HelloWorld.TestCallInLambda' uses 'HelloWorld.TestCallInLambda' as StaticInvocation 1 times 
		And The class 'HelloWorld.TestCallInLambda' uses 'HelloWorld.TestCallInLambda' as InstanceInvocation 1 times 
		And The class 'HelloWorld.TestCallInLambda' uses 'System.Console' 3 times
		And The class 'HelloWorld.TestCallInLambda' uses 'System.Console' as StaticInvocation 3 times 