using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


/*=================================================================
* Author: QUJUN
* CreatedTime: 2019/2/28 11:37:25
* Description: <功能描述> 
*
* Dear maintainer:
* 
* Once you are done trying to 'optimize' this routine,
* and have realized what a terrible mistake that was,
* please increment the following counter as a warning
* to the next guy:
* 
* total_hours_wasted_here = 42
*
===================================================================*/

namespace TransparentWindowsForms
{
    public class QH_DNN_Helper
    {

        //extern "C" __declspec(dllimport) int InitNet_DNN(char* datacfg, char* cfgfile, char* weightfile, float thresh = 0.05, float hier_thresh = 0.5, int picW = 416, int picH = 416, int nID = -1);
        //extern "C" __declspec(dllimport) void FreeNet(int nID);
        //extern "C" __declspec(dllimport) void QHGS_DNN(unsigned char* fileCode, int nFileLen, GSRESULTEX_DNN &GsRst, int nNetIdx);
        //extern "C" __declspec(dllimport) void QHGS2_DNN(unsigned char* pData, int nW, int nH, int nC, int nFileLen, GSRESULTEX_DNN &GsRst, int nNetIdx);

        ////CPU版本
        //extern "C" __declspec(dllexport) int QH_InitDnnNetCV(char* cfgFile, char* weightsFile, int nW, int nH, int nNetID = -1);
        //extern "C" __declspec(dllexport) void QH_DNN_Detect_CV(uchar* pData, int nW, int nH, int nC, GSRESULTEX_DNN &gs, int nNetID);

        //extern "C" __declspec(dllexport) int QHGSSharp_DNN(unsigned char* fileCode, int nFileLen, GSALGITEMIMGEX_DNN[] GsRst, int nNetIdx);
        //extern "C" __declspec(dllexport) int QHGSSharp2_DNN(unsigned char* pData, int nW, int nH, int nC, int nFileLen, GSALGITEMIMGEX_DNN* GsRst, int nNetIdx);

        [DllImport(@"QH_DNN.dll")]
        public static extern int InitNet_DNN(byte[] datacfg, byte[] cfgfile, byte[] weightfile, float thresh = 0.05f, float hier_thresh = 0.5f, int picW = 416, int picH = 416, int nID = -1);
        [DllImport(@"QH_DNN.dll")]
        public static extern void FreeNet(int nID);
        [DllImport(@"QH_DNN.dll")]
        public static extern int QHGSSharp_DNN(byte[] fileCode, int nFileLen,[In,Out] GSALGITEMIMGEX_DNN[] GsRst, int nNetIdx);

        [DllImport(@"QH_DNN.dll")]
        public static extern  int QHGSSharp2_DNN(byte[] pData, int nW, int nH, int nC, int nFileLen, GSALGITEMIMGEX_DNN[] GsRst, int nNetIdx);


        //CPU
        [DllImport(@"QH_DNN_CV.dll")]
        public static extern int QH_InitDnnNetCV(byte[] cfgFile, byte[] weightsFile, int nW, int nH, int nNetID = -1);
        [DllImport(@"QH_DNN_CV.dll")]
        public static extern void QH_DNN_Detect_CV(byte[] pData, int nW, int nH, int nC, ref GSRESULTEX_DNN gs, int nNetID);


        public static byte[] GetFileByteData(string fileUrl)
        {
            using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
            {
                byte[] buffur = new byte[fs.Length];
                byte[] pReadByte = new byte[0];
                using (BinaryReader bw = new BinaryReader(fs))
                {
                    bw.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                    pReadByte = bw.ReadBytes((int)bw.BaseStream.Length);
                }
                return pReadByte;
            }
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GSRESULTEX_DNN
    {
        public int nCnt;//识别结果个数，最多MAX_GS_CNT
        public GSALGITEMIMGEX_DNN[] algImgItem;//具体识别结果
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct GSALGITEMIMGEX_DNN
    {
        public int nObjId;//识别目标的ID
        public float fConfidenc;//识别得分（1为满分）
        public RECT roi;//目标的坐标位置
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
