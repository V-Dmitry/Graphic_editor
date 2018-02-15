using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Graphic_editor_v1._0
{
    /// <summary>
    /// Логика взаимодействия для FraktalWin.xaml
    /// </summary>
    public partial class FraktalWin : Window
    {
        public bool ok = false;
        public FraktalWin()
        {
            InitializeComponent();
            ux.Text = "30";
            uy.Text = "30";
            Iter.Text = "10";
            StvolVisBox.Text = "100";
            StvolTolBox.Text = "10";
            VerVetBox.Text = "1";
            RazbrosBox.Text = "5";
        }

        private void CreatrFraktBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ux.Text != "" & uy.Text != "" & Iter.Text != "" & StvolTolBox.Text != "" & StvolVisBox.Text != "" & VerVetBox.Text != "" & RazbrosBox.Text != "")
                {
                    MainWindow.ux = Convert.ToDouble(ux.Text);
                    MainWindow.uy = Convert.ToDouble(uy.Text);
                    MainWindow.Iter = Convert.ToDouble(Iter.Text);
                    MainWindow.StvolT = Convert.ToDouble(StvolTolBox.Text);
                    MainWindow.StvolV = Convert.ToDouble(StvolVisBox.Text);
                    MainWindow.ok = true;
                    MainWindow.VerVet = Convert.ToDouble(VerVetBox.Text);
                    MainWindow.Razbros = Convert.ToDouble(RazbrosBox.Text);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены!");
                }
            }
            catch (Exception q)
            {
                MainWindow.ok = false;
                MessageBox.Show(q.Message);
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}