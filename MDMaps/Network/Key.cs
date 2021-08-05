using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using MDMaps;

namespace MDMaps.Network
{
    public static class Key
    {
        public static string GetKey()
        {
            Assembly assem = typeof(MainPage).Assembly;
            using (Stream stream = assem.GetManifestResourceStream("MDMaps.Network.key.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
