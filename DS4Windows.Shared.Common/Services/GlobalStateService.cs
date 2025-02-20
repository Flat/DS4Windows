﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DS4Windows.Shared.Common.Core;
using DS4Windows.Shared.Common.Telemetry;

namespace DS4Windows.Shared.Common.Services
{
    /// <summary>
    ///     Provides global properties that can change during runtime but will not be persisted to or restored from disk.
    /// </summary>
    public sealed class GlobalStateService : IGlobalStateService
    {
        private readonly ActivitySource activitySource = new(TracingSources.CommonAssemblyActivitySourceName);

        private readonly string appDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

        public GlobalStateService()
        {
            using var activity = activitySource.StartActivity(
                $"{nameof(GlobalStateService)}:Constructor");
        }

        /// <summary>
        ///     Absolute path to <see cref="Constants.ProfilesSubDirectory" />
        /// </summary>
        public string ProfilesDirectory
        {
            get
            {
                var programFolderFile = Path.Combine(appDirectory
                    , Constants.ProfilesSubDirectory);

                return File.Exists(programFolderFile)
                    ? programFolderFile
                    : Path.Combine(RoamingAppDataPath, Constants.ProfilesSubDirectory);
            }
        }

        /// <summary>
        ///     Absolute path to <see cref="Constants.LegacyProfilesFileName" />
        /// </summary>
        public string AppSettingsFilePath
        {
            get
            {
                var programFolderFile = Path.Combine(appDirectory
                    , Constants.LegacyProfilesFileName);

                return File.Exists(programFolderFile)
                    ? programFolderFile
                    : Path.Combine(RoamingAppDataPath, Constants.LegacyProfilesFileName);
            }
        }

        /// <summary>
        ///     Absolute path to <see cref="Constants.LegacyLinkedProfilesFileName" />
        /// </summary>
        public string LinkedProfilesPath
        {
            get
            {
                var programFolderFile = Path.Combine(appDirectory
                    , Constants.LegacyLinkedProfilesFileName);

                return File.Exists(programFolderFile)
                    ? programFolderFile
                    : Path.Combine(RoamingAppDataPath, Constants.LegacyLinkedProfilesFileName);
            }
        }

        /// <summary>
        ///     Absolute path to <see cref="Constants.LegacyAutoProfilesFileName" />
        /// </summary>
        public string AutoSwitchingProfilesPath
        {
            get
            {
                var programFolderFile = Path.Combine(appDirectory
                    , Constants.LegacyAutoProfilesFileName);

                return File.Exists(programFolderFile)
                    ? programFolderFile
                    : Path.Combine(RoamingAppDataPath, Constants.LegacyAutoProfilesFileName);
            }
        }

        /// <inheritdoc />
        public event Action StartupTasksCompleted;

        /// <inheritdoc />
        public void InvokeStartupTasksCompleted()
        {
            StartupTasksCompleted?.Invoke();
        }

        /// <summary>
        ///     Absolute path to roaming application directory in current user profile.
        /// </summary>
        public string RoamingAppDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Constants.ApplicationName);
    }
}