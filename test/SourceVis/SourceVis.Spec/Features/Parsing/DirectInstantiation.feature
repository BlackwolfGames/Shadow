Feature: AST parser - Direct instantiation

 Scenario: Parser can understand instantiation, and ignores transient dependencies
  Given we parse 'Parsing/Instantiation/TestDirectInstantiation.cs'
  Then there are 2 classes
  Then there is a class named 'InstantiatesSelf'
  Then there is a class named 'InstantiatesOther'
  And The class 'InstantiatesSelf' has 1 dependency
  And The class 'InstantiatesSelf' uses 'InstantiatesSelf' as DirectInstantiation 1 time
  And The class 'InstantiatesOther' has 1 dependency
  And The class 'InstantiatesOther' uses 'InstantiatesSelf' as DirectInstantiation 1 time