

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace WareHouseWPF.Converters
{
    internal class PathToByteArray
    {
        public static byte[] ToByteArr(string imagePath)
        {
            byte[] imageByteArray = null;
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                imageByteArray = new byte[reader.BaseStream.Length];
                for (int i = 0; i < reader.BaseStream.Length; i++)
                {
                    imageByteArray[i] = reader.ReadByte();
                }
            }
            return imageByteArray;
        }
    }
}
