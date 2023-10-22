Feature: Abstract syntax tree parser
Scenario: Parser can read classes
	Given we parse the following code
	"""
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
	And The class 'HelloWorld.Program' depends on 'Console' 1 time 