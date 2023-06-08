using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Banico_Exercise08.TowerOfHanoi;

namespace Banico_Exercise08
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Moves = 0;
        private List<Peg> PegList= new List<Peg>();
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            addTocmd();
            timer.Interval = TimeSpan.FromSeconds(0.3);
            timer.Tick += timer_tick;
        }

        private void addTocmd()
        {
            string[] name = new string[] { txt_peg1.Text, txt_peg2.Text, txt_peg3.Text };
            foreach (string x in name)
            {
                cmb_StartPeg.Items.Add(x);
                cmb_EndPeg.Items.Add(x);
            }

            cmb_EndPeg.Items.RemoveAt(0);
            cmb_StartPeg.SelectedIndex = 0;
            cmb_EndPeg.SelectedIndex = 1;
        }

        private void removeTocmd()
        {
            cmb_StartPeg.Items.Clear();
            cmb_EndPeg.Items.Clear();
            addTocmd();
        }

        private void txt_peg3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_peg3.Text == "" || txt_peg3.Text == " ") { txt_peg3.Text = "C"; }
            else if (txt_peg3.Text == txt_peg2.Text || txt_peg3.Text == txt_peg1.Text)
            {
                MessageBox.Show("Please enter a different name");
                txt_peg3.Text = "C";
            }
            else
            {
                removeTocmd();
            }
        }

        private void txt_peg2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_peg2.Text == "" || txt_peg2.Text == " ") { txt_peg2.Text = "B"; }
            else if (txt_peg3.Text == txt_peg2.Text || txt_peg2.Text == txt_peg1.Text)
            {
                MessageBox.Show("Please enter a different name");
                txt_peg2.Text = "B";
            }
            else
            {
                removeTocmd();
            }
        }

        private void txt_peg1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_peg1.Text == "" || txt_peg1.Text == " ") { txt_peg1.Text = "A"; }
            else if (txt_peg1.Text == txt_peg2.Text || txt_peg3.Text == txt_peg1.Text)
            {
                MessageBox.Show("Please enter a different name");
                txt_peg1.Text = "A";
            }
            else
            {
                removeTocmd();
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_StartPeg.SelectedIndex != -1 && cmb_EndPeg.SelectedIndex != -1)
            {
                PegsToShow();

                string StartPeg = cmb_StartPeg.SelectedItem.ToString();
                string EndPeg = cmb_EndPeg.SelectedItem.ToString();
                string AuxPeg = "";
                foreach (var x in cmb_StartPeg.Items)
                {
                    if (x.ToString() != StartPeg && x.ToString() != EndPeg)
                    {
                        AuxPeg = x.ToString();
                    }
                }

                if (!lst_instruc.Items.IsEmpty) { lst_instruc.Items.Clear(); }

                InstrucMove(int.Parse(txt_disks.Text), StartPeg, AuxPeg, EndPeg);

                txt_Instruc.Text = lst_instruc.Items[0].ToString();

                switch (cmb_StartPeg.SelectedIndex)
                {
                    case 0:
                        Start_PegPlacement(0);
                        break;
                    case 1:
                        Start_PegPlacement(240);
                        break;
                    case 2:
                        Start_PegPlacement(500);
                        break;
                }
            }
            else { MessageBox.Show("Please select Start and End Peg"); }
            
        }

        private void InstrucMove(int n, string StartPeg, string AuxPeg, string EndPeg) 
        {
            if (n == 1)
                lst_instruc.Items.Add($"{lst_instruc.Items.Count + 1}. Move disk {n} from {StartPeg} to {EndPeg}");
            else 
            {
                InstrucMove(n - 1, StartPeg, EndPeg, AuxPeg);

                lst_instruc.Items.Add($"{lst_instruc.Items.Count + 1}. Move disk {n} from {StartPeg} to {EndPeg}");

                InstrucMove(n-1, AuxPeg, StartPeg, EndPeg);
            }
        }
        
        private void PegMove(int n, string StartPeg, string AuxPeg, string EndPeg) 
        {
            if (n == 1) 
            {
                if (Moves <= lst_instruc.SelectedIndex) 
                {
                    PegSteps(n, EndPeg);
                }
                Moves += 1;
            }    
            else
            {
                PegMove(n - 1, StartPeg, EndPeg, AuxPeg);
                if (Moves <= lst_instruc.SelectedIndex) 
                {
                    PegSteps(n, EndPeg);
                }
                Moves += 1;

                PegMove(n - 1, AuxPeg, StartPeg, EndPeg);
            }
        }

        private void PegSteps(int n, string nextpos) 
        {
            int marginleft = 0;

            if (nextpos == txt_peg1.Text)
            {
                marginleft = 0;
            }
            else if (nextpos == txt_peg2.Text)
            {
                marginleft = 240;
            }
            else if (nextpos == txt_peg3.Text)
            {
                marginleft = 500;
            }

            var data = PegList[n-1];
            bool checker = false;
            if (nextpos == txt_peg1.Text) 
            {
                data.CurrentPeg = txt_peg1.Text;
                foreach (var x in PegList) 
                {
                    if (x.Name != data.Name && x.CurrentPeg == data.CurrentPeg) 
                    {
                        pegmargin(data.CurrentPosition, marginleft, n, false, data);
                        checker = true;
                        break;
                    }
                    else { checker = false; }
                }

                if (checker == false) { pegmargin(data.CurrentPosition, marginleft, n, true, data); }
            }

            else if (nextpos == txt_peg2.Text) 
            {
                data.CurrentPeg = txt_peg2.Text;
                foreach (var x in PegList)
                {
                    if (x.Name != data.Name && x.CurrentPeg == data.CurrentPeg)
                    {
                        pegmargin(data.CurrentPosition, marginleft, n, false, data);
                        checker = true;
                        break;
                    }
                    else { checker = false; }
                }

                if (checker == false) { pegmargin(data.CurrentPosition, marginleft, n, true, data); }
            }

            else if (nextpos == txt_peg3.Text) 
            {
                data.CurrentPeg = txt_peg3.Text; 
                foreach (var x in PegList)
                {
                    if (x.Name != data.Name && x.CurrentPeg == data.CurrentPeg)
                    {
                        pegmargin(data.CurrentPosition, marginleft, n, false, data);
                        checker = true;
                        break;
                    }
                    else { checker = false; }
                }

                if (checker == false) { pegmargin(data.CurrentPosition, marginleft, n, true, data); }
            }

        }

        private void pegmargin(string pos, int marginleft, int n, bool isAlone, Peg data) 
        {
            var splitpos = pos.Split(',');
            var margin = new List<int>();

            foreach (string x in splitpos) 
            {
                margin.Add(int.Parse(x));
            }

            if (isAlone) margin[1] = 380;
            else if (!isAlone) 
            {
                int samepegcc = 0;
                foreach (var x in PegList) 
                {
                    if (x.Name != data.Name && x.CurrentPeg == data.CurrentPeg) 
                    {
                        samepegcc++;
                    }
                }
                margin[1] = 380 - (25 * (samepegcc));
            }

            margin[0] = marginleft;

            sequence(int.Parse(txt_disks.Text), n, margin);

            data.CurrentPosition = $"{margin[0]},{margin[1]},{margin[2]},{margin[3]}";
        }

        private void sequence(int totaldisks, int n, List<int> margin)
        {
            if (totaldisks == 1)
            {
                switch (n)
                {
                    case 1:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 2)
            {
                switch (n)
                {
                    case 2:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p11.Margin = new Thickness(65+ margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 3)
            {
                switch (n)
                {
                    case 3:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 4)
            {
                switch (n)
                {
                    case 4:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p9.Margin = new Thickness(86 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 5)
            {
                switch (n)
                {
                    case 5:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 6)
            {
                switch (n)
                {
                    case 6:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 7)
            {
                switch (n)
                {
                    case 7:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 6:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p6.Margin = new Thickness(115 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 8)
            {
                switch (n)
                {
                    case 8:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 7:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 6:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p6.Margin = new Thickness(115 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p5.Margin = new Thickness(125 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 9)
            {
                switch (n)
                {
                    case 9:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 8:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 7:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 6:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p6.Margin = new Thickness(115 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p5.Margin = new Thickness(125 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p4.Margin = new Thickness(135 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 10)
            {
                switch (n)
                {
                    case 10:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 9:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 8:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 7:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 6:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p6.Margin = new Thickness(115 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p5.Margin = new Thickness(125 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p4.Margin = new Thickness(135 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p3.Margin = new Thickness(145 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 11)
            {
                switch (n)
                {
                    case 11:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 10:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 9:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 8:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 7:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 6:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p6.Margin = new Thickness(115 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p5.Margin = new Thickness(125 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p4.Margin = new Thickness(135 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p3.Margin = new Thickness(145 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p2.Margin = new Thickness(155 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
            else if (totaldisks == 12)
            {
                switch (n)
                {
                    case 12:
                        p12.Margin = new Thickness(55 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 11:
                        p11.Margin = new Thickness(65 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 10:
                        p10.Margin = new Thickness(75 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 9:
                        p9.Margin = new Thickness(85 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 8:
                        p8.Margin = new Thickness(95 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 7:
                        p7.Margin = new Thickness(105 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 6:
                        p6.Margin = new Thickness(115 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 5:
                        p5.Margin = new Thickness(125 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 4:
                        p4.Margin = new Thickness(135 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 3:
                        p3.Margin = new Thickness(145 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 2:
                        p2.Margin = new Thickness(155 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                    case 1:
                        p1.Margin = new Thickness(165 + margin[0], margin[1], margin[2], margin[3]);
                        break;
                }
            }
        }

        private List<Peg> AddPegToVar()
        {
            var PegList = new List<Peg>();
            int name = 1;

            if (p1.IsVisible) 
            { 
                PegList.Add(new Peg(name, p1.Margin.ToString(), 1, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p2.IsVisible) 
            { 
                PegList.Add(new Peg(name, p2.Margin.ToString(), 2, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p3.IsVisible) 
            { 
                PegList.Add(new Peg(name, p3.Margin.ToString(), 3, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p4.IsVisible) 
            { 
                PegList.Add(new Peg(name, p4.Margin.ToString(), 4, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p5.IsVisible) 
            { 
                PegList.Add(new Peg(name, p5.Margin.ToString(), 5, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p6.IsVisible) 
            { 
                PegList.Add(new Peg(name, p6.Margin.ToString(), 6, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p7.IsVisible) 
            { 
                PegList.Add(new Peg(name, p7.Margin.ToString(), 7, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p8.IsVisible) 
            { 
                PegList.Add(new Peg(name, p8.Margin.ToString(), 8, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p9.IsVisible) 
            { 
                PegList.Add(new Peg(name, p9.Margin.ToString(), 9, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p10.IsVisible) 
            { 
                PegList.Add(new Peg(name, p10.Margin.ToString(), 10, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p11.IsVisible)
            {
                PegList.Add(new Peg(name, p11.Margin.ToString(), 11, cmb_StartPeg.SelectedItem.ToString()));
                name++;
            }
            if (p12.IsVisible) 
            { 
                PegList.Add(new Peg(name, p12.Margin.ToString(), 12, cmb_StartPeg.SelectedItem.ToString()));
            }

            return PegList;
        }

        private void PegsToShow()
        {
            p12.Visibility = Visibility.Hidden;
            p11.Visibility = Visibility.Hidden;
            p10.Visibility = Visibility.Hidden;
            p9.Visibility = Visibility.Hidden;
            p8.Visibility = Visibility.Hidden;
            p7.Visibility = Visibility.Hidden;
            p6.Visibility = Visibility.Hidden;
            p5.Visibility = Visibility.Hidden;
            p4.Visibility = Visibility.Hidden;
            p3.Visibility = Visibility.Hidden;
            p2.Visibility = Visibility.Hidden;
            p1.Visibility = Visibility.Hidden;

            int n = int.Parse(txt_disks.Text);

            for (int x = n; x >= 0; x--)
            {
                switch (x)
                {
                    case 1:
                        p12.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        p11.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        p10.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        p9.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        p8.Visibility = Visibility.Visible;
                        break;
                    case 6:
                        p7.Visibility = Visibility.Visible;
                        break;
                    case 7:
                        p6.Visibility = Visibility.Visible;
                        break;
                    case 8:
                        p5.Visibility = Visibility.Visible;
                        break;
                    case 9:
                        p4.Visibility = Visibility.Visible;
                        break;
                    case 10:
                        p3.Visibility = Visibility.Visible;
                        break;
                    case 11:
                        p2.Visibility = Visibility.Visible;
                        break;
                    case 12:
                        p1.Visibility = Visibility.Visible;
                        break;

                }
            }
        }

        private void cmb_StartPeg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmb_StartPeg.SelectedIndex)
            {
                case 0:
                    Start_PegPlacement(0);
                    break;
                case 1:
                    Start_PegPlacement(240);
                    break;
                case 2:
                    Start_PegPlacement(500);
                    break;
            }

            if (cmb_StartPeg.HasItems) 
            {
                if (!cmb_EndPeg.Items.IsEmpty) { cmb_EndPeg.Items.Clear(); }
                string[] name = new string[] { txt_peg1.Text, txt_peg2.Text, txt_peg3.Text };
                foreach (string x in name)
                {
                    if (cmb_StartPeg.SelectedItem.ToString() != x)
                    {
                        cmb_EndPeg.Items.Add(x);
                    }
                }

                cmb_EndPeg.SelectedIndex = 1;
            }

            lst_instruc.Items.Clear();
            txt_Instruc.Text = "";
        }

        private void Start_PegPlacement(int addedMargin)
        {
            p12.Margin = new Thickness(55 + addedMargin, 380, 0, 0);
            p11.Margin = new Thickness(65 + addedMargin, 355, 0, 0);
            p10.Margin = new Thickness(75 + addedMargin, 330, 0, 0);
            p9.Margin = new Thickness(85 + addedMargin, 305, 0, 0);
            p8.Margin = new Thickness(95 + addedMargin, 280, 0, 0);
            p7.Margin = new Thickness(105 + addedMargin, 255, 0, 0);
            p6.Margin = new Thickness(115 + addedMargin, 230, 0, 0);
            p5.Margin = new Thickness(125 + addedMargin, 205, 0, 0);
            p4.Margin = new Thickness(135 + addedMargin, 180, 0, 0);
            p3.Margin = new Thickness(145 + addedMargin, 155, 0, 0);
            p2.Margin = new Thickness(155 + addedMargin, 130, 0, 0);
            p1.Margin = new Thickness(165 + addedMargin, 105, 0, 0);
        }

        private void btn_Restart_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();   
        }

        private void ClearFields() 
        {
            lst_instruc.Items.Clear();
            txt_Instruc.Text = "";
            rb_Manual.IsChecked = true;
            cmb_StartPeg.SelectedIndex = 0;
            cmb_EndPeg.SelectedIndex = 1;
            Start_PegPlacement(0);

            p12.Visibility = Visibility.Hidden;
            p11.Visibility = Visibility.Hidden;
            p10.Visibility = Visibility.Hidden;
            p9.Visibility = Visibility.Hidden;
            p8.Visibility = Visibility.Hidden;
            p7.Visibility = Visibility.Hidden;
            p6.Visibility = Visibility.Hidden;
            p5.Visibility = Visibility.Hidden;
            p4.Visibility = Visibility.Hidden;
            p3.Visibility = Visibility.Hidden;
            p2.Visibility = Visibility.Hidden;
            p1.Visibility = Visibility.Hidden;
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (!lst_instruc.Items.IsEmpty)
            {
                if (lst_instruc.SelectedIndex < lst_instruc.Items.Count)
                {
                    if (lst_instruc.SelectedIndex + 1 != lst_instruc.Items.Count) 
                    {
                        lst_instruc.SelectedIndex += 1;
                        txt_Instruc.Text = lst_instruc.SelectedItem.ToString();
                        lst_instruc.ScrollIntoView(lst_instruc.SelectedIndex);
                    }
                    
                }

                if (lst_instruc.SelectedIndex < lst_instruc.Items.Count)
                {
                    Moves = 0;

                    switch (cmb_StartPeg.SelectedIndex)
                    {
                        case 0:
                            Start_PegPlacement(0);
                            break;
                        case 1:
                            Start_PegPlacement(240);
                            break;
                        case 2:
                            Start_PegPlacement(500);
                            break;
                    }

                    PegList = AddPegToVar();

                    string StartPeg = cmb_StartPeg.SelectedItem.ToString();
                    string EndPeg = cmb_EndPeg.SelectedItem.ToString();
                    string AuxPeg = "";
                    foreach (var x in cmb_StartPeg.Items)
                    {
                        if (x.ToString() != StartPeg && x.ToString() != EndPeg)
                        {
                            AuxPeg = x.ToString();
                        }
                    }

                    PegMove(int.Parse(txt_disks.Text), StartPeg, AuxPeg, EndPeg);
                }
            }
            else { MessageBox.Show("Please Click the Add Button first"); }
        }

        private void btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            if (!lst_instruc.Items.IsEmpty)
            {
                if (lst_instruc.SelectedIndex >= 0)
                {
                    lst_instruc.SelectedIndex -= 1;
                    if (lst_instruc.SelectedIndex != -1) 
                    {
                        txt_Instruc.Text = lst_instruc.SelectedItem.ToString();
                        lst_instruc.ScrollIntoView(lst_instruc.SelectedIndex);
                    }
                    
                }
                else if (lst_instruc.SelectedIndex == -1)
                {
                    switch (cmb_StartPeg.SelectedIndex)
                    {
                        case 0:
                            Start_PegPlacement(0);
                            break;
                        case 1:
                            Start_PegPlacement(240);
                            break;
                        case 2:
                            Start_PegPlacement(500);
                            break;
                    }
                }

                if (lst_instruc.SelectedIndex >= 0)
                {
                    Moves = 0;

                    switch (cmb_StartPeg.SelectedIndex)
                    {
                        case 0:
                            Start_PegPlacement(0);
                            break;
                        case 1:
                            Start_PegPlacement(240);
                            break;
                        case 2:
                            Start_PegPlacement(500);
                            break;
                    }

                    PegList = AddPegToVar();

                    string StartPeg = cmb_StartPeg.SelectedItem.ToString();
                    string EndPeg = cmb_EndPeg.SelectedItem.ToString();
                    string AuxPeg = "";
                    foreach (var x in cmb_StartPeg.Items)
                    {
                        if (x.ToString() != StartPeg && x.ToString() != EndPeg)
                        {
                            AuxPeg = x.ToString();
                        }
                    }

                    PegMove(int.Parse(txt_disks.Text), StartPeg, AuxPeg, EndPeg);
                    
                }
                
            }
            else { MessageBox.Show("Please Click the Add Button first"); }


        }
        
        private void rb_Automatic_Checked(object sender, RoutedEventArgs e)
        {
            if (!lst_instruc.Items.IsEmpty)
            {
                timer.Start();
            }
            else 
            {
                MessageBox.Show("Please Click the Add Button First");
                rb_Manual.IsChecked = true;
            }
            
        }

        void timer_tick(object sender, EventArgs e) 
        {
            btn_Next_Click(btn_Next, EventArgs.Empty);
        }

        private void rb_Manual_Checked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void lst_instruc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Moves = 0;

            switch (cmb_StartPeg.SelectedIndex)
            {
                case 0:
                    Start_PegPlacement(0);
                    break;
                case 1:
                    Start_PegPlacement(240);
                    break;
                case 2:
                    Start_PegPlacement(500);
                    break;
            }

            PegList = AddPegToVar();

            string StartPeg = cmb_StartPeg.SelectedItem.ToString();
            string EndPeg = cmb_EndPeg.SelectedItem.ToString();
            string AuxPeg = "";
            foreach (var x in cmb_StartPeg.Items)
            {
                if (x.ToString() != StartPeg && x.ToString() != EndPeg)
                {
                    AuxPeg = x.ToString();
                }
            }

            PegMove(int.Parse(txt_disks.Text), StartPeg, AuxPeg, EndPeg);
        }
    }
}
