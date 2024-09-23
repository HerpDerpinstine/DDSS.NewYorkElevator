using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(DDSS_NewYorkElevator.Properties.BuildInfo.Description)]
[assembly: AssemblyDescription(DDSS_NewYorkElevator.Properties.BuildInfo.Description)]
[assembly: AssemblyCompany(DDSS_NewYorkElevator.Properties.BuildInfo.Company)]
[assembly: AssemblyProduct(DDSS_NewYorkElevator.Properties.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + DDSS_NewYorkElevator.Properties.BuildInfo.Author)]
[assembly: AssemblyTrademark(DDSS_NewYorkElevator.Properties.BuildInfo.Company)]
[assembly: AssemblyVersion(DDSS_NewYorkElevator.Properties.BuildInfo.Version)]
[assembly: AssemblyFileVersion(DDSS_NewYorkElevator.Properties.BuildInfo.Version)]
[assembly: MelonInfo(typeof(DDSS_NewYorkElevator.MelonMain), 
    DDSS_NewYorkElevator.Properties.BuildInfo.Name, 
    DDSS_NewYorkElevator.Properties.BuildInfo.Version,
    DDSS_NewYorkElevator.Properties.BuildInfo.Author,
    DDSS_NewYorkElevator.Properties.BuildInfo.DownloadLink)]
[assembly: MelonGame("StripedPandaStudios", "DDSS")]
[assembly: MelonPriority(int.MinValue)]
[assembly: VerifyLoaderVersion("0.6.5", true)]
[assembly: HarmonyDontPatchAll]