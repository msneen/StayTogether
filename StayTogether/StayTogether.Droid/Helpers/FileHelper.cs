using Android.Graphics;
using Java.IO;

namespace StayTogether.Droid.Helpers
{
    public class FileHelper
    {

        public static string GetDocumentFileName()
        {
            var picDirectory = Android.OS.Environment.DirectoryDocuments;
            var fullDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(picDirectory);

            var directory = new File(fullDirectory.AbsolutePath);
            if (!directory.Exists())
            {
                directory.Mkdirs();
            }
            ;
            var myPath = new File(directory, "StayTogetherLog.txt");
            return myPath.Path;
        }
    }
}