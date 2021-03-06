﻿using Modeling.Common.Enums;
using Modeling.Modes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
	    private const string ADD_MENU = "AddMenu";
	    private const string COUNT = "Amount";
	    private const string GO = "Go";

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
	    private StackPanel addMenu;
	    private TextBox count;
	    private TextBox go;

	    private int currentStep = 0;
        private int step = 0;
		private int rubbits = 0;
		private int hunters = 0;
		private int wolfs = 0;

		private bool end = false;
	    private bool editable = true;
		public IList<Island> states = new List<Island>();

	    private ICell currentCell = null;
	    private Canvas currentCanvas = null;
		public MainWindow()
		{
			InitializeComponent();

            island = new Island(HEIGHT, WIDHT, false);
		    AddState(island.Clone());

            panel =  this.FindName(PANEL) as DockPanel;
		    addMenu = this.FindName(ADD_MENU) as StackPanel;
            viewbox = panel.FindName(VIEW) as ScrollViewer;
            grid = panel.FindName(GRID) as Canvas;
            count = this.FindName(COUNT) as TextBox;;
		    go = this.FindName(GO) as TextBox; ;

            GenerateRows();
            UpdateField(island);
        }


        public void GenerateRows()
        {
            var groupBoxes = new Canvas[HEIGHT, WIDHT];
            for (int i = 0; i != HEIGHT; ++i)
            {
                for (int j = 0; j != WIDHT; ++j)
                {

                    var border = new Border();
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = Brushes.Black;
                    border.Width = SIZE_ROW;
                    border.Height = SIZE_ROW;
                    border.Margin = new Thickness(i * SIZE_ROW, j * SIZE_ROW, 0, 0);

                    var cloneGroupBox = new Canvas();
                    cloneGroupBox.Width = SIZE_ROW;
                    cloneGroupBox.Height = SIZE_ROW; 
                    cloneGroupBox.Name = $"cell_{i}_{j}";
 
                    grid.Children.Add(border);

                    border.Child = cloneGroupBox;

                    groupBoxes[i, j] = cloneGroupBox;
                    cloneGroupBox.MouseRightButtonDown += new MouseButtonEventHandler(Grid_OnMouseRightButtonDown);
                    
                    UpdateCell(island.Cells[i, j], cloneGroupBox);
                }

                viewbox.Width = SIZE_ROW * WIDHT;
                this.Width = SIZE_ROW * WIDHT + PANEL_WIDHT;
            }
            groupBox = groupBoxes;
        }


	    private void RainColor(ICell cell, Canvas groupBox)
	    {

	        switch (cell.GetSun())
	        {
	            case 0:
	                groupBox.Background = conv.ConvertFromString("#e3f7f7") as SolidColorBrush;
	                break;
	            case 1:
	                groupBox.Background = conv.ConvertFromString("#b8f2f2") as SolidColorBrush;
	                break;
	            case 2:
	                groupBox.Background = conv.ConvertFromString("#8eeded") as SolidColorBrush;
	                break;
	            case 3:
	                groupBox.Background = conv.ConvertFromString("#3e9b9b") as SolidColorBrush;
	                break;
            }
	    }

	    private void SunColor(ICell cell, Canvas groupBox)
	    {

	        switch (cell.GetSun())
	        {
	            case 0:
	                groupBox.Background = conv.ConvertFromString("#fcfcf9") as SolidColorBrush;
	                break;
	            case 1:
	                groupBox.Background = conv.ConvertFromString("#fcfca4") as SolidColorBrush;
	                break;
	            case 2:
	                groupBox.Background = conv.ConvertFromString("#ffff84") as SolidColorBrush;
	                break;
	            case 3:
	                groupBox.Background = conv.ConvertFromString("#ffff3d") as SolidColorBrush;
	                break;
	        }
	    }

        private Canvas CreateSun(ICell cell)
	    {
	        var cloneGroupBox = new Canvas();
	        cloneGroupBox.Width = SIZE_ROW / 2;
	        cloneGroupBox.Height = SIZE_ROW / 2;
	        cloneGroupBox.Margin = new Thickness(0, 0, 0, 0);
	        SunColor(cell, cloneGroupBox);
	        return cloneGroupBox;
	    }

	    private Canvas CreateRain(ICell cell)
	    {
	        var cloneGroupBox = new Canvas();
	        cloneGroupBox.Width = SIZE_ROW / 2;
	        cloneGroupBox.Height = SIZE_ROW / 2;
	        cloneGroupBox.Margin = new Thickness(SIZE_ROW / 2, 0, 0, 0);
	        RainColor(cell, cloneGroupBox);
            return cloneGroupBox;
	    }

        private void NextSteps(int count)
        {
            for (int i = 0; i != count; ++i)
            {
                island.UpdateIsland();
                AddState(island.Clone());
            }

            step += count;
            currentStep = step;
            editable = true;
        }

	    private void UpdateField(Island islandState)
	    {
	        rubbits = 0;
	        hunters = 0;
	        wolfs = 0;

            for (int i = 0; i != HEIGHT; ++i)
            {
	            for (int j = 0; j != WIDHT; ++j)
	            {
	                UpdateCell(islandState.Cells[i, j], groupBox[i, j]);

	                rubbits += islandState.Cells[i, j].GetRubbits();
	                wolfs += islandState.Cells[i, j].GetWolfs();
	                hunters += islandState.Cells[i, j].GetHunters();
	            }
	        }
            UpdateStatistics();
        }

	    private void UpdateStatistics()
	    {
	        var textBlock = panel.FindName("Statistics") as TextBlock;
	        textBlock.Text = $"All steps : {step}{Environment.NewLine}Current step : {currentStep}{Environment.NewLine}Rubbits : {rubbits}{Environment.NewLine}Hunters : {hunters}{Environment.NewLine}Wolfs : {wolfs}{Environment.NewLine}";
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
            }
            else if (cell.GetLocality() == Locality.Hill)
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
            UpdatePopulation(cell, groupBox);
        }

	    private Image CreateImage(string source)
	    {
	        var image = new Image();
	        BitmapImage bitmap = new BitmapImage();
	        bitmap.BeginInit();
	        bitmap.UriSource = new Uri(source, UriKind.Relative);
	        bitmap.EndInit();
	        image.Stretch = Stretch.Fill;
	        image.Source = bitmap;
	        return image;
	    }

        private void UpdatePopulation(ICell cell, Canvas groupBox)
        {
            if (cell.GetLocality() == Locality.Field)
            {
                groupBox.Children.Clear();

                var sun = CreateSun(cell);
                groupBox.Children.Add(sun);

                var rain = CreateRain(cell);
                groupBox.Children.Add(rain);

                for (int i = 0; i != cell.GetRubbits(); ++i)
                {
                    var image = new Canvas();
                    image.Margin = new Thickness(i * IMAGE_SIZE * 2 + IMAGE_SIZE, IMAGE_SIZE, 0, 0);
                    image.Height = IMAGE_SIZE;
                    image.Width = IMAGE_SIZE;
                    image.Background = Brushes.Green;
                    groupBox.Children.Add(image);
                }
;

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

                //var text = new TextBlock();
                //text.Text = $"S:{cell.GetSun()}|R:{cell.GetRain()}|J:{cell.GetJuiciness()}";
                //groupBox.Children.Add(text);
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
            ClosePanel();
            if (int.TryParse(count.Text, out var amount))
            {
                NextSteps(amount);
                UpdateField(island);
            }        
        }

		public void AddState(Island state)
		{
			states.Add(state);
		}

	    public void UpdateState(Island state)
	    {
	        states[step] = state;
	    }

        public Island GetState(int i)
		{
			if (i < 0 || i > states.Count - 1)
			{
				return null;
			}

		    currentStep = i;

		    editable = step == currentStep;

			var state =  states[i];
		    return state;
		}

		public int GetIndex(Island island)
		{
			return states.IndexOf(island);
		}

	    private void Grid_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	    {
	        if (!editable)
	        {
                return;
	        }
	        currentCanvas = sender as Canvas;
	        var split = currentCanvas.Name.Split('_');

	        var i = int.Parse(currentCanvas.Name.Split('_')[1]); 
	        var j = int.Parse(currentCanvas.Name.Split('_')[2]);

	        currentCell = island.Cells[i, j];

            AddMenu.Visibility = Visibility.Visible;
	        var border = currentCanvas.Parent as Border;
	        var magin = border.Margin;
	        magin.Left += SIZE_ROW / 2;
	        magin.Top += SIZE_ROW / 2;
            AddMenu.Margin = magin;
	    }

	    private void Rubbits_Button(object sender, RoutedEventArgs e)
	    {
	        if (currentCell.AddOneRubbit())
	        {
	            ++rubbits;
	            UpdateCell(currentCell, currentCanvas);
	            UpdateStatistics();
                UpdateState(island.Clone());
            }
	        AddMenu.Visibility = Visibility.Hidden;
        }


        private void Hunters_Button(object sender, RoutedEventArgs e)
	    {
	        if (currentCell.AddOneHunter())
	        {
	            ++hunters;
	            UpdateCell(currentCell, currentCanvas);
	            UpdateStatistics();
	            UpdateState(island.Clone());
            }
	        AddMenu.Visibility = Visibility.Hidden;

        }

        private void Wolfs_Button(object sender, RoutedEventArgs e)
	    {
	        if (currentCell.AddOneWolf())
	        {
	            ++wolfs;
	            UpdateCell(currentCell, currentCanvas);
	            UpdateStatistics();
	            UpdateState(island.Clone());
            }
	        AddMenu.Visibility = Visibility.Hidden;
        }

	    private void GenerateRandomWorld(object sender, RoutedEventArgs e)
	    {
	        ClosePanel();
            grid.Children.Clear();
	        states.Clear();
	        island = new Island(HEIGHT, WIDHT, true);
	        AddState(island.Clone());
            step = 0;
	        currentStep = 0;
	        rubbits = 0;
	        wolfs = 0;
	        hunters = 0;
	        GenerateRows();
            UpdateField(island);
	    }


        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
	    {
	        Regex regex = new Regex("[^0-9]+");
	        e.Handled = regex.IsMatch(e.Text);
        }

	    private void Close_Panel(object sender, RoutedEventArgs e)
	    {
	        ClosePanel();
        }

	    private void ClosePanel()
	    {
	        AddMenu.Visibility = Visibility.Hidden;
        }

	    private void Button_Go(object sender, RoutedEventArgs e)
	    {
	        ClosePanel();

            if (int.TryParse(go.Text, out var amount))
	        {
	            if (step >= amount)
	            {
	                UpdateField(GetState(amount));
	            }
	        }
        }

	    private void  Button_Next(object sender, RoutedEventArgs e)
	    {
	        ClosePanel();
            var st = GetState(currentStep + 1);
	        if (st != null)
	        {
	            UpdateField(st);
	        }
        }

	    private void Button_Back(object sender, RoutedEventArgs e)
	    {
	        ClosePanel();
            var st = GetState(currentStep - 1);
	        if (st != null)
	        {
	            UpdateField(st);
	        }
            
        }
	}
}
