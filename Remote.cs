using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace PGControlv0._1
{
    public class Remote
    {
        public List<RemoteButton> rbArray;
        public int id { get; set; }
        public Remote(UInt16 row,UInt16 column,String remoteName)
        {
            this.row = row;
            this.column = column;
            this.remoteName = remoteName;
            rbArray=new List<RemoteButton>();
        }
        public UInt16 row;
        public UInt16 column;
        //public String remoteName;
        public string remoteName { get; set; }
        public Int32[] buttonRefer;

        public void addButton(RemoteButton rb)
        {
            rbArray.Add(rb);    
        }

        public void setReference()
        {
            buttonRefer = new Int32[257];
            for (int i = 0; i < rbArray.Count; i++)
            {
                buttonRefer[int.Parse(rbArray[i].name)] = i;
            }
        }
    }
}
