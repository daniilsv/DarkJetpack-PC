using System;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

namespace DarkJetpack {
    class SaveGameStorage {
        public static void SaveHighScores(int highscore, int skin) {
            string fullpath = "Save" + skin + ".sav";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(int));
                serializer.Serialize(stream, highscore);
            } finally {
                stream.Close();
            }
        }
        public static int LoadHighScores(int skin) {
            int ret;
            string fullpath = "Save" + skin + ".sav";
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
