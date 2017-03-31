using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace Chronos
{
    [Serializable]
    public sealed class EnvironmentInformation : IEquatable<EnvironmentInformation>
    {
        public EnvironmentInformation()
        {
            Refresh();
        }

        public string MachineName { get; private set; }

        public int ProcessId { get; private set; }

        public string AppDomainName { get; private set; }

        public string PhysicalAddress { get; private set; }

        private void Refresh()
        {
            MachineName = GetMachineName();
            AppDomainName = GetAppDomainName();
            ProcessId = GetProcessId();
            PhysicalAddress = GetPhysicalAddress();
        }

        private static string GetMachineName()
        {
            return Environment.MachineName;
        }

        private static string GetAppDomainName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        private static int GetProcessId()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                return process.Id;
            }
        }

        private static string GetPhysicalAddress()
        {
            NetworkInterfaceType[] types = new []
            {
                NetworkInterfaceType.Ethernet,
                NetworkInterfaceType.Ethernet3Megabit,
                NetworkInterfaceType.FastEthernetT,
                NetworkInterfaceType.FastEthernetFx,
                NetworkInterfaceType.GigabitEthernet,
                NetworkInterfaceType.Wireless80211,
                NetworkInterfaceType.TokenRing,
                NetworkInterfaceType.Fddi,
                NetworkInterfaceType.BasicIsdn,
                NetworkInterfaceType.PrimaryIsdn,
                NetworkInterfaceType.Ppp,
                NetworkInterfaceType.Slip,
                NetworkInterfaceType.Atm,
                NetworkInterfaceType.GenericModem,
                NetworkInterfaceType.Isdn,
                NetworkInterfaceType.AsymmetricDsl,
                NetworkInterfaceType.RateAdaptDsl,
                NetworkInterfaceType.SymmetricDsl,
                NetworkInterfaceType.VeryHighSpeedDsl,
                NetworkInterfaceType.IPOverAtm,
                NetworkInterfaceType.MultiRateSymmetricDsl,
                NetworkInterfaceType.HighPerformanceSerialBus,
                //NetworkInterfaceType.Loopback,
                //NetworkInterfaceType.Tunnel,
            };
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterfaceType type in types)
            {
                NetworkInterfaceType temp = type;
                NetworkInterface networkInterface = networkInterfaces.FirstOrDefault(x => x.NetworkInterfaceType == temp);
                if (networkInterface == null || networkInterface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }
                return networkInterface.GetPhysicalAddress().ToString();
            }
            return string.Empty;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(EnvironmentInformation))
            {
                return false;
            }
            EnvironmentInformation other = (EnvironmentInformation) obj;
            return Equals(other);
        }

        public bool Equals(EnvironmentInformation other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (!string.Equals(PhysicalAddress, other.PhysicalAddress, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (ProcessId != other.ProcessId)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (PhysicalAddress != null ? PhysicalAddress.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
