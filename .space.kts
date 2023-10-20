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
          cd shadow_rust
          cargo build --release --verbose --package shadow_rust
          """
        }
      }
      
      // Container for C#
      container(displayName = "C# Container", image = "mcr.microsoft.com/dotnet/sdk:latest") {
        shellScript {
            content = """
            
              # Installing livingdoc and specflow tools via dotnet
              dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI
              export PATH="$PATH:/root/.dotnet/tools"
              
              # Building and running tests from ShadowTest
              dotnet build ShadowTest/ShadowTest.csproj
              dotnet test ShadowTest/ShadowTest.csproj
              
              # Building and running tests from ShadowSpecs
              dotnet build ShadowSpecs/ShadowSpecs.csproj
              dotnet test ShadowSpecs/ShadowSpecs.csproj
              
              # Generating living documentation
              cd ShadowSpecs/bin/Debug/net7.0
              livingdoc test-assembly ShadowSpecs.dll -t TestExecution.json
              cp LivingDoc.html ../../../../LivingDoc.html
              """
        }
          // Upload build/build.zip to the default file repository
      fileArtifacts {
          // To upload to another repo, uncomment the next line
          // repository = FileRepository(name = "my-file-repo", remoteBasePath = "{{ run:number }}")

          // Local path to artifact relative to working dir
          localPath = "./LivingDoc.html"
          // Don't fail job if build.zip is not found
          optional = true
          // Target path to artifact in file repository.
          remotePath = "{{ run:number }}/build.zip"
          // Upload condition (job run result): SUCCESS (default), ERROR, ALWAYS
          onStatus = OnStatus.SUCCESS
      }
    }
}
