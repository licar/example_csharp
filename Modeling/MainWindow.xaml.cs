using Modeling.Common.Enums;
using Modeling.Modes;
using System;
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
        private const string GROUP_BOX = "TextBox";
        private const string GRID = "Grid";
        private Canvas[,] groupBox;

        private const double IMAGE_SIZE = 10;
        private const double SIZE_ROW = 70;
        private const int HEIGHT = 10;
        private const int WIDHT = 10;
        private BrushConverter conv = new BrushConverter();
        private Island island;

        private Canvas grid;
        private Canvas panel;

        public MainWindow()
		{
			InitializeComponent();
            island = new Island(HEIGHT, WIDHT);
            panel =  this.FindName("Panel") as Canvas;
            grid = panel.FindName(GRID) as Canvas;
            GenerateRows();
            
        }

        public void Refresh()
        {
            NextStep();
            
        }

        public void GenerateRows()
        {
            var groupBoxes = new Canvas[HEIGHT, WIDHT];
            for (int i = 0; i != HEIGHT; ++i)
            {
                for (int j = 0; j != WIDHT; ++j)
                {
                    var cloneGroupBox = new Canvas();
                    

                    cloneGroupBox.Width = SIZE_ROW;
                    cloneGroupBox.Height = SIZE_ROW; 
                    cloneGroupBox.Name = $"{GROUP_BOX}{i}_{j}";
                    cloneGroupBox.Margin = new Thickness(i * SIZE_ROW, j * SIZE_ROW, 0, 0);
                    panel.Children.Add(cloneGroupBox);

                    groupBoxes[i, j] = cloneGroupBox;
                    
                    UpdateCell(island.Cells[i, j], cloneGroupBox);
                }
            }
            groupBox = groupBoxes;
        }

        private void NextStep()
        {
            island.NextBeat();
            for (int i = 0; i != HEIGHT; ++i)
            {
                for (int j = 0; j != WIDHT; ++j)
                {
                    UpdateCell(island.Cells[i, j], groupBox[i, j]);
                }
            }
        }

        private void ChangeCellColor(ICell cell, Canvas groupBox)
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

        private void UpdateCell(ICell cell, Canvas groupBox)
        {
            ChangeCellColor(cell, groupBox);
            UpdateText(cell, groupBox);
        }

        private void UpdateText(ICell cell, Canvas groupBox)
        {
            if (cell.GetLocality() == Locality.Field)
            {
                groupBox.Children.Clear();

                for (int i = 0; i != cell.GetRubbits(); ++i)
                {
                    var image = new Canvas();
                    image.Margin = new Thickness(i * IMAGE_SIZE * 2 + IMAGE_SIZE, IMAGE_SIZE, 0, 0);
                    image.Height = IMAGE_SIZE;
                    image.Width = IMAGE_SIZE;
                    image.Background = Brushes.White;
                    groupBox.Children.Add(image);
                }

                for (int i = 0; i != cell.GetHunters(); ++i)
                {
                    var image = new Canvas();
                    image.Margin = new Thickness(i * IMAGE_SIZE * 2 + IMAGE_SIZE, IMAGE_SIZE * 3, 0, 0);
                    image.Height = IMAGE_SIZE;
                    image.Width = IMAGE_SIZE;
                    image.Background = Brushes.Brown;
                    groupBox.Children.Add(image);
                }

                for (int i = 0; i != cell.GetWolfs(); ++i)
                {
                    var image = new Canvas();
                    image.Margin = new Thickness(i * IMAGE_SIZE * 2 + IMAGE_SIZE, IMAGE_SIZE * 5, 0, 0);
                    image.Height = IMAGE_SIZE;
                    image.Width = IMAGE_SIZE;
                    image.Background = Brushes.Red;
                    groupBox.Children.Add(image);
                }

                //image.Background


                //var textBlock = groupBox.Children[0] as TextBlock;
                //textBlock.Text  = $"Rubbits : {cell.GetRubbits()}{Environment.NewLine}Hunters : {cell.GetHunters()}{Environment.NewLine}Wolfs : {cell.GetWolfs()}{Environment.NewLine}";
            }
        }

        private Canvas Load(string cloneName)
        {
            StringReader stringReader = new StringReader(cloneName);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as Canvas;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
