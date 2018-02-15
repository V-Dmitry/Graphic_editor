using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization.Formatters.Binary;

namespace Graphic_editor_v1._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        Random r;

        int count = 0;
        int group_count = 0;
        int morf_count = 0;
        int tmp1 = 0;
        int list_numb = 0;
        //int list_count = 0;

        List<Line> list;
        List<List<Line>> mas_list;
        List<Line> group;
        List<List<Line>> mas_group;
        List<Line> morf;
        List<List<Line>> mas_morf;
        Line line = null;
        Line tmp = null;
        List<Line> oldPosition;
        List<List<Line>> mas_position;
        List<MyLine> spisok;
        List<List<MyLine>> mass_spisok;
        List<Point> oldPoint;
        Point point;

        bool IsDrawing = false;
        bool IsLine = false;
        bool Group = false;
        bool Morf = false;
        new bool Cursor = true;
        bool IsDotLine1 = false;
        bool IsDotLine2 = false;

        static public double ux = 0;
        static public double uy = 0;
        static public double Iter = 0;
        static public bool ok = false;
        static public double StvolV = 0;
        static public double StvolT = 0;
        static public double VerVet = 0;
        static public double Razbros = 0;
        #endregion

        #region Load
        public MainWindow()
        {
            InitializeComponent();
            ListNumBox.IsReadOnly = true;
            list = new List<Line>(count);
            group = new List<Line>(group_count);
            morf = new List<Line>(morf_count);
            oldPosition = new List<Line>();
            mas_position = new List<List<Line>>();
            //spisok = new List<MyLine>();
            mass_spisok = new List<List<MyLine>>();
            mas_list = new List<List<Line>>();
            mas_group = new List<List<Line>>();
            mas_morf = new List<List<Line>>();
            oldPoint = new List<Point>();
        }
        #endregion

        #region Line
        private Line CopyLine(Line line)
        {
            Line l1 = new Line();
            l1.X1 = line.X1;
            l1.X2 = line.X2;
            l1.Y1 = line.Y1;
            l1.Y2 = line.Y2;
            return l1;
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            line = sender as Line;
            if (Cursor)
            {
                point = e.GetPosition(canvas);
                if ((Math.Pow((point.X-line.X1), 2) + Math.Pow((point.Y-line.Y1),2)) < 16)
                {
                    IsDotLine1 = true;
                    line.Stroke = Brushes.Red;
                }
                else if ((Math.Pow((point.X - line.X2), 2) + Math.Pow((point.Y - line.Y2), 2)) < 16)
                {
                    IsDotLine2 = true;
                    line.Stroke = Brushes.Red;
                }
                else
                {
                    if (!group.Contains(line))
                        (line).Stroke = Brushes.Red;
                    else
                        for (int i = 0; i < group_count; i++)
                            group[i].Stroke = Brushes.Red;
                    Mouse.Capture(line);
                }
            }
            if (Group)
            {
                if (!group.Contains(line))
                {
                    line.Stroke = Brushes.Blue;
                    group.Add(line);
                    group_count = group_count + 1;
                }
            }
            if(Morf)
            {
                if (!morf.Contains(line))
                {
                    line.Stroke = Brushes.Pink;
                    morf.Add(line);
                    oldPosition.Add(CopyLine(line));
                    morf_count = morf_count + 1;
                }
            }
            IsLine = true;
        }

        private void Line_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            line = sender as Line;
            if (Cursor)
            {
                if (MessageBox.Show("Подтвердите удаление объекта!", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (group.Contains(line))
                    {
                        int ch = group_count;
                        for (int i = 0; i < ch; i++)
                        {
                            list.Remove(group[i]);
                            count--;
                            canvas.Children.Remove(group[i]);
                            group_count = group_count - 1;
                        }
                        group.Clear();
                        canvas.Children.Remove(line);
                    }
                    else
                    {
                        if (morf.Contains(line)) morf.Remove(line);
                        canvas.Children.Remove(line);
                        list.Remove(line);
                        count = count - 1;
                    }
                }
            }
            if (Group)
            {
                if (group.Contains(line))
                {
                    if(!morf.Contains(line))
                        line.Stroke = Brushes.Black;
                    group.Remove(line);
                    group_count = group_count - 1;
                }
            }
            if(Morf)
            {
                if (morf.Contains(line))
                {
                    if (!group.Contains(line))
                        line.Stroke = Brushes.Black;
                    else
                        line.Stroke = Brushes.Blue;
                    morf.Remove(line);
                    oldPosition.Remove(line);
                    morf_count = morf_count - 1;
                }
            }
        }
        private void Line_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            line = sender as Line;
            if (Cursor)
            {
                if (IsDotLine1 == false & IsDotLine2 == false)
                {
                    if (IsLine)
                    {
                        if (!group.Contains(line))
                        {
                            if (morf.Contains(line))
                                line.Stroke = Brushes.Pink;
                            else
                                (line).Stroke = Brushes.Black;
                            Mouse.Capture(null);
                            Canvas.SetLeft(line, 0);
                            Canvas.SetTop(line, 0);
                            line.X1 = line.X1 + (e.GetPosition(canvas).X - point.X);
                            line.X2 = line.X2 + (e.GetPosition(canvas).X - point.X);
                            line.Y1 = line.Y1 + (e.GetPosition(canvas).Y - point.Y);
                            line.Y2 = line.Y2 + (e.GetPosition(canvas).Y - point.Y);
                        }
                        else
                        {
                            Mouse.Capture(null);
                            for (int i = 0; i < group_count; i++)
                            {
                                group[i].Stroke = Brushes.Blue;
                                Canvas.SetLeft(group[i], 0);
                                Canvas.SetTop(group[i], 0);
                                group[i].X1 = group[i].X1 + (e.GetPosition(canvas).X - point.X);
                                group[i].X2 = group[i].X2 + (e.GetPosition(canvas).X - point.X);
                                group[i].Y1 = group[i].Y1 + (e.GetPosition(canvas).Y - point.Y);
                                group[i].Y2 = group[i].Y2 + (e.GetPosition(canvas).Y - point.Y);
                            }
                        }
                    }
                }
                if (!group.Contains(line))
                {
                    if (morf.Contains(line))
                        line.Stroke = Brushes.Pink;
                    else
                        (line).Stroke = Brushes.Black;
                }
                else
                    for (int i = 0; i < group_count; i++)
                    {
                        if (morf.Contains(group[i]))
                            group[i].Stroke = Brushes.Pink;
                        else
                            group[i].Stroke = Brushes.Blue;
                    }
                }
            IsDotLine1 = false;
            IsDotLine2 = false;
            IsLine = false;
        }
        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cursor)
            {
                if (IsLine)
                {
                    if (Mouse.Captured is Line)
                    {
                        if (!group.Contains((Line)sender))
                        {
                            tmp = Mouse.Captured as Line;
                            Canvas.SetLeft(tmp, e.GetPosition(canvas).X - point.X);
                            Canvas.SetTop(tmp, e.GetPosition(canvas).Y - point.Y);
                        }
                        else
                        {
                            for (int i = 0; i < group_count; i++)
                            {
                                Canvas.SetLeft(group[i], e.GetPosition(canvas).X - point.X);
                                Canvas.SetTop(group[i], e.GetPosition(canvas).Y - point.Y);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Canvas
        private void canvas_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (Cursor)
            {
                if (IsLine == false)
                {
                    point = e.GetPosition(canvas);
                    line = new Line();
                    line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                    line.MouseMove += Line_MouseMove;
                    line.MouseLeftButtonUp += Line_MouseLeftButtonUp;
                    line.MouseRightButtonDown += Line_MouseRightButtonDown;
                    line.Stroke = Brushes.Black;
                    line.StrokeThickness = LineSlider.Value;
                    line.X1 = line.X2 = point.X;
                    line.Y1 = line.Y2 = point.Y;
                    canvas.Children.Add(line);
                    count++;
                    list.Add(line);
                    IsDrawing = true;
                }
            }
        }

        private void canvas_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            IsDrawing = false;
            IsLine = false;
            line = null;
            IsDotLine1 = false;
            IsDotLine2 = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cursor)
            {
                if (IsDotLine1 == false & IsDotLine2 == false)
                {
                    if (IsDrawing)
                    {
                        Point pointEnd = e.GetPosition(canvas);
                        line.X2 = pointEnd.X;
                        line.Y2 = pointEnd.Y;
                    }
                }
                else if (IsDotLine1)
                {
                    line.X1 = e.GetPosition(canvas).X;
                    line.Y1 = e.GetPosition(canvas).Y;
                }
                else if (IsDotLine2)
                {
                    line.X2 = e.GetPosition(canvas).X;
                    line.Y2 = e.GetPosition(canvas).Y;
                }
            }
            label1.Content = e.GetPosition(canvas).X;
            label2.Content = e.GetPosition(canvas).Y;
        }
        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Подтвердите очистку поля!", "Очистка", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                canvas.Children.Clear();
                list.Clear();
                group.Clear();
                morf.Clear();
                count = 0;
                group_count = 0;
                morf_count = 0;
            }
        }
        #endregion

        #region Buttons
        private void CursorBut_Click(object sender, RoutedEventArgs e)
        {
            CursorBut.Background = Brushes.Gray;
            GroupBut.Background = Brushes.LightGray;
            MorfBut.Background = Brushes.LightGray;
            Cursor = true;
            Group = false;
            Morf = false;
        }

        private void GroupBut_Click(object sender, RoutedEventArgs e)
        {
            CursorBut.Background = Brushes.LightGray;
            GroupBut.Background = Brushes.Gray;
            MorfBut.Background = Brushes.LightGray;
            Cursor = false;
            Group = true;
            Morf = false;
        }

        private void MorfBut_Click(object sender, RoutedEventArgs e)
        {
            MorfSlider.Value = 0;
            CursorBut.Background = Brushes.LightGray;
            GroupBut.Background = Brushes.LightGray;
            MorfBut.Background = Brushes.Gray;
            Cursor = false;
            Group = false;
            Morf = true;
        }
        #endregion

        #region Morfing
        private void MorfSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Morf & morf.Count > 0)
            { 
                if (morf_count % 2 != 0) tmp1 = morf_count - 1;
                else tmp1 = morf_count;
            
                for (int i = 0; i < tmp1-1; i = i + 2) {
                    if (e.NewValue > e.OldValue)
                    {
                        morf[i].X1 = (1 - (MorfSlider.Value / 10)) * morf[i].X1 + (MorfSlider.Value / 10) * morf[i+1].X1;
                        morf[i].X2 = (1 - (MorfSlider.Value / 10)) * morf[i].X2 + (MorfSlider.Value / 10) * morf[i+1].X2;
                        morf[i].Y1 = (1 - (MorfSlider.Value / 10)) * morf[i].Y1 + (MorfSlider.Value / 10) * morf[i+1].Y1;
                        morf[i].Y2 = (1 - (MorfSlider.Value / 10)) * morf[i].Y2 + (MorfSlider.Value / 10) * morf[i+1].Y2;
                    }
                    if (e.NewValue < e.OldValue)
                    {
                        morf[i].X1 = (1-(MorfSlider.Value / 10)) * morf[i].X1 + (MorfSlider.Value / 10) * oldPosition[i].X1;
                        morf[i].X2 = (1-(MorfSlider.Value / 10)) * morf[i].X2 + (MorfSlider.Value / 10) * oldPosition[i].X2;
                        morf[i].Y1 = (1-(MorfSlider.Value / 10)) * morf[i].Y1 + (MorfSlider.Value / 10) * oldPosition[i].Y1;
                        morf[i].Y2 = (1-(MorfSlider.Value / 10)) * morf[i].Y2 + (MorfSlider.Value / 10) * oldPosition[i].Y2;
                    }
                }
            }
        }

        #endregion

        #region 2D-Graphic
        private void t11_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        static double[,] MultiplicationMatr(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        r[i,j] += a[i,k] * b[k,j];
                    }
                }
            }
            return r;
        }

        private double[,] MatrizaLine(Line line)
        {
            double[,] mas = new double[2, 3];
            mas[0, 0] = line.X1;
            mas[0, 1] = line.Y1;
            mas[0, 2] = 1;
            mas[1, 0] = line.X2;
            mas[1, 1] = line.Y2;
            mas[1, 2] = 1;
            return mas;
        }

        private double[,] Matriza2D()
        {
            double[,] mas = new double[3, 3];
            mas[0, 0] = Convert.ToDouble(t11.Text);
            mas[0, 1] = Convert.ToDouble(t12.Text);
            mas[0, 2] = Convert.ToDouble(t13.Text);
            mas[1, 0] = Convert.ToDouble(t21.Text);
            mas[1, 1] = Convert.ToDouble(t22.Text);
            mas[1, 2] = Convert.ToDouble(t23.Text);
            mas[2, 0] = Convert.ToDouble(t31.Text);
            mas[2, 1] = Convert.ToDouble(t32.Text);
            mas[2, 2] = Convert.ToDouble(t33.Text);
            return mas;
        }

        private void MatrToLine(Line l, double[,] a)
        {
            //Line l = new Line();
            l.X1 = a[0, 0];
            l.X2 = a[1, 0];
            l.Y1 = a[0, 1];
            l.Y2 = a[1, 1];
            //l.Stroke = Brushes.Blue;
            //return l;
        }

        private void _2dComandBut_Click(object sender, RoutedEventArgs e)
        {
            if (group.Count != 0)
            {
                try
                {
                    int i = 0;
                   // List<double[,]> mas = new List<double[,]>(group.Count);
                    for (i = 0; i < group_count; i++)
                    {
                        double[,] mas = new double[2, 3];
                        mas = MatrizaLine(group[i]);
                        mas = MultiplicationMatr(mas, Matriza2D());
                        MatrToLine(group[i], mas);
                    }
                    /*for (i = 0; i < group_count; i++)
                    {
                    }
                    for (i = 0; i < group_count; i++)
                    {
                    }*/
                }
                catch (Exception q)
                {
                    MessageBox.Show(q.Message);
                }
            }
        }
        #endregion

        #region Fractal

        private Line CreateLineFromTree(double x, double y, double t, double u, double w)
        {
            line = new Line();
            line.X1 = x;
            line.Y1 = y;
            if (t > 2)
                line.Stroke = Brushes.Brown;
            else
                line.Stroke = Brushes.Green;
            line.X2 = x + (w * Math.Cos(u));
            line.Y2 = y + (w * Math.Sin(u));
            line.StrokeThickness = t;
            line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
            line.MouseMove += Line_MouseMove;
            line.MouseLeftButtonUp += Line_MouseLeftButtonUp;
            line.MouseRightButtonDown += Line_MouseRightButtonDown;
            list.Add(line);
            group.Add(line);
            group_count++;
            count++;
            return line;
        }

        public void Fractal(double u, double i, double t, double w, double x, double y)
        {
            r = new Random();
            if (r.NextDouble() < VerVet)
            {
                for (int j = 0; j < 2; j++)
                {
                    line = CreateLineFromTree(x, y, t, u, w); 
                    if (i > 1)
                    {
                        if (j == 0)
                            Fractal(u + (ux + r.NextDouble() * Razbros) * Math.PI / 180, i - 1, t / 1.35, w / 1.35, line.X2, line.Y2);
                        if (j == 1)
                            Fractal(u - (uy + 1 - r.NextDouble() * Razbros) * Math.PI / 180, i - 1, t / 1.35, w / 1.35, line.X2, line.Y2);
                    }
                }
            }
            else
            {
                line = CreateLineFromTree(x, y, t, u, w); 
                if (i > 1)
                    Fractal(u, i - 1, t / 1.35, w / 1.35, line.X2, line.Y2);
            }
        }

        private void FractalBut_Click(object sender, RoutedEventArgs e)
        {
            FraktalWin win = new FraktalWin();
            win.CreatrFraktBut.Click += CreatrFraktBut_Click;
            win.Show();
        }

        private void CreatrFraktBut_Click(object sender, RoutedEventArgs e)
        {
            if (ok)
            {
                try
                {
                    group_count = 0;
                    group = new List<Line>(group_count);
                    r = new Random();
                    Fractal(Math.PI/2, Iter, StvolT, StvolV, 400 + r.NextDouble()*5, 100 + r.NextDouble()*5);
                    foreach (Line x in list)
                        if (!canvas.Children.Contains(x))
                            canvas.Children.Add(x);
                }
                catch (Exception q)
                {
                    MessageBox.Show(q.Message);
                }
                finally
                {
                    ok = false;
                }
            }
        }

        #endregion

        #region List
        private void PrevListBut_Click(object sender, RoutedEventArgs e)
        {
            if(list_numb > 0)
            {
                canvas.Children.Clear();
                if (list_numb < mas_list.Count)
                {
                    mas_list[list_numb] = list;
                    mas_group[list_numb] = group;
                    mas_morf[list_numb] = morf;
                    mas_position[list_numb] = oldPosition;
                    list_numb--;
                    list = mas_list[list_numb];
                    group = mas_group[list_numb];
                    morf = mas_morf[list_numb];
                    count = mas_list[list_numb].Count;
                    group_count = mas_group[list_numb].Count;
                    morf_count = mas_morf[list_numb].Count;
                    oldPosition = mas_position[list_numb];
                    foreach (Line x in list)
                        canvas.Children.Add(x);
                }
                 else
                {
                    if (!mas_list.Contains(list))
                    {
                        mas_list.Add(list);
                        mas_group.Add(group);
                        mas_morf.Add(morf);
                        mas_position.Add(oldPosition);
                    }
                    canvas.Children.Clear();
                    list_numb--;
                    list = mas_list[list_numb];
                    group = mas_group[list_numb];
                    morf = mas_morf[list_numb];
                    oldPosition = mas_position[list_numb];
                    count = mas_list[list_numb].Count;
                    group_count = mas_group[list_numb].Count;
                    morf_count = mas_morf[list_numb].Count;
                    foreach (Line x in list)
                        canvas.Children.Add(x);
                }
                ListNumBox.Text = Convert.ToString(list_numb);
            }
        }

        private void NextListBut_Click(object sender, RoutedEventArgs e)
        {
            if (list_numb + 1 < mas_list.Count)
            {
                mas_list[list_numb] = list;
                mas_group[list_numb] = group;
                mas_morf[list_numb] = morf;
                list_numb++;
                canvas.Children.Clear();
                list = mas_list[list_numb];
                group = mas_group[list_numb];
                morf = mas_morf[list_numb];
                oldPosition = mas_position[list_numb];
                count = mas_list[list_numb].Count;
                group_count = mas_group[list_numb].Count;
                morf_count = mas_morf[list_numb].Count;
                foreach (Line x in list)
                    canvas.Children.Add(x);
            }
            else
            {
                if (!mas_list.Contains(list))
                {
                    mas_list.Add(list);
                    mas_group.Add(group);
                    mas_morf.Add(morf);
                    mas_position.Add(oldPosition);
                }
                canvas.Children.Clear();
                list = new List<Line>(count);
                group = new List<Line>(group_count);
                morf = new List<Line>(morf_count);
                oldPosition = new List<Line>();
                count = 0;
                group_count = 0;
                morf_count = 0;
                list_numb++;
            }
            ListNumBox.Text = Convert.ToString(list_numb);
        }

        private void LoadListBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (canvas.Children.Count != 0)
                    if (MessageBox.Show("Лист содержит элементы! Заменить?", "Загрузка из файла", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                else
                    {
                        list.Clear();
                        count = 0;
                        group.Clear();
                        group_count = 0;
                        morf.Clear();
                        morf_count = 0;
                        canvas.Children.Clear();
                    }
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Binary files(*.bin)|*.bin|All files(*.*)|*.*";
                if (ofd.ShowDialog() == false) return;
                BinaryFormatter bf = new BinaryFormatter();
                string filename = ofd.FileName;
                Stream str = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                spisok = (List<MyLine>)bf.Deserialize(str);
                int i = 0;
                foreach(MyLine x in spisok)
                {
                    line = x.GetLine();
                    line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                    line.MouseMove += Line_MouseMove;
                    line.MouseLeftButtonUp += Line_MouseLeftButtonUp;
                    line.MouseRightButtonDown += Line_MouseRightButtonDown;
                    list.Add(line);
                    count++;
                    if(x.Is_Group)
                    {
                        group.Add(line);
                        group_count++;
                    }
                    if(x.Is_Morf)
                    {
                        while (morf.Count - 1 < x.Morf_Ind)
                        {
                            oldPosition.Add(new Line());
                            morf.Add(new Line());
                        }
                        morf.Insert(x.Morf_Ind, line);
                        morf_count++;
                        tmp = new Line();
                        tmp.X1 = x.oldx1;
                        tmp.X2 = x.oldx2;
                        tmp.Y1 = x.oldy1;
                        tmp.Y2 = x.oldy2;
                        tmp.Stroke = Brushes.Pink;
                        oldPosition.Insert(x.Morf_Ind, tmp);
                    }
                    canvas.Children.Add(line);
                    i++;
                }
            }
            catch(Exception q)
            {
                MessageBox.Show(q.Message);
            }
        }

        private List<MyLine> GetSpisok()
        {
            int i = 0;
            spisok = new List<MyLine>();
            foreach(Line x in list)
            {
                spisok.Add(new MyLine(group.Contains(x), morf.Contains(x), x.X1, x.X2, x.Y1, x.Y2, x.Stroke.GetHashCode(), x.StrokeThickness, group.IndexOf(x), morf.IndexOf(x)));
                if (morf.Contains(x))
                {
                    spisok[i].oldx1 = oldPosition[morf.IndexOf(x)].X1;
                    spisok[i].oldx2 = oldPosition[morf.IndexOf(x)].X2;
                    spisok[i].oldy1 = oldPosition[morf.IndexOf(x)].Y1;
                    spisok[i].oldy2 = oldPosition[morf.IndexOf(x)].Y2;

                }
                i++;
            }
            return spisok;
        }

        private void SaveListBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Binary files(*.bin)|*.bin|All files(*.*)|*.*";
                sfd.FileName = "file1.bin";
                if (sfd.ShowDialog() == false) return;
                BinaryFormatter bf = new BinaryFormatter();
                string filename = sfd.FileName;
                Stream str = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                spisok = GetSpisok();
                bf.Serialize(str, spisok);
                str.Close();
            }
            catch(Exception q)
            {
                MessageBox.Show(q.Message);
            }
        }
        #endregion
    }
}