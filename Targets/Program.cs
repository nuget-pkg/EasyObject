using static Bullseye.Targets;
using static SimpleExec.Command;

Target("sed",  () => RunAsync("bash", "-c '_sed.sh'"));
Target("build", dependsOn: ["sed"], () => RunAsync("dotnet", "build -p:Configuration=Release"));
Target("test", dependsOn: ["build"], () => RunAsync("dotnet", "test --configuration Release --no-build"));
Target("pack", dependsOn: ["test"], () => RunAsync("dotnet", "pack -o . -p:Configuration=Release"));
Target("release", dependsOn: ["pack"], () => RunAsync("bash", "-c '_release.sh'"));
Target("default", dependsOn: ["test"]);

await RunTargetsAndExitAsync(args, ex => ex is SimpleExec.ExitCodeException);
