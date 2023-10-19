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
          cargo build --release --verbose
          """
        }
        // Uploading the Rust library as an artifact
        artifactRules = listOf("shadow_rust/target/release/*.so")
      }
      
      // Container for C#
      container(displayName = "C# Container", image = "mcr.microsoft.com/dotnet/sdk:latest") {
        // Downloading the Rust library from artifacts
        artifactRules = listOf("*.so => MyCSProject/libs/")
        shellScript {
          content = """
          dotnet restore
          dotnet test
          """
        }
      }
}
}
