using System;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

namespace DarkJetpack
{
    class SaveGameStorage
    {
        public static void SaveHighScores(Vector2 _pos, int _skin)
        {
            // Get the path of the save game
            string fullpath = "Save"+_skin+".sav";
            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(Vector2));
                serializer.Serialize(stream, _pos);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }
        public static Vector2 LoadHighScores(int _skin)
        {
            Vector2 data;
            // Get the path of the save game
            string fullpath = "Save" + _skin + ".sav"; ;


            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(Vector2));
                data = (Vector2)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
            return (data);
        }
    }
}
