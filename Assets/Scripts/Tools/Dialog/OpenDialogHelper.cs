using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// 文件/文件夹操作
    /// </summary>
    public class OpenDialogHelper
    {
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="callBack">获得文件回调</param>
        /// <param name="fileter">文件类型</param>
        public static void SelectFile(Action<string> callBack, string fileter)
        {
            try
            {
                OpenFileName openFileName = new OpenFileName();
                openFileName.structSize = Marshal.SizeOf(openFileName);
                openFileName.filter = fileter;
                openFileName.file = new string(new char[1024]);
                openFileName.maxFile = openFileName.file.Length;
                openFileName.fileTitle = new string(new char[64]);
                openFileName.maxFileTitle = openFileName.fileTitle.Length;
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
                openFileName.title = "选择文件";
                //openFileName.defExt = "FBX";
                //openFileName.flags = 0x00001000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
                openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

                if (LocalDialog.GetFile(openFileName))
                {
                    string filePath = openFileName.file;
                    if (File.Exists(filePath))
                    {
                        callBack?.Invoke(filePath);
                        return;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="title"></param>
        public static void SelectFolder(Action<string> callBack, string title = "请选择文件夹")
        {
            try
            {
                OpenDialogDir ofn2 = new OpenDialogDir();
                ofn2.pszDisplayName = new string(new char[2048]);
                ofn2.lpszTitle = title; // 标题  
                ofn2.ulFlags = 0x00000040; // 新的样式,带编辑框  
                IntPtr pidlPtr = LocalDialog.GetFolder(ofn2);

                char[] charArray = new char[2048];

                for (int i = 0; i < 2048; i++)
                {
                    charArray[i] = '\0';
                }
                LocalDialog.GetPathFormIDList(pidlPtr, charArray);
                string res = new string(charArray);
                res = res.Substring(0, res.IndexOf('\0'));
                if (Directory.Exists(res))
                {
                    callBack?.Invoke(res);
                }

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 文件另存为
        /// </summary>
        public static void SaveFile(Action<string> callBack, string fileter)
        {
            try
            {
                OpenFileName openFileName = new OpenFileName();
                openFileName.structSize = Marshal.SizeOf(openFileName);
                openFileName.filter = fileter;
                openFileName.file = new string(new char[1024]);
                openFileName.maxFile = openFileName.file.Length;
                openFileName.fileTitle = new string(new char[64]);
                openFileName.maxFileTitle = openFileName.fileTitle.Length;
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
                openFileName.title = "另存为";
                //openFileName.defExt = "FBX";
                //openFileName.flags = 0x00001000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
                openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

                if (LocalDialog.SaveFile(openFileName))
                {
                    string filePath = openFileName.file;
                    if (File.Exists(filePath))
                    {
                        callBack?.Invoke(filePath);
                        return;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 打开目录
        /// </summary>
        /// <param name="path">将要打开的文件目录</param>
        public static void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        /// <summary>
        /// 删除文件夹下面所有文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFell(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemInfo[] fileSystems = dir.GetFileSystemInfos();
            foreach (var item in fileSystems)
            {
                if (item is DirectoryInfo)//判断是否是文件夹
                {
                    DirectoryInfo directory = new DirectoryInfo(item.FullName);
                    directory.Delete(true);
                }
                else
                {
                    File.Delete(item.FullName);//删除这个文件
                }
            }
        }

    }

}