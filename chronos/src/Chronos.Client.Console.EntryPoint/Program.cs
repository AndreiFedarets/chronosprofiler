using System;
using System.IO;
using Chronos.Daemon.Internal;
using System.Runtime.InteropServices;
using Rhiannon.Extensions;

namespace Chronos.Client.Console.EntryPoint
{
    class Program
    {
        //[DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        //private static extern ConvertedPage ConvertPage(SourcePage sourcePage);

        //xcopy /y $(TargetPath) $(TargetDir)\..\shell\win\extensions\
        //xcopy /y $(TargetPath) $(TargetDir)\..\host\extensions\
        private static void Main(string[] args)
        {
            //SourcePage sourcePage;
            //sourcePage.ThreadId = 1;
            //sourcePage.PageIndex = 2;
            //sourcePage.BeginLifetime = 3;
            //sourcePage.EndLifetime = 4;
            //sourcePage.Flag = 5;
            //sourcePage.Data = (IntPtr)6;
            //sourcePage.DataSize = 7;
            //ConvertedPage convertedPage = ConvertPage(sourcePage);


            //IConvertedPage[] pages = ReadFiles(processor);
            //Test1 
            //IConvertedPage test1Page = processor.MergePages(pages[0], pages[1]);
            //Test2
            //IConvertedPage test2Page1 = processor.MergePage(pages[0]);
            //IConvertedPage test2Page2 = processor.MergePage(pages[1]);
            //IConvertedPage test2Page = processor.MergePages(test2Page1, test2Page2);

            //bool equals = processor.ComparePages(test1Page, test2Page);
            //pages = Merge(pages, processor);

            //pages = Switch(pages);

            //pages = Merge(pages, processor);
            System.Console.ReadKey();

        }

        //internal static NativeArray[] Merge(NativeArray[] pages, Processor processor)
        //{
        //    int newSize = pages.Length/2+pages.Length%2;
        //    NativeArray[] newPages = new NativeArray[newSize];
        //    for (int i = 0; i < pages.Length; i += 2)
        //    {
        //        using (NativeArray data1 = pages[i])
        //        {
        //            using (NativeArray data2 = pages[i + 1])
        //            {
        //                NativeArray data = processor.MergePages(data1, data2);
        //                int newIndex = i / 2;
        //                newPages[newIndex] = data;
        //            }
        //        }
        //    }
        //    return newPages;
        //}

        //    internal static IConvertedPage[] ReadFiles(Processor processor)
        //    {
        //        uint callstackId = 0;
        //        DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\chronos_test\2\0");
        //        System.IO.FileInfo[] files = directoryInfo.GetFiles();
        //        IConvertedPage[] pages = new IConvertedPage[files.Length];
        //        for (int i = 0; i < files.Length; i++)
        //        {
        //            System.IO.FileInfo fileInfo = files[i];
        //            using (Stream source = fileInfo.OpenRead())
        //            {
        //                uint threadId = 0;
        //                uint pageIndex = (uint) i;
        //                PageState flag = ((i == files.Length - 1) ? PageState.Close : PageState.Continue);
        //                if (flag != PageState.Continue)
        //                {
        //                    callstackId++;
        //                }
        //                int dataSize = (int)source.Length;
        //                ISourcePage pageData = new SourcePage(threadId, callstackId, pageIndex, flag, 0, 0, dataSize);
        //                using (Stream target = pageData.OpenWrite())
        //                {
        //                    source.CopyTo(target);
        //                }
        //                using (pageData)
        //                {
        //                    pages[i] = processor.ConvertPage(pageData);
        //                }
        //            }
        //        }
        //        return pages;
        //    }

        //}

        //public interface IFileInfo
        //{
        //    DateTime CreationTime { get; }
        //}


        //public class FileInfo
        //{
        //    public DateTime CreationTime { get { return DateTime.MinValue; } }
        //}
    }
}
