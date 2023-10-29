cd "SourceVisSpec\bin\Debug\net7.0"
livingdoc test-assembly SourceVisSpec.dll -t TestExecution.json
copy "LivingDoc.html" "../../../../LivingDoc.html"
start LivingDoc.html