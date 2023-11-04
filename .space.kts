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
    
        gitPush { enabled = true }
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
        apt install -y openjdk-17-jdk
        export JAVA_HOME=/usr/lib/jvm/java-17-openjdk-amd64
        
        # Setup environment paths
        export PATH="${'$'}PATH:/root/.dotnet/tools:${'$'}JAVA_HOME/bin"

        # Install necessary tools via dotnet
        dotnet tool install -g SpecFlow.Plus.LivingDoc.CLI
        dotnet tool install -g dotnet-stryker
        dotnet tool install --global dotnet-sonarscanner
        dotnet tool install --global dotnet-test-html
        dotnet tool install --global JetBrains.dotCover.GlobalTool
        
        # Create artifacts directories
        mkdir -p artifacts/{Analysis,Shadow,SourceVis}
        
        # Function for building, testing and collecting coverage and reports
        build_and_test() {
            local project_name="${'$'}1"
            local test_type="${'$'}2"  # Unit or Spec
            dotnet build src/${'$'}project_name -c Release –no-incremental
            dotnet test src/${'$'}project_name.Tests --logger html
            dotnet dotcover test src/${'$'}project_name.Tests --dcReportType=HTML
            dotnet stryker
            cp -r StrykerOutput artifacts/${'$'}project_name/MutationReport${'$'}test_type
            cp -r TestResults artifacts/${'$'}project_name/TestResults${'$'}test_type
        }

        # Analyzers
        build_and_test "Analyzers1/Analyzers1" "Unit"

        # SourceVis
        build_and_test "SourceVis/SourceVisCore" "Unit"
        build_and_test "SourceVis/SourceVisSpec" "Spec"

        # ShadowEngine
        build_and_test "ShadowEngine/ShadowCore" "Unit"
        build_and_test "ShadowEngine/ShadowSpec" "Spec"

        # Generate living documentation for SpecFlow projects
        generate_living_doc() {
            local project_path="${'$'}1"
            local output_path="${'$'}2"
            cd ${'$'}project_path/bin/Debug/net7.0
            livingdoc test-assembly ${'$'}project_path.dll -t TestExecution.json
            cp LivingDoc.html ${'$'}output_path
            cd -
        }
        
        generate_living_doc "src/SourceVis/SourceVisSpec" "artifacts/SourceVis/LivingDocSourceVis.html"
        generate_living_doc "src/ShadowEngine/ShadowSpec" "artifacts/Shadow/LivingDocShadow.html"

        # Packaging executables
        package_executable() {
            local project_name="${'$'}1"
            local output_dir="${'$'}2"
            dotnet build src/${'$'}project_name -c Release -r win-x64 --self-contained true -o ${'$'}output_dir
        }
        
        package_executable "Analyzers1/Analyzers1" "artifacts/Analysis"
        package_executable "ShadowEngine/ShadowEngine" "artifacts/Shadow"
        package_executable "SourceVis/SourceVisCore" "artifacts/SourceVis"

        # Finish up
        dotnet sonarscanner end
        """
    }

          fileArtifacts {
              // To upload to another repo, uncomment the next line
              // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

              // Local path to artifact relative to working dir
              localPath = "artifacts/Analysis/*"
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
          localPath = "artifacts/SourceVis/*"
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
              localPath = "artifacts/Shadow/*"
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
