Feature: Abstract syntax tree parser
Scenario: Parser can read classes
	Given we parse the following code
	"""
       namespace HelloWorld
       {
           class Program
           {
           }
       }
	"""
	Then there is 1 class
	
	Scenario: Parser can detect function calls
		Given we parse the following code
		"""
			using System;
		      namespace HelloWorld
		      {
		          class Program
		          {
		              static void Main(string[] args)
		              {
		                  Console.WriteLine("Hello, World!");
		              }
		          }
		      }
		"""
		Then there is 1 class
		And The class 'HelloWorld.Program' depends on 'System.Console' as StaticInvocation 1 time 