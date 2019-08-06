#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.File
* 项目描述 ：
* 类 名 称 ：FileMethod
* 类 描 述 ：
* 命名空间 ：Xray.Tools.File
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/5/15 16:42:10
* 更新时间 ：2019/5/15 16:42:10
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Xray.Tools.File
{
    public class FileMethod
    {
        /// <summary>
        /// 获得目录下所有文件或指定文件类型文件(包含所有子文件夹)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="searchPattern">通配符</param>
        /// <returns>List<FileInfo></returns>
        [Obsolete("该方法已被弃用，请使用Directory.GetFiles代替")]
        public static List<String> GetFiles(string searchdir, string searchPattern = "*.*")
        {
            try
            {
                List<String> lst = new List<String>();
                foreach(var d in Directory.GetDirectories(searchdir))
                {
                    lst.AddRange(GetFiles(d,searchPattern));
                }
                lst.AddRange(Directory.GetFiles(searchdir, searchPattern));

                return lst.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 复制文件夹及文件夹下的文件
        /// </summary>
        /// <param name="searchdir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static void CopyDIr(string sourcedir, string targetdir)
        {
            try
            {
                if(!Directory.Exists(targetdir))
                {
                    Directory.CreateDirectory(targetdir);
                }
                var files = Directory.GetFiles(sourcedir);
                if(files?.Length >0 )
                {
                    foreach(var file in files)
                    {
                        System.IO.File.Copy(file,file.Replace(sourcedir,targetdir));
                    }
                }
                foreach (var d in Directory.GetDirectories(sourcedir))
                {
                    CopyDIr(d, d.Replace(sourcedir, targetdir));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }
    }
}