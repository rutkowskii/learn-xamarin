using System;
using System.IO;
using learn_xamarin.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]

public class FileHelper : IFileHelper
{
    public string GetLocalFilePath(string filename)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        return Path.Combine(path, filename);
    }
}