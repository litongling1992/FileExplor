using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class TestCurrentOs
    {
        // Only tested for one 64 bit Windows7 PC, not other operating systems
        public static bool IsWindows7()
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            System.PlatformID platform =osInfo.Platform;
            int majorVersion = osInfo.Version.Major;
            int minorVersion = osInfo.Version.Minor;
            return ( (platform==System.PlatformID.Win32NT) && (majorVersion==6) && (minorVersion>=1));
        }
    }
}
