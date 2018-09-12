using System;
using System.Collections.Specialized;

namespace Chronos
{
    /// <summary>
    /// Represents settings of configuration.
    /// This class contains basic information about configuration,
    /// list of extensions to be loaded in agent / daemon and settings for these extensions.
    /// </summary>
    [Serializable]
    public sealed class ConfigurationSettings// : DynamicSettings
    {
        private Guid _configurationUid;

        public ConfigurationSettings(Guid profilingTargetUid)
        {
            ProfilingTargetSettings = new ProfilingTargetSettings(profilingTargetUid);
            FrameworksSettings = new FrameworkSettingsCollection();
            ProfilingTypesSettings = new ProfilingTypeSettingsCollection();
            GatewaySettings = new GatewaySettings();
            ConfigurationUid = Guid.NewGuid();
        }

        private ConfigurationSettings(Guid configurationUid, ProfilingTargetSettings profilingTargetSettings,
            FrameworkSettingsCollection frameworksSettings, ProfilingTypeSettingsCollection profilingTypesSettings, GatewaySettings gatewaySettings)
        {
            ProfilingTargetSettings = profilingTargetSettings;
            FrameworksSettings = frameworksSettings;
            ProfilingTypesSettings = profilingTypesSettings;
            GatewaySettings = gatewaySettings;
            ConfigurationUid = configurationUid;
        }

        public uint GatewaySyncStreams { get; set; }

        /// <summary>
        /// Get display name of configuration. 
        /// This name is provided by user when configuration creating.
        /// </summary>
        public string ConfigurationName { get; set; }

        /// <summary>
        /// Configuration unique identifier
        /// </summary>
        public Guid ConfigurationUid
        {
            get { return _configurationUid; }
            set
            {
                _configurationUid = value;
                StringDictionary dictionary = ProfilingTargetSettings.EnvironmentVariables;
                dictionary[Constants.EnvironmentVariablesNames.ConfigurationToken] = ConfigurationUid.ToString();
                ProfilingTargetSettings.EnvironmentVariables = dictionary;
            }
        }

        public ProfilingTargetSettings ProfilingTargetSettings { get; private set; }

        public FrameworkSettingsCollection FrameworksSettings { get; private set; }

        public ProfilingTypeSettingsCollection ProfilingTypesSettings { get; private set; }

        public GatewaySettings GatewaySettings { get; private set; }

        public ConfigurationSettings Clone()
        {
            ProfilingTargetSettings profilingTargetSettings = (ProfilingTargetSettings)ProfilingTargetSettings.Clone();
            FrameworkSettingsCollection frameworksSettings = FrameworksSettings.Clone();
            ProfilingTypeSettingsCollection profilingTypesSettings = ProfilingTypesSettings.Clone();
            GatewaySettings gatewaySettings = (GatewaySettings)GatewaySettings.Clone();

            ConfigurationSettings settings = new ConfigurationSettings(ConfigurationUid, profilingTargetSettings,
                frameworksSettings, profilingTypesSettings, gatewaySettings);
            return settings;
        }

        public void Validate()
        {
            if (ConfigurationUid == Guid.Empty)
            {
                throw new TempException();
            }
            ProfilingTargetSettings.Validate();
            FrameworksSettings.Validate();
            ProfilingTypesSettings.Validate();
        }
    }
}
