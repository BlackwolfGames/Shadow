Feature: AST parser - Class Types

 Scenario: Parser can read classes
  Given we parse 'Parsing/Classes/TestClassTypes.cs'
  Then there are 4 class
  Then there is a class named 'TestClass'
  Then there is a class named 'TestStruct'
  Then there is a class named 'ITestInterface'
  Then there is a class named 'TestRecord'

 Scenario: Parser can detect multiple classes in one file
  Given we parse 'Parsing/Classes/TestCallMultipleClass.cs'
  Then there are 2 classes
  Then there is a class named 'TestCallMultipleClass1'
  Then there is a class named 'TestCallMultipleClass2'
  And The class 'TestCallMultipleClass1' has 1 dependency
  And The class 'TestCallMultipleClass1' uses 'Console' as StaticInvocation 1 time
  And The class 'TestCallMultipleClass2' has 1 dependency
  And The class 'TestCallMultipleClass2' uses 'Console' as StaticInvocation 1 time