using System;
using System.Collections.Generic;
using System.Text;
#if WINDOWS_APP
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#elif GTK
#else
using System.Windows.Threading;
#endif

namespace Quack
{
    class Utilities
    {
        /// <summary>
        /// Dispatcher.BeginInvoke의 Wrapper함수
        /// </summary>
        /// 플랫폼 별로 함수가 다른 관계로 wrapper로 제공
        /// <param name="a">실행할 action</param>
        public static async void BeginInvoke(Action a)
        {
#if WINDOWS_APP
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                a();
            });
#elif GTK
#else
            Dispatcher.CurrentDispatcher.BeginInvoke(a);
#endif
        }
        private static int OSX_ID =
#if MONO
			6
#else
 (int)PlatformID.MacOSX
#endif
;
        public static bool IsOSX
        {
            get
            {
                return (int)Environment.OSVersion.Platform == OSX_ID;
            }
        }
        public static bool IsLinux
        {
            get
            {
                return IsUnix && !IsOSX;
            }
        }
        public static bool IsUnix
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }
        }
        public static bool IsWindows
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT;
            }
        }
    }
}
