using JToolbox.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace JToolbox.Core.Utilities
{
    public static class NetworkUtils
    {
        public static List<IPAddress> GetLocalIPAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .ToList();
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var unicastAddresses in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastAddresses.Address.AddressFamily == AddressFamily.InterNetwork && unicastAddresses.Address.ToString() == address.ToString())
                    {
                        return unicastAddresses.IPv4Mask;
                    }
                }
            }
            return null;
        }

        public static bool ConnectedToLocalNetwork()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        public static bool ConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<IPAddress> GetGatewayAddresses()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a?.AddressFamily == AddressFamily.InterNetwork && Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
                .ToList();
        }

        public static string GetHostName(IPAddress ipAddress)
        {
            try
            {
                return Dns.GetHostEntry(ipAddress)?.HostName;
            }
            catch (SocketException)
            {
                return null;
            }
        }

        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
        {
            var ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            var ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            var broadCastIpAddress = ipAddress | ~ipMaskV4;
            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }

        public static IPAddress GetNetworkAddress(IPAddress address, IPAddress mask)
        {
            var ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            var ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            var networkIpAddress = ipAddress & ipMaskV4;
            return new IPAddress(BitConverter.GetBytes(networkIpAddress));
        }

        public static void RemoveBroadcastNetworkAddresses(List<IPAddress> addresses, IPAddress mask)
        {
            for (int i = addresses.Count - 1; i >= 0; i--)
            {
                var address = addresses[i];
                if (address == GetBroadcastAddress(address, mask) || address == GetNetworkAddress(address, mask))
                {
                    addresses.Remove(address);
                }
            }
        }

        public static List<IPAddress> GetAddressesInNetwork(IPAddress address, IPAddress mask)
        {
            var start = GetNetworkAddress(address, mask).Add(1);
            var end = GetBroadcastAddress(address, mask).Add(-1);
            return GetContinousAddressesInRange(start, end);
        }

        public static bool IsInSameSubnet(IPAddress address1, IPAddress address2, IPAddress subnetMask)
        {
            IPAddress network1 = GetNetworkAddress(address1, subnetMask);
            IPAddress network2 = GetNetworkAddress(address2, subnetMask);
            return network1.Equals(network2);
        }

        public static IPAddress FirstAddressInSubnet(IPAddress address, IPAddress subnetMask)
        {
            var networkAddress = GetNetworkAddress(address, subnetMask);
            var addressBytes = networkAddress.GetAddressBytes();
            return new IPAddress(new byte[] { addressBytes[0], addressBytes[1], addressBytes[2], (byte)(addressBytes[3] + 1) });
        }

        public static IPAddress LastAddressInSubnet(IPAddress address, IPAddress subnetMask)
        {
            var networkAddress = GetBroadcastAddress(address, subnetMask);
            var addressBytes = networkAddress.GetAddressBytes();
            return new IPAddress(new byte[] { addressBytes[0], addressBytes[1], addressBytes[2], (byte)(addressBytes[3] - 1) });
        }

        public static List<IPAddress> GetAddressesInRange(IPAddress startAddress, IPAddress endAddress)
        {
            var startIp = startAddress.GetAddressBytes();
            var endIp = endAddress.GetAddressBytes();

            var startIp0 = Math.Min(startIp[0], endIp[0]);
            var startIp1 = Math.Min(startIp[1], endIp[1]);
            var startIp2 = Math.Min(startIp[2], endIp[2]);
            var startIp3 = Math.Min(startIp[3], endIp[3]);

            var endIp0 = Math.Max(startIp[0], endIp[0]);
            var endIp1 = Math.Max(startIp[1], endIp[1]);
            var endIp2 = Math.Max(startIp[2], endIp[2]);
            var endIp3 = Math.Max(startIp[3], endIp[3]);

            int capacity = 1;
            for (int i = 0; i < 4; i++)
            {
                capacity *= endIp[i] - startIp[i] + 1;
            }

            var ips = new List<IPAddress>(capacity);
            for (int i0 = startIp0; i0 <= endIp0; i0++)
            {
                for (int i1 = startIp1; i1 <= endIp1; i1++)
                {
                    for (int i2 = startIp2; i2 <= endIp2; i2++)
                    {
                        for (int i3 = startIp3; i3 <= endIp3; i3++)
                        {
                            ips.Add(new IPAddress(new byte[] { (byte)i0, (byte)i1, (byte)i2, (byte)i3 }));
                        }
                    }
                }
            }

            return ips;
        }

        public static List<IPAddress> GetContinousAddressesInRange(IPAddress startAddress, IPAddress endAddress)
        {
            var result = new List<IPAddress>();
            IPAddress end;
            IPAddress start;
            if (startAddress.Compare(endAddress) < 0)
            {
                start = startAddress;
                end = endAddress;
            }
            else
            {
                end = startAddress;
                start = endAddress;
            }

            while (start.Compare(end) <= 0)
            {
                result.Add(start);
                start = start.Add(1);
            }
            return result;
        }

        public static bool IsInRange(IPAddress address, IPAddress startAddress, IPAddress endAddress)
        {
            return address.Compare(startAddress) >= 0
                && address.Compare(endAddress) <= 0;
        }

        public static List<int> GetActiveTcpConnections()
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            return properties.GetActiveTcpConnections()
                .Select(c => c.LocalEndPoint.Port)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        public static List<int> GetOpenTcpPorts()
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            return properties.GetActiveTcpListeners()
                .Select(c => c.Port)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        public static List<int> GetOpenUdpPorts()
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            return properties.GetActiveUdpListeners()
                .Select(c => c.Port)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }
    }
}