using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanguageExt;
using Microsoft.Win32;
using RTTE.Library.Common;
using RTTE.Library.Process;

namespace RTTE.UI.Common {
    internal static class Utils {
        private static Option<string> GetDefaultbrowser() {
            Option<RegistryKey> browsers = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet\");
            Option<string> firstBrowser = browsers.Match(b => b.GetSubKeyNames().FirstOrDefault(), Option<string>.None);

            static string parsePath(object value)
                => value.ToString().Replace("\"", "");

            static Option<string> getBrowserPath(Option<RegistryKey> browsers, string firstBrowser) {
                Option<RegistryKey> final = browsers
                    .Match(b => b.OpenSubKey(@$"{firstBrowser}\shell\open\command"), Option<RegistryKey>.None);
                Option<object> value = final.Match<Option<object>>(f => f.GetValue(""), Option<object>.None);
                return value.Match(v => parsePath(v), Option<string>.None);
            }

            return firstBrowser.Match(f => getBrowserPath(browsers, f), Option<string>.None);
        }

        internal static Option<Process> OpenInDefaultBrowser(Option<string> url)
            => url.Match(url =>
                GetDefaultbrowser()
                    .Match(defaultBrowser => Process.Start(new ProcessStartInfo(defaultBrowser, url)), Option<Process>.None),
                () => Option<Process>.None
            );
    }
}
