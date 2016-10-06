using System;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

namespace DarkJetpack {
    public struct data {
        public bool unlocked;
        public int highscore;

        public data(int highscore, bool unlocked1) : this() {
            this.highscore = highscore;
            this.unlocked = unlocked1;
        }
    }
    class SaveGameStorage {
        public static void SaveData(int highscore, int skin, bool unlocked) {
            string fullpath = "Save" + skin + ".sav";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(data));
                data d = new data(highscore, unlocked);
                serializer.Serialize(stream, d);
            } finally {
                stream.Close();
            }
        }
        public static data LoadData(int skin) {
            data ret;
            string fullpath = "Save" + skin + ".sav";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(data));
                ret = (data)serializer.Deserialize(stream);
            } finally {
                stream.Close();
            }
            return ret;
        }
    }
}
