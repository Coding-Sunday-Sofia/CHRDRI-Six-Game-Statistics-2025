using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Collections;

namespace Game_Six;

public partial class MainWindow : Window {
	private Button CreateHexButton(int row, int col, double width, double height) {
		var polygon = new Polygon {
			Points = new[]
			{
				new Point(width/2,0),
				new Point(width, height/4),
				new Point(width, 3*height/4),
				new Point(width/2, height),
				new Point(0, 3*height/4),
				new Point(0, height/4)
			},
			Fill = Brushes.White,
			Stroke = Brushes.Black,
			StrokeThickness = 1
		};

		var button = new Button {
			Width = width,
			Height = height,
			Background = Brushes.Transparent,
			Padding = new Avalonia.Thickness(0),
			BorderThickness = new Avalonia.Thickness(0),
			Content = polygon
		};

		button.Click += (sender, e) => {
			Console.WriteLine($"Button clicked at row {row}, column {col}");
		};

		return button;
	}

	private int turn = 0;
	private string gguid = "";
	private char[][] board = {};

	private static async void send(HttpClient client, string url, StringContent content) {
		try{await client.PostAsync(url, content);}catch(Exception ex){}
	}

	private void onMainWindowPointerPressed(object? sender, PointerPressedEventArgs e) {
		HttpClient client = new HttpClient();

		string url = "http://localhost:8080/index.php";

		string state = string.Join(Environment.NewLine,board.Select(row => new string(row)));

		string json = "{ \"gguid\": \""+gguid+"\", \"turn\": "+turn+", \"board\": "+JsonSerializer.Serialize(state)+"}";

		StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

		send(client, url, content);
	}

	private void reset() {
		turn = 0;
		board = new char[42][];
		for(int i=0; i<board.Length; i++) {
			board[i] = new char[42];

			for(int j=0; j<board[i].Length; j++) {
				board[i][j] = 'E';
			}
		}
		board[21][21] = 'B';
		board[21][22] = 'R';
		gguid = Guid.NewGuid().ToString();
	}

	private void show() {
		for(int i=0; i<board.Length; i++) {
			for(int j=0; j<board[i].Length; j++) {
				switch(board[i][j]) {
					case 'E': ((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.Yellow); break;
					case 'B': ((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.Blue); break;
					case 'R': ((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.Red); break;
					default: ((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.White); break;
				}
			}
		}
	}

	private int rows = 42;
	private int cols = 42;
	private Button[,] hexButtons;

	public MainWindow() {
		InitializeComponent();

		Width = 1260;
		Height = 1260;

		var canvas = new Canvas();
		Content = canvas;

		hexButtons = new Button[rows, cols];

		double hexHeight = 30;
		double hexWidth = Math.Sqrt(3) / 2 * hexHeight;

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				var button = CreateHexButton(row, col, hexWidth, hexHeight);

				// Flat-topped hexagon placement
				double x = col * hexWidth;
				double y = row * hexHeight * 0.45 * Math.Sqrt(3);
				if (row % 2 == 1) {
					x += hexWidth * 0.5;
				}

				Canvas.SetLeft(button, x);
				Canvas.SetTop(button, y);

				canvas.Children.Add(button);
				hexButtons[row, col] = button;
			}
		}
		reset();
		show();
	}
}

