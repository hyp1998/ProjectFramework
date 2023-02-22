using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// �ļ�/�ļ��в���
    /// </summary>
    public class OpenDialogHelper
    {
        /// <summary>
        /// ���ļ�
        /// </summary>
        /// <param name="callBack">����ļ��ص�</param>
        /// <param name="fileter">�ļ�����</param>
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
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//Ĭ��·��
                openFileName.title = "ѡ���ļ�";
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
        /// ѡ���ļ���
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="title"></param>
        public static void SelectFolder(Action<string> callBack, string title = "��ѡ���ļ���")
        {
            try
            {
                OpenDialogDir ofn2 = new OpenDialogDir();
                ofn2.pszDisplayName = new string(new char[2048]);
                ofn2.lpszTitle = title; // ����  
                ofn2.ulFlags = 0x00000040; // �µ���ʽ,���༭��  
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
        /// �ļ����Ϊ
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
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//Ĭ��·��
                openFileName.title = "���Ϊ";
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
        /// ��Ŀ¼
        /// </summary>
        /// <param name="path">��Ҫ�򿪵��ļ�Ŀ¼</param>
        public static void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        /// <summary>
        /// ɾ���ļ������������ļ�
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFell(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemInfo[] fileSystems = dir.GetFileSystemInfos();
            foreach (var item in fileSystems)
            {
                if (item is DirectoryInfo)//�ж��Ƿ����ļ���
                {
                    DirectoryInfo directory = new DirectoryInfo(item.FullName);
                    directory.Delete(true);
                }
                else
                {
                    File.Delete(item.FullName);//ɾ������ļ�
                }
            }
        }

    }

}