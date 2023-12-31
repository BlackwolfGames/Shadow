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
                             
                             
                                    # Install necessary tools if not already present
                                    tool_check_and_install() {
                                      local tool_name="$1"
                                      if ! dotnet tool list -g | grep -q "${'$'}tool_name"; then
                                        dotnet tool install -g "${'$'}tool_name"
                                      fi
                                    }
                                    
                                    # Tools to install
                                    tool_check_and_install SpecFlow.Plus.LivingDoc.CLI
                                    tool_check_and_install dotnet-stryker
                                    tool_check_and_install dotnet-sonarscanner
                                    tool_check_and_install JetBrains.dotCover.GlobalTool
                                     
                                     # Create artifacts directories
                                     mkdir -p artifacts/ShadowEngine
                                     mkdir -p artifacts/SourceVis
                                    
                                       dotnet sonarscanner begin \
                                         /o:blackwolfgames \
                                         /k:BlackwolfGames_Shadow \
                                         /d:sonar.host.url=https://sonarcloud.io \
                                         /d:sonar.cs.dotcover.reportsPaths=artifacts/**/CoverageReport*.html
                                     
                                     dotnet build ShadowEngine.sln --no-incremental -c Debug
                                     # Function for building, testing and collecting coverage and reports
                                     build_and_test() {
                                         local project_name="${'$'}1"
                                         local test_type="${'$'}2"  # Unit or Spec
                                         local original_dir=${'$'}(pwd) # Save the original directory path
                                     
                                         # Define the test project path
                                         local test_project_path="test/${'$'}{project_name}/${'$'}{project_name}.${'$'}{test_type}"
                                     
                                         # Check if the test project directory exists before attempting to test
                                         if [ -d "${'$'}test_project_path" ]; then
                                             # Navigate to the test project directory
                                             cd "${'$'}test_project_path" || return 1
                                     
                                             # Run tests and collect coverage
                                             dotnet test --no-build --logger "html;LogFileName=TestResults.html" -c Debug --results-directory "${'$'}{original_dir}/artifacts/${'$'}{project_name}/TestResults${'$'}{test_type}.html"
                                             dotnet dotcover test --no-build --dcReportType="HTML" --dcOutput="${'$'}{original_dir}/artifacts/${'$'}{project_name}/CoverageReport${'$'}{test_type}.html"
                                     
                                             # Only run mutation testing if not a cron job
                                             if [ -z "${'$'}IS_CRON_JOB" ]; then
                                                 dotnet stryker
                                                 # Copy the results
                                                 cp -r StrykerOutput "${'$'}{original_dir}/artifacts/${'$'}{project_name}/MutationReport${'$'}{test_type}"
                                             fi
                                     
                                             # Return to the original directory
                                             cd "${'$'}original_dir" || return 1
                                         else
                                             echo "Test project directory '${'$'}{test_project_path}' does not exist."
                                             return 1
                                         fi
                                     }
                                     # SourceVis
                                     build_and_test "SourceVis" "Unit"
                                     build_and_test "SourceVis" "Spec"
                             
                                     # ShadowEngine
                                     build_and_test "ShadowEngine" "Unit"
                                     build_and_test "ShadowEngine" "Spec"
                             
                                     # Generate living documentation for SpecFlow projects
                                     generate_living_doc() {
                                        local original_dir=${'$'}(pwd) # Save the original directory path
                                         local project_name="${'$'}1"
                                         cd test/${'$'}project_name/${'$'}project_name.Spec/bin/Debug/net7.0
                                         livingdoc test-assembly ${'$'}project_name.Spec.dll -t TestExecution.json
                                         cp LivingDoc.html ${'$'}original_dir/artifacts/${'$'}project_name/LivingDoc${'$'}project_name.html
                                         cd ${'$'}original_dir
                                     }
                                     
                                     generate_living_doc "SourceVis"
                                     generate_living_doc "ShadowEngine"
                             
                                     # Packaging executables
                                     package_executable() {
                                         local project_name="${'$'}1"
                                         local project_type="${'$'}2"
                                         dotnet build src/${'$'}project_name/${'$'}project_name.${'$'}project_type -c Release -r win-x64 --self-contained true -o artifacts/${'$'}project_name/Build
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
          env["SONAR_TOKEN"] = "{{ project:SONAR_TOKEN }}"

        shellScript {
            content = callSharedScript()
        }
        
          fileArtifacts {
              // To upload to another repo, uncomment the next line
              // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

              // Local path to artifact relative to working dir
              localPath = "artifacts/**/*"
              // Don't fail job if build.zip is not found
              optional = false
              archive = true
              // Target path to artifact in file repository.
              remotePath = "artifacts.zip"
              // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
              onStatus = OnStatus.SUCCESS
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
              localPath = "artifacts/**/*"
              // Don't fail job if build.zip is not found
              optional = false
              archive = true
              // Target path to artifact in file repository.
              remotePath = "artifacts.zip"
              // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
              onStatus = OnStatus.SUCCESS
          }
    }
    
}
