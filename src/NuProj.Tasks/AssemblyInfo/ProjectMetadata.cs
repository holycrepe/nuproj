namespace NuProj.Tasks.AssemblyInfo
{
    using System.IO;

    public class ProjectMetadata
    {
        public DirectoryInfo Directory { get; }
        public string DirectoryName { get; }
        public string Name { get; }
        public FileInfo File { get; }
        public string FileName { get; }

        public ProjectMetadata(string projectFilePath)
        {
            FileName = projectFilePath;
            File = new FileInfo(FileName);
            Name = Path.GetFileNameWithoutExtension(FileName);
            Directory = File.Directory;
            DirectoryName = Directory.FullName;
        }
    }
}