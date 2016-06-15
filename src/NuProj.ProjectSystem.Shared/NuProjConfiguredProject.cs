﻿using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.ProjectSystem;
#if Dev12 || Dev14
using Microsoft.VisualStudio.ProjectSystem.Utilities;
#endif

namespace NuProj.ProjectSystem
{
    [Export]
#if Dev12
    [PartMetadata(ProjectCapabilities.Requires, NuProjCapabilities.NuProj)]
#else
    [AppliesTo(NuProjCapabilities.NuProj)]
#endif
    internal sealed class NuProjConfiguredProject
    {
        [Import]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "MEF")]
        public ConfiguredProject ConfiguredProject { get; private set; }

        [Import]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "MEF")]
        public Lazy<NuProjProjectProperties> Properties { get; private set; }
    }
}
