using System;
using System.Runtime.InteropServices;

namespace Aide.Winforms.Helpers {
  public class FunctionHelper {
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr LockWindowUpdate(IntPtr Handle);
  }
}
