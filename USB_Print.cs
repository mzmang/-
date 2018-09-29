using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public enum EnPrinterPort
    {

        PP_COM,
        PP_LPT,
        PP_USB,
        PP_NET
    };
    public enum EnPrinterInstruction
    {
        PI_SeletCutModeAndCutPaper,//1D 56 M N
        PI_SelectPrintMode,//1B 21 N
        PI_PrintDownLoadedBMP,//1D 2F M
        PI_GenerateDrawerPlse,//1B 70 M T1 T2
        PI_PrintSingleBeeper,//1B 42 N T
        PI_PrintSingleBeeperAndAlarmLightFlashes//1B 43 M T N
    };
    public enum EnStateFlag
    {
        SF_Failed,//����ʧ��
        SF_Success//�����ɹ�
    };
    public class printUSBHelper
    {

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr OpenUsb();

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseUsb(IntPtr hUsb);

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        // public static extern int WriteUsb(IntPtr hUsb, string sendBuffer, int bufferSize, ref int sendCount);
        public static extern int WriteUsb(IntPtr hUsb, byte[] sendBuffer, int bufferSize, ref int sendCount);

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr OpenComA(string comn, int baud);

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseCom(IntPtr hUsb);

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int WriteCom(IntPtr hUsb, string sendBuffer, int bufferSize, ref int sendCount);

        [DllImport("JsUsbDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern EnStateFlag SendInstruction(EnPrinterPort pPort,
            IntPtr handle,
            EnPrinterInstruction pInstr,
            int parameter1,
            int parameter2,
            int parameter3,
            int parameter4);
        /// <summary>
        /// ��ӡ����
        /// </summary>
        /// <param name="hUsb">�Զ��������IntPTR hUsb</param>
        /// <param name="sfbm">�շѱ���</param>
        /// <param name="bm">���α���</param>
        /// <param name="cpmc">��Ʒ����</param>
        /// <param name="yxq">��Ч��</param>
        /// <param name="cpxh">��Ʒ�ͺ�</param>
        /// <param name="zczh">ע��֤��</param>
        /// <param name="scph">��Ʒ����</param>
        /// <param name="sccj">��������</param>
        public static void printUSB(IntPtr usb, string sfbm, string bm, string cpmc, string yxq, string cpxh, string zczh, string scph, string sccj)
        {
            usb = OpenUsb();
            //hUsb = OpenComA("COM5",115200);

            if (null == usb && "-1" != usb.ToString())
            {
                MessageBox.Show("��ӡ������ʧ�ܣ������USB��ӡ����");
            }
            //string sendBuffer = "SIZE 60mm,30mm\r\nGAP 3,0\r\nOFFSET 3 mm\r\nSPEED 5\r\nDENSITY 7\r\nDIRECTION 1\r\nREFRENCE 0, 0\r\nCLS\r\nBARCODE 30,170,\"EAN128\",30,1,0,1,0,\"" + bm + "\"\r\nTEXT 30,10,\"TSS24.BF2\",0,1,1,\"���ƣ�" + cpmc + "\"\r\nTEXT 30,35,\"TSS24.BF2\",0,1,1,\"Ч�ڣ�" + yxq + "\"\r\nTEXT 30,60,\"TSS24.BF2\",0,1,1,\"�ͺţ�" + cpxh + "\"\r\nTEXT 30,85,\"TSS24.BF2\",0,1,1,\"ע��֤�ţ�" + zczh + "\"\r\nTEXT 30,110,\"TSS24.BF2\",0,1,1,\"���ţ�" + scph + "\"\r\nTEXT 30,135,\"TSS24.BF2\",0,1,1,\"���ң�" + sccj + "\"\r\nPRINT 1,1\r\nCLS";
            string sendBuffer = "SIZE 60mm,40mm\r\nGAP 3,0\r\nOFFSET 3 mm\r\nSPEED 1\r\nDENSITY 0\r\nDIRECTION 1\r\nREFRENCE 0, 0\r\nCLS\r\nBARCODE 30,240,\"EAN128\",30,1,0,1,0,\"" + sfbm + "\"\r\nBARCODE 30,170,\"EAN128\",30,1,0,1,0,\"" + bm + "\"\r\nTEXT 30,10,\"TSS24.BF2\",0,1,1,\"���ƣ�" + cpmc + "\"\r\nTEXT 30,35,\"TSS24.BF2\",0,1,1,\"Ч�ڣ�" + yxq + "\"\r\nTEXT 30,60,\"TSS24.BF2\",0,1,1,\"�ͺţ�" + cpxh + "\"\r\nTEXT 30,85,\"TSS24.BF2\",0,1,1,\"ע��֤�ţ�" + zczh + "\"\r\nTEXT 30,110,\"TSS24.BF2\",0,1,1,\"���ţ�" + scph + "\"\r\nTEXT 30,135,\"TSS24.BF2\",0,1,1,\"���ң�" + sccj + "\"\r\nPRINT 1,1\r\nCLS";
            if (null != usb && "-1" != usb.ToString())
            {
                int sendSize = 0;
                List<byte> sendBytes = new List<byte>();
                foreach (char chr in sendBuffer)
                {
                    char[] tempChar = { chr };
                    byte[] tempBytes = Encoding.GetEncoding("GBK").GetBytes((tempChar));
                    foreach (byte tempByte in tempBytes)
                    {
                        sendBytes.Add(tempByte);
                    }
                }
                //string sendUnicode = Encoding.Unicode.GetString(Encoding.Default.GetBytes(sendBuffer));
                //WriteUsb(usb, sendUnicode, Encoding.Unicode.GetByteCount(sendUnicode), ref sendSize);
                WriteUsb(usb, sendBytes.ToArray(), sendBytes.Count, ref sendSize);
                SendInstruction(EnPrinterPort.PP_USB,
                usb,
                EnPrinterInstruction.PI_SeletCutModeAndCutPaper,
                0, 0, 0, 0
                );
                return;
            }
            else
            {
                MessageBox.Show("��USBʧ�ܣ������USB��ӡ����");
            }
            CloseUsb(usb);
        }

        public static void printUSB(IntPtr usb, string bm)
        {
            usb = OpenUsb();
            //hUsb = OpenComA("COM5",115200);

            if (null == usb && "-1" != usb.ToString())
            {
                MessageBox.Show("��ӡ������ʧ�ܣ������USB��ӡ����");
            }
            string sendBuffer = "SIZE 60mm,30mm\r\nGAP 3,0\r\nOFFSET 3 mm\r\nSPEED 5\r\nDENSITY 7\r\nDIRECTION 1\r\nREFRENCE 0, 0\r\nCLS\r\nBARCODE 30,170,\"EAN128\",30,1,0,2,0,\"" + bm + "\"\r\nPRINT 1,1\r\nCLS";
            if (null != usb && "-1" != usb.ToString())
            {
                int sendSize = 0;
                List<byte> sendBytes = new List<byte>();
                foreach (char chr in sendBuffer)
                {
                    char[] tempChar = { chr };
                    byte[] tempBytes = Encoding.GetEncoding("GBK").GetBytes((tempChar));
                    foreach (byte tempByte in tempBytes)
                    {
                        sendBytes.Add(tempByte);
                    }
                }
                //string sendUnicode = Encoding.Unicode.GetString(Encoding.Default.GetBytes(sendBuffer));
                //WriteUsb(usb, sendUnicode, Encoding.Unicode.GetByteCount(sendUnicode), ref sendSize);
                WriteUsb(usb, sendBytes.ToArray(), sendBytes.Count, ref sendSize);
                SendInstruction(EnPrinterPort.PP_USB,
                usb,
                EnPrinterInstruction.PI_SeletCutModeAndCutPaper,
                0, 0, 0, 0
                );
                return;
            }
            else
            {
                MessageBox.Show("��USBʧ�ܣ������USB��ӡ����");
            }
            CloseUsb(usb);
        }
    }
}
