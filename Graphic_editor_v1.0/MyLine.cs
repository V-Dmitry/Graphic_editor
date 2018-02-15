using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graphic_editor_v1._0
{
    [Serializable]
    class MyLine
    {
        bool is_group = false;
        int group_ind = 0;

        bool is_morf = false;
        int morf_ind = 0;

        double x1, x2, y1, y2;

        int color;

        double thickness = 0;

        public double oldx1, oldx2, oldy1, oldy2;

        public MyLine(bool is_group, bool is_morf, double x1, double x2, double y1, double y2, int color = 0, double thickness = 3, int group_ind = 0, int morf_ind = 0)
        {
            this.is_group = is_group;
            this.is_morf = is_morf;
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.color = color;
            this.thickness = thickness;
            this.group_ind = group_ind;
            this.morf_ind = morf_ind;
        }

        public Line GetLine()
        {
            Line line = new Line();
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;
            if (!Is_Group & !Is_Morf)
                line.Stroke = Brushes.Black;
            else if(Is_Morf)
                line.Stroke = Brushes.Pink;
            else if (Is_Group)
                line.Stroke = Brushes.Blue;
            line.StrokeThickness = thickness;
            return line;
        }

        public bool Is_Group
        {
            get { return is_group; }
        }

        public bool Is_Morf
        {
            get { return is_morf; }
        }

        public int Group_Ind
        {
            get { return group_ind; }
        }

        public int Morf_Ind
        {
            get { return morf_ind; }
        }
    }
}