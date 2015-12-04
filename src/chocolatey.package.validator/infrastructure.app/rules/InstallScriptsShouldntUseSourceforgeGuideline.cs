﻿// Copyright © 2015 - Present RealDimensions Software, LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//
// 	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace chocolatey.package.validator.infrastructure.app.rules
{
    using System.IO;
    using infrastructure.rules;
    using NuGet;

    public class InstallScriptsShouldntUseSourceforgeGuideline : BasePackageRule
    {
        public override string ValidationFailureMessage
        {
            get
            {
                return
                    @"Using SourceForge as the download source of installers is not recommended.  Please consider using an alternative download location if one is available (as long as it is still an official distribution point). You may not be able to change this and that is okay.  This can also give a false positive for commented code with the words:
  * sourceforge";
            }
        }

        protected override PackageValidationOutput is_valid(IPackage package)
        {
            var valid = true;

            var files = package.GetFiles().or_empty_list_if_null();

            foreach (var packageFile in files)
            {
                string extension = Path.GetExtension(packageFile.Path).to_lower();
                if (extension != ".ps1" && extension != ".psm1") continue;

                var contents = packageFile.GetStream().ReadToEnd().to_lower();

                if (contents.Contains("sourceforge")) valid = false;
            }

            return valid;
        }
    }
}
