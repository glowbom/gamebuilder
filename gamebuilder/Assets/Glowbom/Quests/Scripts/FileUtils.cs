using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/*
 * Created on Sun Jul 21 2019
 *
 * Copyright (c) 2019 Glowbom, Inc.
 */
namespace Assets.Scripts.Utils
{
    class FileUtils
    {
        public static void SafeWriteToFile(string content, string path)
        {
            try
            {
                using (
                    StreamWriter sw =
                        new StreamWriter(string.Concat(Application.persistentDataPath, "/", path), false))
                {
                    sw.Write(content);
                }
            }
            catch (IOException e)
            {
                Debug.Log(e.StackTrace);
            }
        }

        public static T LoadAndDeserialize<T>(string path)
        {
            var save = default(T);
            try
            {
                var fullPath = string.Concat(Application.persistentDataPath, "/", path);
         
                if (File.Exists(fullPath))
                {
                    using (var sr = new StreamReader(fullPath))
                    {
                        string data = sr.ReadToEnd();
                        save = JsonUtility.FromJson<T>(data);
                    }
                }
                return save;
            }
            catch (IOException e)
            {
                Debug.LogError(e.StackTrace);
            }
            return default(T);
        }
    }


}
