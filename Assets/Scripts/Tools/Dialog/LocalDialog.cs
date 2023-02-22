using System;
using System.Runtime.InteropServices;

namespace Tools
{
    internal class LocalDialog
    {
        //����ָ��ϵͳ����       ���ļ��Ի���
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        //����ָ��ϵͳ����        ���Ϊ�Ի���
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);


        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="ofn"></param>
        /// <returns></returns>
        public static bool GetFile([In, Out] OpenFileName ofn)
        {
            return GetOpenFileName(ofn);
        }

        /// <summary>
        /// ���Ϊ
        /// </summary>
        /// <param name="ofn"></param>
        /// <returns></returns>
        public static bool SaveFile([In, Out] OpenFileName ofn)
        {
            return GetSaveFileName(ofn);
        }


        /// <summary>
        ///  ��ȡ�ļ���·��
        /// </summary>
        public static IntPtr GetFolder([In, Out] OpenDialogDir ofn)
        {
            return SHBrowseForFolder(ofn);
        }
        /// <summary>
        /// ��ȡ�ļ���·��
        /// </summary>
        public static bool GetPathFormIDList([In] IntPtr pidl, [In, Out] char[] fileName)
        {
            return SHGetPathFromIDList(pidl, fileName);
        }
    }

}