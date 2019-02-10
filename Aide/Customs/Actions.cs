using Aide.Models;
using System.Collections.Generic;

namespace Aide.Customs {
  public delegate void CustomizedRowActionDelegate(int cid, List<KeyValuePair<string, object>> identifiers);
  public delegate void CustomizedTableActionDelegate(AideBaseFilterIndexModel fiModel, Dictionary<string, string> filters);
}
