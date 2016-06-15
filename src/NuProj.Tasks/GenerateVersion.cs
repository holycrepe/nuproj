using System;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NuProj.Tasks
{
    using System.Linq;

    using NuProj.Tasks.AssemblyInfo;
    using NuProj.Tasks.Extensions;

    public class GenerateVersion : Task
    {

        [Required]
        public string AssemblyName { get; set; }

        [Output]
        public string TargetName
            => $"{AssemblyName}.{Version}";

        [Output]
        public string Version { get; set; }

        public ITaskItem[] Files { get; set; }

        public override bool Execute()
        {
            try
            {
                GetVersion();
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                Log.LogErrorFromException(ex);
            }

            return !Log.HasLoggedErrors;
        }
        private void GetVersion()
        {
            if (this.Version != "$version$" && !string.IsNullOrEmpty(this.Version))
            {
                Log.LogMessageFromText($"Using explicitly provided version {Version}",
                                       MessageImportance.Low);
                return;
            }

            var assembly = GetMainAssembly();
            if (assembly == null)
            {
                Log.LogError("Unable to automatically generate version: "
                             + "Ensure main project contains valid 'AssemblyVersionInfo.cs' or 'Properties/AssemblyInfo.cs' file, with an 'AssemblyVersion' attribute");
                return;
            }
            Version = assembly.Version.Main.ToString();
            Log.LogMessageFromText($"Generated version {Version} from main project {assembly.Project.Name}",
                                   MessageImportance.High);
        }

        private ProjectAssembly GetMainAssembly()
            => this.Files.Select(p => new ProjectAssembly(p.ItemSpec)).FirstOrDefault(p => p.HasMainVersion);
    }
}