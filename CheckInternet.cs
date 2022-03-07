//
// Mark König, 03/2022
//

using System;
using System.Runtime.InteropServices;

namespace LoadBingPicture
{
    public class CheckInternet
    {
        #region windows stuff

        // for the I-Net check
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern int InternetAttemptConnect(uint res);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetConnectedState(out int flags, int reserved);

        #endregion

        private static int ERROR_SUCCESS = 0;

        public static bool IsInternetConnected()
        {
            int dwConnectionFlags = 0;
            if (!InternetGetConnectedState(out dwConnectionFlags, 0))
                return false;

            if (InternetAttemptConnect(0) != ERROR_SUCCESS)
                return false;

            return true;
        }
    }
}
