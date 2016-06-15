using System;

namespace NuProj.Tasks.AssemblyInfo
{


    public class AssemblyVersions
    {
        public Version Main { get; }
        public Version File { get; }
        public Version Informational { get; }
        public static Version ParseVersion(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
            Version ver = null;
            if (!Version.TryParse(input, out ver))
                Console.WriteLine($"Unable to determine the version from '{input}'.");
            return ver;
        }
        public AssemblyVersions(ProjectAssembly info)
        {
            Main = ParseVersion(info.GetAttribute("AssemblyVersion"));
            File = ParseVersion(info.GetAttribute("AssemblyFileVersion"));
            Informational = ParseVersion(info.GetAttribute("AssemblyInformationalVersion"));
        }
    }
}
