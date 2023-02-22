using System;
using System.Runtime.InteropServices;

namespace Tools
{
    internal class LocalDialog
    {
        //链接指定系统函数       打开文件对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        //链接指定系统函数        另存为对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);


        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ofn"></param>
        /// <returns></returns>
        public static bool GetFile([In, Out] OpenFileName ofn)
        {
            return GetOpenFileName(ofn);
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="ofn"></param>
        /// <returns></returns>
        public static bool SaveFile([In, Out] OpenFileName ofn)
        {
            return GetSaveFileName(ofn);
        }


        /// <summary>
        ///  获取文件夹路径
        /// </summary>
        public static IntPtr GetFolder([In, Out] OpenDialogDir ofn)
        {
            return SHBrowseForFolder(ofn);
        }
        /// <summary>
        /// 获取文件夹路径
        /// </summary>
        public static bool GetPathFormIDList([In] IntPtr pidl, [In, Out] char[] fileName)
        {
            return SHGetPathFromIDList(pidl, fileName);
        }
    }

}