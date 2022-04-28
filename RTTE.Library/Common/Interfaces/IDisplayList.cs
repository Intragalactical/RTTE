using System.Collections.Generic;

namespace RTTE.Library.Common.Interfaces;

public interface IDisplayList {
    public IEnumerable<IDisplay> Get();
}
