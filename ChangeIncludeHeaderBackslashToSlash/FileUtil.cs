using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeIncludeHeaderBackslashToSlash
{
    internal class FileUtil
    {
        // ;로 구분된 것을 추출한다. 예: cpp;inl ==> [ cpp, inl ]
        internal static string[] CollectFileExts(string arg)
        {
            return arg.Split(';');
        }

        // 각 파일 확장자에 대한 파일 탐색
        internal static string[] CollectFiles(string path, string[] fileExts)
        {
            List<string> ret = new List<string>();

            foreach (string fileExt in fileExts)
            {
                ret.AddRange(Directory.GetFiles(path, $"*.{fileExt}", SearchOption.AllDirectories));
            }

            return ret.ToArray();
        }

        // include 구문을 변경한다. 가령 ..\를 ../로.
        internal static string GetChangedOrNotSourceLine(string line, ref bool changed)
        {
            string line2 = line.TrimStart(new char[] { ' ', '\t' });
            int trimLength = line.Length - line2.Length; // 몇글자가 짤려나갔는지

            if (!line2.StartsWith("#include"))
            {
                // #include 로 시작 안하니까 안바뀜
                return line;
            }

            int col1;
            col1 = line2.IndexOf('"');
            if (col1 < 0)
            {
                col1 = line2.IndexOf('<');
            }
            if (col1 < 0)
            {
                // 구문이 비정상 같아서 안바뀜
                return line;
            }
            int col2;
            col2 = line2.IndexOf('"', col1 + 1);
            if (col2 < 0)
            {
                col2 = line2.IndexOf('>', col1 + 1);
            }
            if (col2 < 0)
            {
                // 구문이 비정상 같아서 안바뀜
                return line;
            }

            string fileSpec = line2.Substring(col1, col2 - col1);  // e.g. "aa\bb.inl"
            string fileSpec2 = fileSpec.Replace('\\', '/'); // e.g. "aa/bb.inl"

            // 바뀐 글자가 없으면 스킵
            if (fileSpec2.Equals(fileSpec))
                return line;

            // 이제 바꾼다.
            string finalLine = line.Substring(0, trimLength) + line2.Substring(0, col1) + fileSpec2 + line2.Substring(col2);

            // 바뀌었다.
            changed = true;
            return finalLine;
        }
    }
}
