using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTTE.UI.Common;
using LibraryUtils = RTTE.Library.Common.Utils;

namespace RTTE.UI {
    public partial class AboutForm : Form {
        private static class Constants {
            internal const string CopyrightURL = "https://creativecommons.org/licenses/by-nc-nd/4.0/";
            internal const string Pixelophilia2IconsetURL = "https://www.deviantart.com/omercetin/art/PixeloPhilia2-166570194";
            internal const string OmercetinURL = "https://www.deviantart.com/omercetin";
            internal const string GithubURL = "https://github.com/Intragalactical";
            internal const string GitlabURL = "https://gitlab.com/Intragalactical";
        }

        public AboutForm() {
            InitializeComponent();

            string platform = LibraryUtils.GetCurrentArchitecture().ToString();
            Text = string.Format(Text, platform);
            VersionLabel.Text = string.Format(VersionLabel.Text, Assembly.GetExecutingAssembly().GetName().Version.ToString(), platform);
            GithubLink.Text = string.Format(GithubLink.Text, Constants.GithubURL);
            GitlabLink.Text = string.Format(GitlabLink.Text, Constants.GitlabURL);

            HelpTooltip.SetToolTip(CopyrightLink, Constants.CopyrightURL);
            HelpTooltip.SetToolTip(Pixelophilia2IconsetLink, Constants.Pixelophilia2IconsetURL);
            HelpTooltip.SetToolTip(OmercetinLink, Constants.OmercetinURL);
        }

        private void OkButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void CopyrightLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            _ = Utils.OpenInDefaultBrowser(Constants.CopyrightURL);
        }

        private void Pixelophilia2IconsetLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            _ = Utils.OpenInDefaultBrowser(Constants.Pixelophilia2IconsetURL);
        }

        private void OmercetinLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            _ = Utils.OpenInDefaultBrowser(Constants.OmercetinURL);
        }

        private void GithubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            _ = Utils.OpenInDefaultBrowser(Constants.GithubURL);
        }

        private void GitlabLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            _ = Utils.OpenInDefaultBrowser(Constants.GitlabURL);
        }
    }
}
