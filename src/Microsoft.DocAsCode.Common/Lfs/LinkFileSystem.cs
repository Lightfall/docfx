// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.Common.Lfs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class LinkFileSystem
    {
        public ImmutableArray<PathMapping> Mapping { get; }
        public string OutputFolder { get; }
        public bool IsReadOnly => string.IsNullOrEmpty(OutputFolder);

        public LinkFileSystem(IEnumerable<PathMapping> mapping, string outputFolder)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }
            Mapping = mapping.ToImmutableArray();
            OutputFolder = outputFolder;
        }

        public string[] GetAllFiles()
        {
            return new string[0];
        }

        public bool Exist(string file)
        {
            return Exist((RelativePath)file);
        }

        public bool Exist(RelativePath file)
        {
            return FindPhysicPathNoThrow(file) != null;
        }

        public FileStream Open(string file, FileMode mode)
        {
            return Open((RelativePath)file, mode);
        }

        public FileStream Open(RelativePath file, FileMode mode)
        {
            string pp = FindPhysicPath(file);
            return File.Open(pp, mode);
        }

        private string FindPhysicPath(RelativePath file)
        {
            var physicPath = FindPhysicPathNoThrow(file);
            if (physicPath == null)
            {
                throw new FileNotFoundException("File not found.", file);
            }
            return physicPath;
        }

        private string FindPhysicPathNoThrow(RelativePath file)
        {
            var path = file.GetPathFromWorkingFolder();
            foreach (var m in Mapping)
            {
                var localPath = (path - m.LogicPath).RemoveWorkingFolder().ToString();
                var physicPath = Path.Combine(m.PhysicPath, localPath);
                if (File.Exists(physicPath))
                {
                    return physicPath;
                }
            }
            return null;
        }
    }
}
