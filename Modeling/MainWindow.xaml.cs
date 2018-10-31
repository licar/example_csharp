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
        private TextBlock[,] groupBox;

        private const double SIZE_ROW = 100;
        private const int HEIGHT = 10;
        private const int WIDHT = 10;
        private BrushConverter conv = new BrushConverter();
        private Island island;

        private Grid grid;

        public MainWindow()
		{
			InitializeComponent();
            island = new Island(HEIGHT, WIDHT);
            grid = this.FindName(GRID) as Grid;
            GenerateRows();
            
            
        }

        public void Refresh()
        {
            NextStep();
        }

        public void GenerateRows()
        {
            var groupBoxes = new TextBlock[HEIGHT, WIDHT];
            for (int i = 0; i != HEIGHT; ++i)
            {
                for (int j = 0; j != WIDHT; ++j)
                {
                    var cloneGroupBox = new TextBlock();
                    cloneGroupBox.Width = SIZE_ROW;
                    cloneGroupBox.Height = SIZE_ROW; 
                    cloneGroupBox.Name = $"{GROUP_BOX}{i}_{j}";
                    cloneGroupBox.Margin = new Thickness(i * SIZE_ROW, j * SIZE_ROW, 0, 0);
                    grid.Children.Add(cloneGroupBox);
                    groupBoxes[i, j] = cloneGroupBox;

                    UpdateCell(island.Cells[i, j], cloneGroupBox);
                }
            }
            groupBox = groupBoxes;
        }

        private void NextStep()
        {
            for (int i = 0; i != HEIGHT; ++i)
            {
                for (int j = 0; j != WIDHT; ++j)
                {
                    island.NextBeat();
                    UpdateCell(island.Cells[i, j], groupBox[i, j]);
                }
            }
        }

        private void ChangeCellColor(ICell cell, TextBlock groupBox)
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

        private void UpdateCell(ICell cell, TextBlock groupBox)
        {
            ChangeCellColor(cell, groupBox);
            UpdateText(cell, groupBox);
        }

        private void UpdateText(ICell cell, TextBlock groupBox)
        {
            if (cell.GetLocality() == Locality.Field)
            {
                groupBox.Text = $"R : {cell.GetRubbits()}{Environment.NewLine}H : {cell.GetHunters()}{Environment.NewLine}W : {cell.GetWolfs()}{Environment.NewLine}";
            }
        }

        private TextBlock Load(string cloneName)
        {
            StringReader stringReader = new StringReader(cloneName);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as TextBlock;
        }
    }
}
