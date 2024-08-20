using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace HelloWorldApplication
{
   class HelloWorld
   {
      /*  创建硬链接  */
      [DllImport("Kernel32", CharSet = CharSet.Unicode)]
      public extern static bool CreateHardLink(string linkName, string sourceName, IntPtr attribute);
      
      static void Main(string[] args)
      {
         //
         Console.WriteLine("请拖入待处理的文件（*.d2fp）:");
         string FileToRead = Console.ReadLine();
         Console.WriteLine("文件地址：{0}",FileToRead);
         
         //逐行读取，处理文件
         using (StreamReader ReaderObject = new StreamReader(FileToRead))
         {
            string str;
            int counti = 0;
            while((str = ReaderObject.ReadLine()) != null)
             {
                counti = counti + 1;
                Console.WriteLine("\r\n正在处理第【" + counti.ToString() + "】个，请稍后.......");
                
                // 按照逗号分割字符串
                string[] sArray = str.Split(new char[2] { '|', '>' }); 
                
                // 每个数组元素的值，第一个忽略，第二个保留，第三个及后面的删除后转为硬链接
                int i=0;
                string fileExists = "";
                foreach(string e in sArray)
                {
                   i = 1 + i;
                   
                   //第一个忽略，判断第二个元素指定的文件是否存在
                   if(i == 2 && File.Exists(e))
                   {
                      fileExists = e;
                   }
                   
                   //如果第三个元素指定的文件存在，则先删除文件，然后用第二个创建硬链接
                   if(i >= 3)
                   {
                      if (File.Exists(e)  && File.Exists(fileExists))
                      {
                         File.Delete(e);
                         CreateHardLink(e,fileExists, IntPtr.Zero);
                      }
                   }
                }
                //foreach结束
             }
             //while 结束
             Console.WriteLine("处理完成");
         }
         //using 结束
         Console.ReadKey();
      } 
   }
}