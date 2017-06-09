using System;
using System.Runtime.InteropServices;

namespace slackme.Utils
{
    [Flags]
    public enum ErrorModes
    {
        Default = 0x0,
        FailCriticalErrors = 0x1,
        NoGpFaultErrorBox = 0x2,
    }

    public struct ChangeErrorMode : IDisposable
    {
        private readonly int _oldMode;

        public ChangeErrorMode(ErrorModes mode)
        {
            _oldMode = (int)ErrorModes.Default;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _oldMode = SetErrorMode((int)mode);
            }
        }

        void IDisposable.Dispose()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetErrorMode(_oldMode);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern int SetErrorMode(int newMode);
    }
}
