using System.Xml.Serialization;
using System.IO;

namespace DarkJetpack {
    class SaveGameStorage {
        public static void SaveData(int highscore) {
            string fullpath = "Save.djp";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(int));
                serializer.Serialize(stream, highscore);
            } finally {
                stream.Close();
            }
        }
        public static int LoadData() {
            int ret;
            string fullpath = "Save.djp";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(int));
                ret = (int)serializer.Deserialize(stream);
            } finally {
                stream.Close();
            }
            return ret;
        }
    }
}
