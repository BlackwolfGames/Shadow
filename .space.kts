/**
* JetBrains Space Automation
* This Kotlin script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("run tests on commit") {
    startOn {
        gitPush { enabled = true }
	}
  git {
        // fetch 'release' branch and tags
        refSpec {
            +"Devel"
        }
        // get the entire commit history
        depth = UNLIMITED_DEPTH
    }
    // Container for Rust
      container(displayName = "Rust Container", image = "rust:latest") {
        shellScript {
          content = """
              apt-get update
              apt-get install -y clang lld mingw-w64
              rustup target add x86_64-pc-windows-gnu
                  
              cd shadow_rust
              mkdir artifacts
              cd rusty_brain
              
              cargo build --release --package rusty_brain --target=x86_64-pc-windows-gnu
              cargo test --package rusty_brain --lib tests --no-fail-fast
              
              cp target/x86_64-pc-windows-gnu/release/rusty_brain.dll ../artifacts/rusty_brain.dll
              """
              }

          // Upload build/build.zip to the default file repository
          fileArtifacts {
              // To upload to another repo, uncomment the next line
              // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

              // Local path to artifact relative to working dir
              localPath = "shadow_rust/artifacts/rusty_brain.dll"
              // Don't fail job if build.zip is not found
              optional = false
              // Target path to artifact in file repository.
              remotePath = "rusty_brain.dll"
              // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
              onStatus = OnStatus.SUCCESS
          }
      }
      
      // Container for C#
      container(displayName = "C# Container", image = "mcr.microsoft.com/dotnet/sdk:latest") {

          fileInput {
              // we use the provided parameter to reference the default repo
              source = FileSource.FileArtifact(
                      "{{ run:file-artifacts.default-repository }}/Shadow/jobs/run-tests-on-commit/{{ run:number }}",
                      "rusty_brain.dll"
              )
              localPath = "RustLib/rusty_brain.dll"
          }

        shellScript {
            content = """              
              # Building and running tests from Analyzers1.Tests
              dotnet build Analyzers1/Analyzers1.Tests/Analyzers1.Tests.csproj
              dotnet test Analyzers1/Analyzers1.Tests/Analyzers1.Tests.csproj   --logger "junit;LogFileName=test-results.xml"
              
              # Building and running tests from SourceVisUnit
              dotnet build SourceVisUnit/SourceVisUnit.csproj
              dotnet test SourceVisUnit/SourceVisUnit.csproj  --logger "junit;LogFileName=test-results.xml"
              
              # Building and running tests from SourceVisSpec
              dotnet build SourceVisSpec/SourceVisSpec.csproj
              dotnet test SourceVisSpec/SourceVisSpec.csproj  --logger "junit;LogFileName=test-results.xml"
              
              # Building and running tests from ShadowTest
              dotnet build ShadowTest/ShadowTest.csproj
              dotnet test ShadowTest/ShadowTest.csproj   --logger "junit;LogFileName=test-results.xml"
              
              # Building and running tests from ShadowSpecs
              dotnet build ShadowSpecs/ShadowSpecs.csproj
              dotnet test ShadowSpecs/ShadowSpecs.csproj   --logger "junit;LogFileName=test-results.xml"
              """
        }
    }
}
job("Weekly stress test") {
    startOn {
    
        gitPush { enabled = false }
        schedule { cron("0 0 * * 6") }
	}
  git {
        // fetch 'release' branch and tags
        refSpec {
            +"Devel"
        }
        // get the entire commit history
        depth = UNLIMITED_DEPTH
    }
    // Container for Rust
      container(displayName = "Rust Container", image = "rust:latest") {
        shellScript {
          content = """
              apt-get update
              apt-get install -y clang lld mingw-w64
              rustup target add x86_64-pc-windows-gnu
                  
              cd shadow_rust
              mkdir artifacts
              cd rusty_brain
              
              cargo build --release --package rusty_brain --target=x86_64-pc-windows-gnu
              cargo test --package rusty_brain --lib tests --no-fail-fast
              
              cp target/x86_64-pc-windows-gnu/release/rusty_brain.dll ../artifacts/rusty_brain.dll
              """
        }

          // Upload build/build.zip to the default file repository
          fileArtifacts {
              // To upload to another repo, uncomment the next line
              // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

              // Local path to artifact relative to working dir
              localPath = "shadow_rust/artifacts/rusty_brain.dll"
              // Don't fail job if build.zip is not found
              optional = false
              // Target path to artifact in file repository.
              remotePath = "rusty_brain.dll"
              // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
              onStatus = OnStatus.SUCCESS
          }
      }
      
      // Container for C#
      container(displayName = "C# Container", image = "mcr.microsoft.com/dotnet/sdk:latest") {

          fileInput {
              // we use the provided parameter to reference the default repo
              source = FileSource.FileArtifact(
                      "{{ run:file-artifacts.default-repository }}/Shadow/jobs/Weekly-stress-test/{{ run:number }}",
                      "rusty_brain.dll"
              )
              localPath = "RustLib/rusty_brain.dll"
          }
          env["SONAR_TOKEN"] = "{{ project:SONAR_TOKEN }}"
        shellScript {
            content = """
            apt update
            apt install -y openjdk-11-jdk
            export JAVA_HOME=/usr/lib/jvm/java-11-openjdk-amd64
            
              # Installing livingdoc and specflow tools via dotnet
              export PATH="${'$'}PATH:/root/.dotnet/tools:${'$'}JAVA_HOME/bin"
              dotnet tool install -g SpecFlow.Plus.LivingDoc.CLI
              dotnet tool install -g dotnet-stryker
              dotnet tool install --global dotnet-sonarscanner
              
              mkdir artifacts
              cd artifacts
              mkdir SourceVis
              mkdir Shadow
              mkdir Analysis
              cd ..
              
              dotnet sonarscanner begin \
                /o:blackwolfgames \
                /k:BlackwolfGames_Shadow \
                /d:sonar.host.url=https://sonarcloud.io
              
              # Building and running tests from Analyzers1.Tests
              dotnet build Analyzers1/Analyzers1.Tests/Analyzers1.Tests.csproj
              dotnet test Analyzers1/Analyzers1.Tests/Analyzers1.Tests.csproj  --logger "junit;LogFileName=test-results.xml" --collect:"XPlat Code Coverage"
              
              cd Analyzers1/Analyzers1.Tests
              dotnet stryker
              cp -r StrykerOutput ../../artifacts/Analysis/MutationReport
              cp -r TestResults ../../artifacts/Analysis/TestResults
              cd ../../
              
              # Building and running tests from SourceVisUnit
              dotnet build SourceVisUnit/SourceVisUnit.csproj
              dotnet test SourceVisUnit/SourceVisUnit.csproj  --logger "junit;LogFileName=test-results.xml" --collect:"XPlat Code Coverage"
              
              cd SourceVisUnit
              dotnet stryker
              cp -r StrykerOutput ../artifacts/SourceVis/MutationReportUnit
              cp -r TestResults ../artifacts/SourceVis/TestResultsUnit
              cd ../
              
              # Building and running tests from SourceVisSpec
              dotnet build SourceVisSpec/SourceVisSpec.csproj
              dotnet test SourceVisSpec/SourceVisSpec.csproj  --logger "junit;LogFileName=test-results.xml" --collect:"XPlat Code Coverage"
              
              # Generating living documentation
              cd SourceVisSpec/bin/Debug/net7.0
              livingdoc test-assembly SourceVisSpec.dll -t TestExecution.json
              cp LivingDoc.html ../../../../artifacts/SourceVis/LivingDocSource.html
              cd ../../../
              
              dotnet stryker
              cp -r StrykerOutput ../artifacts/SourceVis/MutationReporttSpec
              cp -r TestResults ../artifacts/SourceVis/TestResultsSpec
              cd ../
              
              # Building and running tests from ShadowTest
              dotnet build ShadowTest/ShadowTest.csproj
              dotnet test ShadowTest/ShadowTest.csproj  --logger "junit;LogFileName=test-results.xml" --collect:"XPlat Code Coverage"
              
              cd ShadowTest
              dotnet stryker -p /mnt/space/work/Shadow/ShadowCore/ShadowCore.csproj
              cp -r StrykerOutput ../artifacts/Shadow/MutationReportUnitCore
              dotnet stryker -p /mnt/space/work/Shadow/ShadowEngine/ShadowEngine.csproj
              cp -r StrykerOutput ../artifacts/Shadow/MutationReportUnitEngine
              cp -r TestResults ../artifacts/Shadow/TestResultsUnit
              cd ../
              
              # Building and running tests from ShadowSpecs
              dotnet build ShadowSpecs/ShadowSpecs.csproj
              dotnet test ShadowSpecs/ShadowSpecs.csproj  --logger "junit;LogFileName=test-results.xml" --collect:"XPlat Code Coverage"
              
              # Generating living documentation
              cd ShadowSpecs/bin/Debug/net7.0
              livingdoc test-assembly ShadowSpecs.dll -t TestExecution.json
              cp LivingDoc.html ../../../../artifacts/Shadow/LivingDocShadow.html
              cd ../../../
                            
              dotnet stryker -p /mnt/space/work/Shadow/ShadowCore/ShadowCore.csproj
              cp -r StrykerOutput ../artifacts/Shadow/MutationReportSpecCore
              dotnet stryker -p /mnt/space/work/Shadow/ShadowEngine/ShadowEngine.csproj
              cp -r StrykerOutput ../artifacts/Shadow/MutationReportSpecEngine
              cp -r TestResults ../artifacts/Shadow/TestResultsSpec
              cd ../    
              
              dotnet build Analyzers1/Analyzers1/Analyzers1.csproj -c Release -r win-x64 --self-contained true -o /artifacts/Analysis
              dotnet build ShadowEngine/ShadowEngine.csproj -c Release -r win-x64 --self-contained true -o /artifacts/Shadow
              dotnet build SourceVisCore/SourceVisCore.csproj -c Release -r win-x64 --self-contained true -o /artifacts/SourceVis
              dotnet sonarscanner end
              """
        }

          fileArtifacts {
              // To upload to another repo, uncomment the next line
              // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

              // Local path to artifact relative to working dir
              localPath = "artifacts/Analysis"
              // Don't fail job if build.zip is not found
              optional = false
              archive = true
              // Target path to artifact in file repository.
              remotePath = "Analysis.zip"
              // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
              onStatus = OnStatus.SUCCESS
          }
      fileArtifacts {
          // To upload to another repo, uncomment the next line
          // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

          // Local path to artifact relative to working dir
          localPath = "artifacts/SourceVis"
          // Don't fail job if build.zip is not found
          optional = false
          archive = true
          // Target path to artifact in file repository.
          remotePath = "SourceVis.zip"
          // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
          onStatus = OnStatus.SUCCESS
      }
          fileArtifacts {
              // To upload to another repo, uncomment the next line
              // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

              // Local path to artifact relative to working dir
              localPath = "artifacts/Shadow"
              // Don't fail job if build.zip is not found
              optional = false
              archive = true
              // Target path to artifact in file repository.
              remotePath = "Shadow.zip"
              // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
              onStatus = OnStatus.SUCCESS
          }
    }
}
