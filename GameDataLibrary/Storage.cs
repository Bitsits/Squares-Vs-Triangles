/*
 * Copyright (c) 2011 BitSits Games
 *  
 * Shubhajit Saha    http://bitsits.blogspot.com/
 * Maya Agarwal      http://bitsitsgames.blogspot.com/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace GameDataLibrary
{
    [Serializable]
    public struct SaveData
    {
        public string WaringMessage;
        public List<int> LevelData;
    }

    public class Storage
    {
        string fileName;
        int MaxLevelIndex;

        public SaveData saveData = new SaveData();

        public Storage(string fileName, int MaxLevelIndex)
        {
            this.fileName = fileName;
            this.MaxLevelIndex = MaxLevelIndex;

            Load();
        }

        void CreatNew()
        {
            saveData.WaringMessage = "\n\n\t\tDo not Change any Content\t\t\n\n";
            saveData.LevelData = new List<int>();

            for (int i = 0; i < MaxLevelIndex; i++) saveData.LevelData.Add(0);
        }

        void Load()
        {
            SaveData saveData = new SaveData();

            if (File.Exists(fileName))
            {
                FileStream saveFile = File.Open(fileName, FileMode.Open);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveData));

                saveData = (SaveData)xmlSerializer.Deserialize(saveFile);
                saveFile.Close();
            }

            // Verify
            bool isCorrect = true;
            if (saveData.LevelData == null || saveData.LevelData.Count != MaxLevelIndex)
                isCorrect = false;

            if (!isCorrect) CreatNew();
            else this.saveData = saveData;
        }

        public void Save()
        {
            FileStream saveFile = File.Open(fileName, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveData));

            xmlSerializer.Serialize(saveFile, saveData);
            saveFile.Close();
        }
    }
}
