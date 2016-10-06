using System.Xml.Serialization;
using System.IO;

namespace DarkJetpack {
    public struct SavedData {
        public bool unlocked;
        public int highscore;
        public SavedData(int highscore, bool unlocked) {
            this.highscore = highscore;
            this.unlocked = unlocked;
        }
    }
    class SaveGameStorage {
        public static void SaveData(int highscore, int skin, bool unlocked) {
            string fullpath = "Save" + skin + ".sav";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(SavedData));
                SavedData d = new SavedData(highscore, unlocked);
                serializer.Serialize(stream, d);
            } finally {
                stream.Close();
            }
        }
        public static SavedData LoadData(int skin) {
            SavedData ret;
            string fullpath = "Save" + skin + ".sav";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(SavedData));
                ret = (SavedData)serializer.Deserialize(stream);
            } finally {
                stream.Close();
            }
            return ret;
        }
    }
}
