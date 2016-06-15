namespace NuProj.Tasks.AssemblyInfo
{
    using System.IO;

    public class AssemblyFiles
    {
        public AssemblyFile Main { get;}
        public AssemblyFile Version { get;}

        public AssemblyFiles(ProjectMetadata project)
        {
            Main = new AssemblyFile(Path.Combine(project.DirectoryName, "Properties", "AssemblyInfo.cs"));
            Version = new AssemblyFile(Path.Combine(project.DirectoryName, "AssemblyVersionInfo.cs"));
        }
    }
}

