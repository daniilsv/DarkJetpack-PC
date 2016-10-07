using System.Xml.Serialization;
using System.IO;

namespace DarkJetpack {
    class SaveGameStorage {
        public static void SaveData(int highscore) {
            string fullpath = "Save.djp";
            FileStream stream = null;
            try {
                stream = File.Open(fullpath, FileMode.Create, FileAccess.Write);
                new XmlSerializer(typeof(int)).Serialize(stream, highscore);
            } catch (System.Exception e) {
                return;
            } finally {
                if (stream != null)
                    stream.Close();
            }
        }
        public static int LoadData() {
            int ret;
            string fullpath = "Save.djp";
            FileStream stream = null;
            try {
                stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
                ret = (int)new XmlSerializer(typeof(int)).Deserialize(stream);
            } catch (System.Exception e) {
                SaveData(0);
                ret = 0;
            } finally {
                if (stream != null)
                    stream.Close();
            }
            return ret;
        }
    }
}
