using System;
class MainClass
{
    public static void Main(string[] args)
    {
        string DirName = @"D:\new";
        Console.WriteLine($"Исходный размер папки {DirName} - {ShowFolderSize(DirName)} байт.");
        DirCleaner(DirName);
        Console.WriteLine($"Текущий размер папки {DirName} - {ShowFolderSize(DirName)} байт.");
    }
    static long ShowFolderSize(string dirName)
    {
        long folderSize = 0;
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles();
                foreach (var item in files)
                {
                    folderSize += item.Length;
                }
                DirectoryInfo[] subfolders = dirInfo.GetDirectories();
                foreach (var item in subfolders)
                {
                    folderSize += ShowFolderSize(item.FullName);
                }
            }
            else
            {
                Console.WriteLine("Указан неверный путь к директории");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return folderSize;
    }
    static void DirCleaner(string dirName)
    {
        long deletedSize = 0;
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            if (dirInfo.Exists)
            {
                FileInfo[] fileNames = dirInfo.GetFiles();
                foreach (var item in fileNames)
                {
                    if (item.LastAccessTime.CompareTo(DateTime.Now - TimeSpan.FromMinutes(30)) < 0)
                    {
                        deletedSize += item.Length;
                        item.Delete();
                    }
                }
                DirectoryInfo[] folderNames = dirInfo.GetDirectories();
                foreach (var item in folderNames)
                {
                    DirCleaner(item.FullName);
                    if (item.LastAccessTime.CompareTo(DateTime.Now - TimeSpan.FromMinutes(30)) < 0)
                    {
                        deletedSize += ShowFolderSize(item.FullName);
                        item.Delete(true);
                    }
                }
                Console.WriteLine($"Очистка папки {dirInfo.FullName} от файлов и папок, не использующихся более 30 минут завершена\nОсвобождено - {deletedSize} байт");
            }
            else
            {
                Console.WriteLine($"По указанному адресу директория отсутствует");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}