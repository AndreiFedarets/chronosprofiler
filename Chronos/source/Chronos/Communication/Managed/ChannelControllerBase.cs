using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Security.Principal;

namespace Chronos.Communication.Managed
{
    internal abstract class ChannelControllerBase : IChannelController
    {
        private IChannel _channel;

        public void CreateChannel()
        {
            if (_channel != null)
            {
                return;
            }
            _channel = CreateChannelInternal();
            try
            {
                ChannelServices.RegisterChannel(_channel, true);
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                throw new ChannelRegistrationException(string.Format("Unable to register channel '{0}'", _channel.ChannelName), exception);
            }
        }

        public void Dispose()
        {
            if (_channel == null)
            {
                return;
            }
            try
            {
                ChannelServices.UnregisterChannel(_channel);
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Warning, exception);
            }
            finally
            {
                _channel = null;
            }
        }

        protected abstract IChannel CreateChannelInternal();

        internal static string GetAuthorizedGroup()
        {
            // Get SID code for the EveryOne user
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            // Get the NT account related to the SID
            NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;
            return account == null ? string.Empty : account.Value;
        }

    }
}
