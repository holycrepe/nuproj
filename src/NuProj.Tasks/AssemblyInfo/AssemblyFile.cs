namespace NuProj.Tasks.AssemblyInfo
{
    using System.Collections.Generic;
    using System.IO;

    public class AssemblyFile
    {
        public FileInfo FileInfo { get; }

        public string Contents { get; } = string.Empty;
        public Dictionary<string,string> Attributes { get; } = new Dictionary<string, string>();

        public AssemblyFile(string fileName)
        {
            FileInfo = new FileInfo(fileName);
            if (!FileInfo.Exists)
                return;
            Contents = File.ReadAllText(FileInfo.FullName);
            Attributes = AssemblyAttributes.ParseAllAttributes(Contents);
        }
    }
}