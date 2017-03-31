using System.Collections.Generic;
using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class ConfigurationSettingsMarshaller : GenericMarshaler<ConfigurationSettings>
	{
		protected override void MarshalInternal(ConfigurationSettings value, Stream stream)
		{
			StringMarshaler.Marshal(value.Name, stream);
			GuidMarshaler.Marshal(value.Token, stream);
			ByteMarshaler.Marshal((byte)value.InitialState, stream);
			Int32Marshaler.Marshal((int)value.Events, stream);
			StringMarshaler.Marshal(value.TargetProcessName, stream);
            StringMarshaler.Marshal(value.ProcessTargetArguments, stream);
            BoolMarshaler.Marshal(value.ProfileChildProcess, stream);
			ByteMarshaler.Marshal((byte)value.FilterType, stream);
            BoolMarshaler.Marshal(value.UseFastHooks, stream);
            BoolMarshaler.Marshal(value.ProfileSql, stream);
			MarshalingManager.Marshal(value.FilterItems, stream);
		}

		protected override ConfigurationSettings DemarshalInternal(Stream stream)
		{
			ConfigurationSettings configurationSettings = new ConfigurationSettings();
			configurationSettings.Name = StringMarshaler.Demarshal(stream);
			configurationSettings.Token = GuidMarshaler.Demarshal(stream);
			configurationSettings.InitialState = (SessionState)ByteMarshaler.Demarshal(stream);
			configurationSettings.Events = (ClrEventsMask)Int32Marshaler.Demarshal(stream);
			configurationSettings.TargetProcessName = StringMarshaler.Demarshal(stream);
			configurationSettings.ProcessTargetArguments = StringMarshaler.Demarshal(stream);
            configurationSettings.ProfileChildProcess = BoolMarshaler.Demarshal(stream);
			configurationSettings.FilterType = (FilterType)ByteMarshaler.Demarshal(stream);
            configurationSettings.UseFastHooks = BoolMarshaler.Demarshal(stream);
            configurationSettings.ProfileSql = BoolMarshaler.Demarshal(stream);
			configurationSettings.FilterItems = MarshalingManager.Demarshal<List<string>>(stream);
			return configurationSettings;
		}
	}
}
