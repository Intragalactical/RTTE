using RTTE.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTTE.UI.Entities {
    public sealed class ProcessEntity {
        [Browsable(false)]
        public IAdvancedProcess Process { get; }

        public Image ImageUnsafe => Process.GetPreviewImage().MatchUnsafe(image => image, () => null); // @TODO: fix null, add default image!
        public string Name => Process.GetName();
        public int Id => Process.GetId();
        public string ArchitectureString => Process.GetArchitectureString();
        public string MainWindowTitle => Process.GetMainWindowTitle()
            .Match(mainWindowTitle => mainWindowTitle, "");

        private ProcessEntity(IAdvancedProcess process) {
            Process = process;
        }

        public static ProcessEntity Create(IAdvancedProcess process)
            => new(process);
    }
}
