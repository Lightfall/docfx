// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.Common.Lfs
{
    using System;
    using System.Collections.Immutable;

    public class PathMapping
    {
        public PathMapping(RelativePath logicPath, string physicPath)
            : this(logicPath, physicPath, null) { }

        public PathMapping(RelativePath logicPath, string physicPath, ImmutableDictionary<string, object> properties)
        {
            if (logicPath == null)
            {
                throw new ArgumentNullException(nameof(logicPath));
            }
            if (physicPath == null)
            {
                throw new ArgumentNullException(nameof(physicPath));
            }
            LogicPath = logicPath.GetPathFromWorkingFolder();
            LogicPathText = LogicPath.ToString();
            PhysicPath = physicPath;
            Properties = properties ?? ImmutableDictionary<string, object>.Empty;
        }

        public RelativePath LogicPath { get; }
        public string LogicPathText { get; }
        public string PhysicPath { get; }
        public ImmutableDictionary<string, object> Properties { get; }
    }
}
