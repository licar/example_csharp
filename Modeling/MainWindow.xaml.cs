using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Modeling
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private const string GROUP_BOX = "GroupBox";
        private const string GRID = "Grid";
        private const double Widht = 95;

        private Grid grid;

        public MainWindow()
		{
			InitializeComponent();
            grid = this.FindName(GRID) as Grid;
            GenerateRows();
		}

        public void GenerateRows(int widht = 10, int height = 10)
        {
            var groupBox =  (GroupBox)this.FindName(GROUP_BOX);
            var cloneName = XamlWriter.Save(groupBox);

            for (int i = 0; i != height; ++i)
            {
                for (int j = 0; j != widht; ++j)
                {
                    var cloneGroupBox = Load(cloneName);
                    cloneGroupBox.Name = $"{GROUP_BOX}{i}_{j}";
                    cloneGroupBox.Margin = new Thickness(i * Widht, 0, 0, j * Widht);
                    grid.Children.Add(cloneGroupBox);
                }
            }


            
        }

        private GroupBox Load(string cloneName)
        {
            StringReader stringReader = new StringReader(cloneName);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as GroupBox;
        }

        //public void Refresh()
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(groupBox); i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(groupBox, i);
        //        var result = child as TextBlock;
        //    };
        //}
	}
}
