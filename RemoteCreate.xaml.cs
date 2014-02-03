using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PGControlv0._1
{
    public partial class RemoteCreate : PhoneApplicationPage
    {
        Remote remote;
        int buttons = 0;
        RemoteButton crb;//current remote button
       
        
            
        

        public RemoteCreate(IDictionary<string, string>  parameters)
        {
            InitializeComponent();    
            remote = new Remote(48, 24, "testRemote");
            remote.setReference();
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
            
        }


        

        private void CreateControls(Remote remote)
        {
            System.Diagnostics.Debug.WriteLine("Remote Creation!");

            Grid rootGrid = new Grid();
            rootGrid.ShowGridLines=true;
            rootGrid.Name = "rootGrid";
            double colWidth = System.Windows.Application.Current.Host.Content.ActualWidth / remote.column;
            double rowHeight = System.Windows.Application.Current.Host.Content.ActualHeight / remote.row;
            for (int i = 0; i < remote.column; i++)
            {
                rootGrid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = new GridLength(colWidth) });
            }



            for (int i = 0; i < remote.row; i++)
            {
                rootGrid.RowDefinitions.Add(
                new RowDefinition() { Height = new GridLength(rowHeight) });
            }



            foreach (RemoteButton rb in remote.rbArray)
            {
                System.Windows.Shapes.Rectangle button = new System.Windows.Shapes.Rectangle();
                button.Name = rb.name;
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.VerticalAlignment = VerticalAlignment.Top;
                button.Margin = new Thickness(0);
                Grid.SetColumnSpan(button, (int)((rb.bottomRightColumn - rb.topLeftColumn) * colWidth));
                Grid.SetRowSpan(button, (int)((rb.bottomRightRow - rb.topLeftRow) * rowHeight));
                button.Width = (rb.bottomRightColumn - rb.topLeftColumn) * colWidth;
                button.Height = (rb.bottomRightRow - rb.topLeftRow) * rowHeight;
                button.Fill = new SolidColorBrush(Colors.Green);
                button.UpdateLayout();
                Grid.SetRow(button, rb.topLeftRow);
                Grid.SetColumn(button, rb.topLeftColumn);
                rootGrid.Children.Add(button);
            }



            LayoutRoot.Children.Add(rootGrid);
        }


        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Touched");
            Grid rootGrid = (Grid)FindName("rootGrid");
           
            //int pointsNumber = e.GetTouchPoints(rootGrid).Count;
            TouchPoint point = e.GetPrimaryTouchPoint(rootGrid);
            if (point != null && point.Action == TouchAction.Down)
            {
                e.SuspendMousePromotionUntilTouchUp();
            }

            if ((point != null) && point.Action == TouchAction.Up)
            {
                Rectangle rectangle = (Rectangle)FindName("current");
                double colWidth = System.Windows.Application.Current.Host.Content.ActualWidth / remote.column;
                double rowHeight = System.Windows.Application.Current.Host.Content.ActualHeight / remote.row;
                int r = (int)(point.Position.Y / rowHeight);
                int c = (int)(point.Position.X / colWidth);
                System.Diagnostics.Debug.WriteLine("Row = " + r +":::Col = " +c);
                if (rectangle != null)
                {
                    crb.bottomRightRow = (ushort)r;
                    crb.bottomRightColumn = (ushort)c;
                    rectangle.Name = crb.name;
                    rootGrid.Children.Remove(rectangle);
                    System.Diagnostics.Debug.WriteLine("Rectangle Row = " + crb.bottomRightRow + ":::Col = " + crb.bottomRightColumn);
                    Cell_Click(crb.topLeftRow, crb.topLeftColumn, r-crb.topLeftRow+1, c-crb.topLeftColumn+1, crb.name);
                    return;
                }    
                Cell_Click(r, c, 1 , 1, "current");
            }
            
        }


        private void Cell_Click(int row, int column, int rs, int cs, string name)
        {
            
            Grid rootGrid = (Grid)FindName("rootGrid");
            if (rs < 1 || cs < 1)
            {
                return;
            }
            //rootGrid.Background = new SolidColorBrush(Colors.Red);
            System.Diagnostics.Debug.WriteLine("Tried to change color");
            double colWidth = System.Windows.Application.Current.Host.Content.ActualWidth / remote.column;
            double rowHeight = System.Windows.Application.Current.Host.Content.ActualHeight / remote.row;
            System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
            rectangle.Name = name;
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Top;
            rectangle.Margin = new Thickness(0);
            Grid.SetColumnSpan(rectangle, cs);
            Grid.SetRowSpan(rectangle, rs);
            rectangle.Width = (cs)*colWidth;
            rectangle.Height =(rs)*rowHeight;
            rectangle.Fill = new SolidColorBrush(Colors.Green);
            rectangle.UpdateLayout();
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            crb = new RemoteButton(buttons.ToString(), "sample", 0, (ushort)row, (ushort)column, (ushort)row, (ushort)column);
            if (!name.Equals("current"))
            {
                buttons += 1;
                rectangle.Opacity = 0;
                rootGrid.Children.Add(rectangle);
                CreateFadeInOutAnimation(0,100,rectangle).Begin();
            }
            else
            {
                rootGrid.Children.Add(rectangle);
            }
            
        }

        private Storyboard CreateFadeInOutAnimation(double from, double to, Rectangle rect)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = from;
            fadeInAnimation.To = to;
            fadeInAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            Storyboard.SetTarget(fadeInAnimation, rect);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));

            sb.Children.Add(fadeInAnimation);

            return sb;
        }

    }

}