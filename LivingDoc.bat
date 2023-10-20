cd "ShadowSpecs\bin\Debug\net7.0"
livingdoc test-assembly ShadowSpecs.dll -t TestExecution.json
copy "LivingDoc.html" "../../../../LivingDoc.html"
start LivingDoc.html