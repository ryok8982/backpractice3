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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;
using System.Xml.Linq;

namespace backPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }
        private void ShowData()
        {
            ListBox1.Items.Clear();

            XElement xml = XElement.Load(@"test004.xml");
            var infos = xml.Elements("ListOfInspectionMethod").Select(x => x.Element("MethodName"));
            {

                foreach (String info in infos)
                {
                    ListBox1.Items.Add(info);
                }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        {

            ShowData();

        }
    

        private void CS_AutoChecked(object sender, RoutedEventArgs e)
        {
            CS_max.IsChecked = false;
            CS_min.IsChecked = false;
        }

        private void CS_MinChecked(object sender, RoutedEventArgs e)
        {
            CS_auto.IsChecked = false;
            CS_max.IsChecked = false;
        }

        private void CS_MaxChecked(object sender, RoutedEventArgs e)
        {
            CS_auto.IsChecked = false;
            CS_min.IsChecked = false;
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.Encoding = Encoding.UTF8;

            int IntTest = Convert.ToInt32(CS_auto.IsChecked);
            int IntMax = Convert.ToInt32(CS_max.IsChecked);
            int IntMin = Convert.ToInt32(CS_min.IsChecked);

            var xml = new XElement("ListOfInspectionMethod",
            new XElement("MethodName", TextBox1.Text),
            new XElement("comparisonSetting",
            new XElement("autoAdjustment", IntTest),
            new XElement("CS_max", IntMax),
            new XElement("CS_min", IntMin),
            new XElement("MaximumResolution", CS_maxResolution.Text)));


            xml.Save(TextBox_export.Text + ".xml");

            MessageBox.Show("Write Complete!");

        }
    

        private void button_write_Click(object sender, RoutedEventArgs e)
        {
            int IntTest = Convert.ToInt32(CS_auto.IsChecked);
            int IntMax = Convert.ToInt32(CS_max.IsChecked);
            int IntMin = Convert.ToInt32(CS_min.IsChecked);

            XElement xml = XElement.Load(@"test004.xml");

            XElement datas = new XElement("ListOfInspectionMethod",
            new XElement("MethodName", TextBox1.Text),
            new XElement("comparisonSetting",
            new XElement("autoAdjustment", IntTest),
            new XElement("CS_max", IntMax),
            new XElement("CS_min", IntMin),
            new XElement("MaximumResolution", CS_maxResolution.Text)));

            xml.Add(datas);

            xml.Save(@"test004.xml");

            MessageBox.Show("Write Complete!");

            ShowData();
        }


        private void PrintText(object sender, RoutedEventArgs e)
        { 
        var xml = XElement.Load(@"test004.xml");
        var lbi = ListBox1.SelectedItem.ToString();
        //メンバー情報のタグ内の情報を取得する
        var cs = (from item in xml.Elements("ListOfInspectionMethod")
                    where item.Element("MethodName").Value == lbi
                    select item ).Single();

        var tes = cs.Element("comparisonSetting");
            int auto = Convert.ToInt32(tes.Element("autoAdjustment").Value);
            CS_auto.IsChecked = Convert.ToBoolean(auto);

            int max = Convert.ToInt32(tes.Element("CS_max").Value);
            CS_max.IsChecked = Convert.ToBoolean(max);

            int min = Convert.ToInt32(tes.Element("CS_min").Value);
            CS_min.IsChecked = Convert.ToBoolean(min);

            string MaxRes = tes.Element("MaximumResolution").Value;
            CS_maxResolution.Text = MaxRes;

        }
        private void button_reference_Click(object sender, RoutedEventArgs e)
        {
            // ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            // ファイルの種類を設定
            dialog.Filter = "テキストファイル (*.xml)|*.xml|全てのファイル (*.*)|*.*";



            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                // 選択されたファイル名 (ファイルパス) をメッセージボックスに表示
                //xmlファイルを指定する
                string filename = dialog.FileName;
                var doc = XElement.Load(filename);

                XElement cs = doc.Element("comparisonSetting");

                int auto = Convert.ToInt32(cs.Element("autoAdjustment").Value);
                CS_auto.IsChecked = Convert.ToBoolean(auto);

                int max = Convert.ToInt32(cs.Element("CS_max").Value);
                CS_max.IsChecked = Convert.ToBoolean(max);

                int min = Convert.ToInt32(cs.Element("CS_min").Value);
                CS_min.IsChecked = Convert.ToBoolean(min);

                string MaxRes = cs.Element("MaximumResolution").Value;
                CS_maxResolution.Text = MaxRes;
            }

        }
    }
}
