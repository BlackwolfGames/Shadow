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
val sharedVolume = volume("shared-volume")

job("Build and Test Rust and C# Projects") {
  // Container for Rust
  container(displayName = "Rust Container", image = "rust:latest") {
    volumeMount(sharedVolume, "/shared")
    shellScript {
      content = """
      cd shadow_rust
      cargo build --release --verbose
      cargo test --verbose
      cp target/release/mylib.so /shared/
      """
    }
  }

  // Container for C#
  container(displayName = "C# Container", image = "mcr.microsoft.com/dotnet/sdk:latest") {
    volumeMount(sharedVolume, "/shared")
    shellScript {
      content = """
      cp /shared/mylib.so MyCSProject/libs/
      dotnet restore
      dotnet test
      """
    }
  }
}
}
