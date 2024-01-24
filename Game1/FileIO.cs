using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class FileIO
    {
        public static void Save<T>(T data, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, data);
                //fileStream.Close();
            }
        }
        public static T Load<T>(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(fileStream);
                //fileStream.Close();
            }
        }
    }
}
