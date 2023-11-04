/**
* JetBrains Space Automation
* This Kotlin script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/


    fun RustBuild() = """
                                    apt-get update
                                    apt-get install -y clang lld mingw-w64
                                    rustup target add x86_64-pc-windows-gnu
                                        
                                    cd shadow_rust
                                    mkdir artifacts
                                    cd rusty_brain
                                    
                                    cargo build --release --package rusty_brain --target=x86_64-pc-windows-gnu
                                    cargo test --package rusty_brain --lib tests --no-fail-fast
                                    
                                    cp target/x86_64-pc-windows-gnu/release/rusty_brain.dll ../artifacts/rusty_brain.dll
                                    """.trimIndent()
    
    fun callSharedScript() = """
                                     apt update
                                     apt install -y openjdk-17-jdk
                                     export JAVA_HOME=/usr/lib/jvm/java-17-openjdk-amd64
                                     
                                     # Setup environment paths
                                     export PATH="${'$'}PATH:/root/.dotnet/tools:${'$'}JAVA_HOME/bin"
                             
                                     # Install necessary tools via dotnet
                                     dotnet tool install -g SpecFlow.Plus.LivingDoc.CLI
                                     dotnet tool install -g dotnet-stryker
                                     dotnet tool install --global dotnet-sonarscanner
                                     dotnet tool install --global JetBrains.dotCover.GlobalTool
                                     
                                     # Create artifacts directories
                                     mkdir -p artifacts/{Shadow,SourceVis}
                                    
                                           dotnet sonarscanner begin \
                                             /o:blackwolfgames \
                                             /k:BlackwolfGames_Shadow \
                                             /d:sonar.host.url=https://sonarcloud.io \
                                             /d:sonar.cs.dotcover.reportsPaths=dotCover.Output.html
                                     
                                     # Function for building, testing and collecting coverage and reports
                                     build_and_test() {
                                         local project_name="${'$'}1"
                                         local test_type="${'$'}2"  # Unit or Spec
                                         dotnet build src/${'$'}project_name/${'$'}project_name.Core -c Release –no-incremental
                                         dotnet test test/${'$'}project_name/${'$'}project_name.${'$'}test_type --logger html
                                         dotnet dotcover test src/${'$'}project_name.Tests --dcReportType=HTML
                                          if [ -z "${'$'}IS_CRON_JOB" ]; then
                                                     dotnet stryker
                                                     cp -r StrykerOutput artifacts/${'$'}project_name/MutationReport${'$'}test_type
                                                 fi
                                         cp -r TestResults artifacts/${'$'}project_name/TestResults${'$'}test_type
                                     }
                                     # SourceVis
                                     build_and_test "SourceVis" "Unit"
                                     build_and_test "SourceVis" "Spec"
                             
                                     # ShadowEngine
                                     build_and_test "ShadowEngine" "Unit"
                                     build_and_test "ShadowEngine" "Spec"
                             
                                     # Generate living documentation for SpecFlow projects
                                     generate_living_doc() {
                                         local project_name="${'$'}1"
                                         cd test/${'$'}project_name/${'$'}project_name.spec/bin/Debug/net7.0
                                         livingdoc test-assembly ${'$'}project_name.Spec.dll -t TestExecution.json
                                         cp LivingDoc.html artifacts/${'$'}project_name/LivingDoc${'$'}project_name.html
                                         cd -
                                     }
                                     
                                     generate_living_doc "SourceVis"
                                     generate_living_doc "ShadowEngine"
                             
                                     # Packaging executables
                                     package_executable() {
                                         local project_name="${'$'}1"
                                         local project_type="${'$'}2"
                                         dotnet build src/${'$'}project_name.${'$'}project_type -c Release -r win-x64 --self-contained true -o artifacts/${'$'}project_name
                                     }
                                     
                                     package_executable "ShadowEngine" "Gui"
                                     package_executable "SourceVis" "Core"
                             
                                     # Finish up
                                     dotnet sonarscanner end
                                     """.trimIndent()

job("run tests on commit") {
    startOn {
        gitPush { enabled = true }
	}
  git {
        // fetch 'release' branch and tags
        refSpec {
            +"master"
        }
        // get the entire commit history
        depth = UNLIMITED_DEPTH
    }
    // Container for Rust
      container(displayName = "Rust Container", image = "rust:latest") {
        shellScript {
          content = RustBuild()
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
        env["IS_CRON_JOB"] = "false"

        shellScript {
            content = callSharedScript()
        }
    }
}
job("Weekly stress test") {
    startOn {
    
        schedule { cron("0 0 * * 6") }
	}
  git {
        // fetch 'release' branch and tags
        refSpec {
            +"master"
        }
        // get the entire commit history
        depth = UNLIMITED_DEPTH
    }
    // Container for Rust
      container(displayName = "Rust Container", image = "rust:latest") {
        shellScript {
          content = RustBuild();
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

        env["IS_CRON_JOB"] = "true"
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
        content = callSharedScript()
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
