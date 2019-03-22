
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TransparentWindowsForms
{
    public class DetectHelper
    {

        [DllImport(@"QH_ObjDetect.dll")]
        //private static extern void Init();
        public static extern void QH_Init(int nW, int nH);
        [DllImport(@"QH_ObjDetect.dll")]
        public static extern void QHGSForJson(byte[] fileCode, int nFileLen, byte[] jsonResult, ref int jsonLen);
        [DllImport(@"QH_ObjDetect.dll", EntryPoint = "QHGSForDetect")]
        public static extern int QHGSForDetect(byte[] fileCode, int nFileLen, IntPtr result);

    }
}
