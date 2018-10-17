using System;
using System.Collections.Generic;
using System.IO;

namespace MyExtensions
{
    public static class Extensions
    {
        public static bool Contains(this string str, string[] terms)
        {
            foreach (string term in terms)
            {
                if (str.ToLower().Contains(term))
                    return true;
            }
            return false;
        }
        public static bool IsSkin(this string str)
        {
            if (!Directory.Exists(str))
                return false;

            string[] files = Directory.GetFiles(str);

            if (files == null && files.Length == 0)
                return false; 

            foreach (string s in files)
            {
                if (Path.GetFileName(s).ToLower() == "skin.ini")
                    return true;
            }

            return false;
        }

        // necessary? check references
        public static bool IsSound(this string file)
        {
            return file.Contains(".wav") || file.Contains(".ogg") || file.Contains("mp3");
        }
        public static bool IsImage(this string file)
        {
            return file.Contains(".png") || file.Contains(".jpeg");
        }

        public static string[] GetImageFiles(this string[] files)
        {
            List<string> imageFiles = new List<string>();
            foreach (string file in files)
            {
                if (file.IsImage())
                {
                    imageFiles.Add(file);
                }
            }
            return imageFiles.ToArray();
        }
        public static string[] GetAudioFiles(this string[] files)
        {
            List<string> audioFiles = new List<string>();
            foreach (string file in files)
            {
                if (file.IsSound())
                {
                    audioFiles.Add(file);
                }
            }
            return audioFiles.ToArray();
        }

        public static T Add<T>(this List<T> list, T element)
        {
            list.Add(element);
            return element;
        }

        public static List<T> ToList<T>(this T[] arr)
        {
            List<T> list = new List<T>();
            foreach (T t in arr)
            {
                list.Add(t);
            }
            return list;
        }

        // https://stackoverflow.com/questions/1547252/how-do-i-concatenate-two-arrays-in-c
        // necessary?
        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }
    }
}
