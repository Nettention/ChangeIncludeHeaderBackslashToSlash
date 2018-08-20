using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeIncludeHeaderBackslashToSlash
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Changes #include \"..\\aa.h\" to #include \"../aa.h\".");
                Console.WriteLine("Args example: ChangeIncludeHeaderBackslashToSlash.exe work\\MySourceFolder cpp;h;inl");
                return;
            }

            try
            {
                string[] fileExts = FileUtil.CollectFileExts(args[1]);

                // 지정된 폴더의 모든 cpp, h file을 찾는다.
                string[] sourceFiles = FileUtil.CollectFiles(args[0], fileExts);

                List<string> changedFiles = new List<string>();
                List<string> unchangedFiles = new List<string>();
                List<string> erroredFiles = new List<string>();

                // 파일 안에서 #include ...를 찾는다.
                foreach (string sourceFileName in sourceFiles)
                {
                    try // 개별 파일 실패는 그냥 다음 파일 처리로 넘어가야...
                    {
                        string[] sourceLines = File.ReadAllLines(sourceFileName);

                        bool changed = false;
                        for (int i = 0; i < sourceLines.Length; i++)
                        {
                            sourceLines[i] = FileUtil.GetChangedOrNotSourceLine(sourceLines[i], ref changed);
                        }

                        if (changed)
                        {
                            File.WriteAllLines(sourceFileName, sourceLines);
                            changedFiles.Add(sourceFileName);
                        }
                        else
                        {
                            unchangedFiles.Add(sourceFileName);
                        }
                    }
                    catch (Exception)
                    {
                        // 바꾼 파일을 저장한다. 실패하는 것들은 모아놨다가 한꺼번에 출력하자.
                        erroredFiles.Add(sourceFileName);
                    }
                }

                // 작업 끝. 이제 결과를 출력하자.
                Console.WriteLine($"{unchangedFiles.Count} files are skipped. {changedFiles.Count} files are updated. {erroredFiles.Count} files are in error.");
                foreach (string e in erroredFiles)
                {
                    Console.WriteLine($"{e} is in error.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error! Work halted.");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
