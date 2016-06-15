﻿using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#if Dev12
using Microsoft.Collections.Immutable;
#else
using System.Collections.Immutable;
#endif
using Microsoft.VisualStudio.ProjectSystem;
#if Dev12 || Dev14
using Microsoft.VisualStudio.ProjectSystem.Designers;
using Microsoft.VisualStudio.ProjectSystem.Utilities;
#else
using Microsoft.VisualStudio.ProjectSystem.References;
#endif
using Microsoft.VisualStudio.Shell.Interop;

namespace NuProj.ProjectSystem
{
    [Export(typeof(IValidProjectReferenceChecker))]
#if Dev12 || Dev14
    [OrderPrecedence(1000)]
#else
    [Order(1000)]
#endif
#if Dev12
    [PartMetadata(ProjectCapabilities.Requires, NuProjCapabilities.NuProj)]
#else
    [AppliesTo(NuProjCapabilities.NuProj)]
#endif
    internal sealed class NuProjValidProjectReferenceChecker : IValidProjectReferenceChecker
    {
        // This import must be present so that this part applies to a specific project.
        [Import]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public UnconfiguredProject UnconfiguredProject { get; set; }

        public Task<SupportedCheckResult> CanAddProjectReferenceAsync(object referencedProject)
        {
            var result = CanAddProjectReference(referencedProject);
            return Task.FromResult(result);
        }

        public Task<CanAddProjectReferencesResult> CanAddProjectReferencesAsync(IImmutableSet<object> referencedProjects)
        {
            if (referencedProjects == null)
                throw new ArgumentNullException("referencedProjects");

            var results = ImmutableDictionary<object, SupportedCheckResult>.Empty;

            foreach (object referencedProject in referencedProjects)
            {
                var result = CanAddProjectReference(referencedProject);
                results = results.Add(referencedProject, result);
            }

            return Task.FromResult(new CanAddProjectReferencesResult(results, null));
        }

        public Task<SupportedCheckResult> CanBeReferencedAsync(object referencingProject)
        {
            return Task.FromResult(SupportedCheckResult.NotSupported);
        }

        private static SupportedCheckResult CanAddProjectReference(object referencedProject)
        {
            return CanReference(referencedProject)
                   ? SupportedCheckResult.Supported
                   : SupportedCheckResult.Unknown;
        }

        private static bool CanReference(object referencedProject)
        {
            var r = referencedProject as IVsProjectReference;
            return r != null && 
                   CanReference(r.FullPath);
        }

        private static bool CanReference(string path)
        {
            return IsNuProj(path) ||
                   HasGetNuGetOutputAndTargetFrameworkInformationTarget(path);
        }

        private static bool IsNuProj(string path)
        {
            return HasExtension(path, "." + NuProjPackage.ProjectExtension);
        }

        private static bool HasGetNuGetOutputAndTargetFrameworkInformationTarget(string path)
        {
            // Here we are cheating like no tomorrow. Ideally, we'd actually inspect in the project file and check
            // whether it actually has an MSBuild target named "GetNuGetOutputAndTargetFrameworkInformation".
            //
            // For now we'll just whitelist the known project files C#, VB and F#.
            return HasExtension(path, ".csproj") ||
                   HasExtension(path, ".vbproj") ||
                   HasExtension(path, ".fsproj");
        }

        private static bool HasExtension(string path, string extension)
        {
            return path != null &&
                   extension != null &&
                   path.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
