using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Chronos.Communication.Managed
{
    public sealed class ManagedConnector
    {
        private readonly List<IChannelController> _availableChannels;

        internal ManagedConnector()
        {
            _availableChannels = new List<IChannelController>();
        }

        public ObjRef Share(MarshalByRefObject service, Guid uid)
        {
            return Share(service, uid, service.GetType());
        }

        public ObjRef Share(MarshalByRefObject service, Guid uid, Type type)
        {
            return RemotingServices.Marshal(service, uid.ToString(), type);
        }

        public T Connect<T>(ConnectionSettings settings)
        {
            return (T)Connect(settings, typeof (T));
        }

        public object Connect(ConnectionSettings settings, Type serviceType)
        {
            if (settings == null)
            {
                throw new InvalidConnectionSettingsException("Connection settings cannot be null");
            }
            ValidateChannelSettings(settings.ChannelSettings);
            object service = Activator.GetObject(serviceType, settings.ObjectUri);
            return service;
        }

        public void CreateChannel(ChannelSettings settings)
        {
            ValidateChannelSettings(settings);
            IChannelController channelController = settings.CreateController();
            _availableChannels.Add(channelController);
            channelController.CreateChannel();
        }

        private void ValidateChannelSettings(ChannelSettings settings)
        {
            if (settings == null)
            {
                throw new InvalidChannelSettingsException("Channel settings cannot be null");
            }
            if (settings is IpcChannelSettings)
            {
                return;
            }
            if (settings is TcpChannelSettings)
            {
                return;
            }
            //if (settings is HttpChannelSetting)
            //{
            //  return;
            //}
            throw new InvalidChannelSettingsException(string.Format("Type {0} is not valid channel settings", settings.GetType()));
        }
    }
}
