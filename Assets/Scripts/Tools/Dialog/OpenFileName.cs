using System;
using System.Runtime.InteropServices;

namespace Tools
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class OpenFileName
    {
        public int structSize = 0;       //结构的内存大小
        public IntPtr dlgOwner = IntPtr.Zero;       //设置对话框的句柄
        public IntPtr instance = IntPtr.Zero;       //根据flags标志的设置，确定instance是谁的句柄，不设置则忽略
        public string filter = null;         //调取文件的过滤方式
        public string customFilter = null;  //一个静态缓冲区 用来保存用户选择的筛选器模式
        public int maxCustFilter = 0;     //缓冲区的大小
        public int filterIndex = 0;                 //指向的缓冲区包含定义过滤器的字符串对
        public string file = null;                  //存储调取文件路径
        public int maxFile = 0;                     //存储调取文件路径的最大长度 至少256
        public string fileTitle = null;             //调取的文件名带拓展名
        public int maxFileTitle = 0;                //调取文件名最大长度
        public string initialDir = null;            //最初目录
        public string title = null;                 //打开窗口的名字
        public int flags = 0;                       //初始化对话框的一组位标志  参数类型和作用查阅官方API
        public short fileOffset = 0;                //文件名前的长度
        public short fileExtension = 0;             //拓展名前的长度
        public string defExt = null;                //默认的拓展名
        public IntPtr custData = IntPtr.Zero;       //传递给lpfnHook成员标识的钩子子程的应用程序定义的数据
        public IntPtr hook = IntPtr.Zero;           //指向钩子的指针。除非Flags成员包含OFN_ENABLEHOOK标志，否则该成员将被忽略。
        public string templateName = null;          //模块中由hInstance成员标识的对话框模板资源的名称
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;                     //可用于初始化对话框的一组位标志


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
