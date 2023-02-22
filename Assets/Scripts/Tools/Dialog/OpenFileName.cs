using System;
using System.Runtime.InteropServices;

namespace Tools
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class OpenFileName
    {
        public int structSize = 0;       //�ṹ���ڴ��С
        public IntPtr dlgOwner = IntPtr.Zero;       //���öԻ���ľ��
        public IntPtr instance = IntPtr.Zero;       //����flags��־�����ã�ȷ��instance��˭�ľ���������������
        public string filter = null;         //��ȡ�ļ��Ĺ��˷�ʽ
        public string customFilter = null;  //һ����̬������ ���������û�ѡ���ɸѡ��ģʽ
        public int maxCustFilter = 0;     //�������Ĵ�С
        public int filterIndex = 0;                 //ָ��Ļ���������������������ַ�����
        public string file = null;                  //�洢��ȡ�ļ�·��
        public int maxFile = 0;                     //�洢��ȡ�ļ�·������󳤶� ����256
        public string fileTitle = null;             //��ȡ���ļ�������չ��
        public int maxFileTitle = 0;                //��ȡ�ļ�����󳤶�
        public string initialDir = null;            //���Ŀ¼
        public string title = null;                 //�򿪴��ڵ�����
        public int flags = 0;                       //��ʼ���Ի����һ��λ��־  �������ͺ����ò��Ĺٷ�API
        public short fileOffset = 0;                //�ļ���ǰ�ĳ���
        public short fileExtension = 0;             //��չ��ǰ�ĳ���
        public string defExt = null;                //Ĭ�ϵ���չ��
        public IntPtr custData = IntPtr.Zero;       //���ݸ�lpfnHook��Ա��ʶ�Ĺ����ӳ̵�Ӧ�ó����������
        public IntPtr hook = IntPtr.Zero;           //ָ���ӵ�ָ�롣����Flags��Ա����OFN_ENABLEHOOK��־������ó�Ա�������ԡ�
        public string templateName = null;          //ģ������hInstance��Ա��ʶ�ĶԻ���ģ����Դ������
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;                     //�����ڳ�ʼ���Ի����һ��λ��־


    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class OpenDialogDir
    {
        public IntPtr hwndOwner = IntPtr.Zero;
        public IntPtr pidlRoot = IntPtr.Zero;
        public String pszDisplayName = null;
        public String lpszTitle = null;
        public UInt32 ulFlags = 0;
        public IntPtr lpfn = IntPtr.Zero;
        public IntPtr lParam = IntPtr.Zero;
        public int iImage = 0;
    }

}
