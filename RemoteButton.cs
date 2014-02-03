using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PGControlv0._1
{
    public class RemoteButton
    {
        
        
        public RemoteButton(String name, String content,UInt16 ascii,UInt16 topLeftRow, UInt16 topLeftColumn, UInt16 bottomRightRow,UInt16 bottomRightColumn)
        {
            this.ascii = ascii;
            this.content = content;
            this.name = name;
            this.topLeftColumn = topLeftColumn;
            this.topLeftRow = topLeftRow;
            this.bottomRightColumn = bottomRightColumn;
            this.bottomRightRow = bottomRightRow;
            this.clicked = false;
        }
        //public String color;
        public String name,content;
        public UInt16 ascii, topLeftRow, topLeftColumn, bottomRightRow, bottomRightColumn;
        public Boolean clicked;

    }
}
