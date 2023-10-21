/**
* JetBrains Space Automation
* This Kotlin script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("run tests") {
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
          cargo build --release --verbose --package rusty_brain --target=x86_64-pc-windows-gnu
          cargo test --color=always --package rusty_brain --lib tests --no-fail-fast -- --show-output
          
          ls
          cd target 
          ls
          cd x86_64-pc-windows-gnu 
          ls
          cd release
          ls
          
          
          cp rusty_brain.dll ../../../../artifacts/rusty_brain.dll
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
                      "{{ run:file-artifacts.default-repository }}",
                      "/Shadow/jobs/run-tests/{{ run:number }}/rusty_brain.dll"
              )
              localPath = "RustLib/rusty_brain.dll"
          }

        shellScript {
            content = """
            
              # Installing livingdoc and specflow tools via dotnet
              dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI
              export PATH="${'$'}PATH:/root/.dotnet/tools"
              
              mkdir artifacts
              cd artifacts
              mkdir Sourcevis
              mkdir Shadow
              mkdir Analysis
              cd ..
              
              # Building and running tests from Analyzers1.Tests
              dotnet build Analyzers1.Tests/Analyzers1.Tests.csproj
              dotnet test Analyzers1.Tests/Analyzers1.Tests.csproj  --logger "trx;LogFileName=results.trx"
              
              # Building and running tests from SourceVisUnit
              dotnet build SourceVisUnit/SourceVisUnit.csproj
              dotnet test SourceVisUnit/SourceVisUnit.csproj  --logger "trx;LogFileName=results.trx"
              
              # Building and running tests from SourceVisSpec
              dotnet build SourceVisSpec/SourceVisSpec.csproj
              dotnet test SourceVisSpec/SourceVisSpec.csproj  --logger "trx;LogFileName=results.trx"
              
              # Building and running tests from ShadowTest
              dotnet build ShadowTest/ShadowTest.csproj
              dotnet test ShadowTest/ShadowTest.csproj  --logger "trx;LogFileName=results.trx"
              
              # Building and running tests from ShadowSpecs
              dotnet build ShadowSpecs/ShadowSpecs.csproj
              dotnet test ShadowSpecs/ShadowSpecs.csproj  --logger "trx;LogFileName=results.trx"
              
              dotnet build Analyzers1.Tests/Analyzers1.csproj -c Release -r win-x64 --self-contained true -o /artifacts/Analysis
              dotnet build ShadowEngine/ShadowEngine.csproj -c Release -r win-x64 --self-contained true -o /artifacts/Shadow
              dotnet build SourceVisCore/SourceVisCore.csproj -c Release -r win-x64 --self-contained true -o /artifacts/Sourcevis
              
              # Generating living documentation
              cd SourceVisSpec/bin/Debug/net7.0
              livingdoc test-assembly SourceVisSpec.dll -t TestExecution.json
              cp LivingDoc.html ../../../../artifacts/Sourcevis/LivingDocSource.html
              cd ../../../../
              
              # Generating living documentation
              cd ShadowSpecs/bin/Debug/net7.0
              livingdoc test-assembly ShadowSpecs.dll -t TestExecution.json
              cp LivingDocShadow.html ../../../../artifacts/Shadow/LivingDocShadow.html
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
          localPath = "artifacts/Sourcevis"
          // Don't fail job if build.zip is not found
          optional = false
          archive = true
          // Target path to artifact in file repository.
          remotePath = "Sourcevis.zip"
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
