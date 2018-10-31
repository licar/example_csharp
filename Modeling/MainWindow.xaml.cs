using Modeling.Common.Enums;
using Modeling.Modes;
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
        private const double SIZE_ROW = 95;
        private const int HEIGHT = 10;
        private const int WIDHT = 10;
        private BrushConverter conv = new BrushConverter();

        private Grid grid;

        public MainWindow()
		{
			InitializeComponent();
            var island = new Island(HEIGHT, WIDHT);
            grid = this.FindName(GRID) as Grid;
            GenerateRows(HEIGHT, WIDHT, island);
		}

        public void GenerateRows(int height, int widht, Island island)
        {
            var groupBox =  (GroupBox)this.FindName(GROUP_BOX);
            var cloneName = XamlWriter.Save(groupBox);

            for (int i = 0; i != height; ++i)
            {
                for (int j = 0; j != widht; ++j)
                {
                    var cloneGroupBox = Load(cloneName);
                    cloneGroupBox.Name = $"{GROUP_BOX}{i}_{j}";
                    cloneGroupBox.Margin = new Thickness(i * SIZE_ROW, 0, 0, j * SIZE_ROW);
                    
                    ChangeCellColor(island.Cells[i, j], cloneGroupBox);

                    grid.Children.Add(cloneGroupBox);
                }
            }
        }

        private void ChangeCellColor(ICell cell, GroupBox groupBox)
        {
            if (cell.GetLocality() == Locality.Field)
            {
                switch (cell.GetJuiciness())
                {
                    case 0:
                        groupBox.Background = conv.ConvertFromString("#E9DE7C") as SolidColorBrush;
                        break;
                    case 1:
                        groupBox.Background = conv.ConvertFromString("#DDE97C") as SolidColorBrush;
                        break;
                    case 2:
                        groupBox.Background = conv.ConvertFromString("#B6E46C") as SolidColorBrush;
                        break;
                    case 3:
                        groupBox.Background = conv.ConvertFromString("#94E46C") as SolidColorBrush;
                        break;
                    case 4:
                        groupBox.Background = conv.ConvertFromString("#6CE479") as SolidColorBrush;
                        break;

                }
                
            }else if (cell.GetLocality() == Locality.Hill)
            {
                groupBox.Background = Brushes.Gray;
            }
            else
            {
                groupBox.Background = Brushes.Blue;
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
