Feature: AST parser - Direct instantiation

	Scenario: Parser can understand instantiation, and ignores transient dependencies
		Given we parse 'Instantiation/TestDirectInstantiation.cs'
		Then there are 2 classes
		Then there is a class named 'Instantiation.InstantiatesSelf'
		Then there is a class named 'Instantiation.InstantiatesOther'
		And The class 'Instantiation.InstantiatesSelf' has 1 dependency 
		And The class 'Instantiation.InstantiatesSelf' uses 'Instantiation.InstantiatesSelf' as DirectInstantiation 1 time
		And The class 'Instantiation.InstantiatesOther' has 1 dependency 
		And The class 'Instantiation.InstantiatesOther' uses 'Instantiation.InstantiatesSelf' as DirectInstantiation 1 time