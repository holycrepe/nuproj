namespace NuProj.Tasks.AssemblyInfo
{
    using System.Collections.Generic;

    using NuProj.Tasks.Extensions.Collections;

    public class ProjectAssembly
    {
        public ProjectMetadata Project { get; }

        public AssemblyFiles Assembly { get;}

        public IDictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public AssemblyVersions Version { get; }
        public bool HasMainVersion { get; }
        
        public string GetAttribute(string name)
            => Attributes.ContainsKey(name)
                            ? Attributes[name]
                            : null; 

        public ProjectAssembly(string projectFileName)
        {
            Project = new ProjectMetadata(projectFileName);
            Assembly = new AssemblyFiles(Project);
            Attributes = Assembly.Main.Attributes.Update(Assembly.Version.Attributes);
            Version = new AssemblyVersions(this);
            HasMainVersion = Version.Main != null;
        }
    }
}

