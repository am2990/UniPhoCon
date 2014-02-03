using System;
using System.Net;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace PGControlv0._1
{
    static class RemoteReader
    {
        public static Remote LoadRemote(int id)
        {
            string rawData = readRawData();
            string[] sep = new string[] { "\r\n" }; //Splittng it with new line
            string[] arrData = rawData.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            System.Diagnostics.Debug.WriteLine("read=" + rawData);
            foreach (var d in arrData)
            {
                Remote temp = JsonConvert.DeserializeObject<Remote>(d);
                if (temp.id == id)
                {
                    return temp;
                }
                System.Diagnostics.Debug.WriteLine("rem id=" + temp.id);
            }
            return null;
        }

        public static List<Remote> LoadData()
        {
            string rawData = readRawData();
            string[] sep = new string[] { "\r\n" }; //Splittng it with new line
            string[] arrData = rawData.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            System.Diagnostics.Debug.WriteLine("read="+rawData);
            List<Remote> remoteList = new List<Remote>();
            foreach (var d in arrData)
            {
                System.Diagnostics.Debug.WriteLine("remote there" + d);
                Remote temp = JsonConvert.DeserializeObject<Remote>(d);
                remoteList.Add(temp);
                System.Diagnostics.Debug.WriteLine("rem name=" + temp.remoteName
                    );
            }
            //Binding data to the UI for display
            return remoteList;
        }

        private static string readRawData()
        {
            IsolatedStorageFile myFile = IsolatedStorageFile.GetUserStoreForApplication();
            string sFile = "Remotes.txt";
            //myFile.DeleteFile(sFile);
            if (!myFile.FileExists(sFile))
            {
                IsolatedStorageFileStream dataFile = myFile.CreateFile(sFile);
                dataFile.Close();
            }

            //Reading and loading data
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream(sFile, FileMode.Open, myFile));
            string rawData = reader.ReadToEnd();
            reader.Close();
            return rawData;
        }
    }
}
