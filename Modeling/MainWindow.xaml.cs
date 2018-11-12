using Modeling.Common.Enums;
using Modeling.Modes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
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
	    private const string VIEW = "ScrollView";
	    private const string PANEL = "Panel";

        private Canvas[,] groupBox;

	    private const double PANEL_WIDHT = 200;
        private const double IMAGE_SIZE = 10;
        private const double SIZE_ROW = 70;
        private const int HEIGHT = 10;
        private const int WIDHT = 10;
        private BrushConverter conv = new BrushConverter();
        private Island island;

        private Canvas grid;
        private ScrollViewer viewbox;
	    private DockPanel panel;

		private int step = 0;
		private int rubbits = 0;
		private int hunters = 0;
		private int wolfs = 0;
		private bool end = false;
		public IList<Island> states = new List<Island>();

		public MainWindow()
		{
			InitializeComponent();
            island = new Island(HEIGHT, WIDHT);
            panel =  this.FindName(PANEL) as DockPanel;
		    viewbox = panel.FindName(VIEW) as ScrollViewer;
            grid = panel.FindName(GRID) as Canvas;
            GenerateRows();
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
                    grid.Children.Add(cloneGroupBox);
					
                    groupBoxes[i, j] = cloneGroupBox;
                    cloneGroupBox.MouseRightButtonDown += new MouseButtonEventHandler(Grid_OnMouseRightButtonDown);


                    UpdateCell(island.Cells[i, j], cloneGroupBox);
                }

                viewbox.Width = SIZE_ROW * WIDHT;
                this.Width = SIZE_ROW * WIDHT + PANEL_WIDHT;
            }
            groupBox = groupBoxes;
        }

        private void NextSteps(int count)
        {
            for (int i = 0; i != count; ++i)
            {
                island.NextBeat();
                AddState(island.Clone());
            }

            step += count;
        }

	    private void UpdateField()
	    {
	        rubbits = 0;
	        hunters = 0;
	        wolfs = 0;

            for (int i = 0; i != HEIGHT; ++i)
	        {
	            for (int j = 0; j != WIDHT; ++j)
	            {
	                UpdateCell(island.Cells[i, j], groupBox[i, j]);

	                rubbits += island.Cells[i, j].GetRubbits();
	                wolfs += island.Cells[i, j].GetWolfs();
	                hunters += island.Cells[i, j].GetHunters();
	            }
	        }

	        var textBlock = panel.FindName("Statistics") as TextBlock;
	        textBlock.Text = $"Step : {step}{Environment.NewLine}Rubbits : {rubbits}{Environment.NewLine}Hunters : {hunters}{Environment.NewLine}Wolfs : {wolfs}{Environment.NewLine}";
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
            NextSteps(1);
            UpdateField();
        }



		public void AddState(Island state)
		{
			states.Add(state);
		}

		public Island GetState(int i)
		{
			if (i < 0 || i > states.Count - 1)
			{
				return null;
			}

			return states[i];
		}

		public int GetIndex(Island island)
		{
			return states.IndexOf(island);
		}

	    private void Grid_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	    {
	        
	    }
	}
}
