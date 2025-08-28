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
			for(int i=0; i<board.Length; i++) {
				for(int j=0; j<board[i].Length; j++) {
					if(sender == hexButtons[i,j]) {
						if(gameOver == true) {
							return;
						}

						if(turn >= 40) {
							Title = "No more moves!";
							return;
						} else {
							//TODO Implement move pices logic.
						}

						if(board[i][j] != 'E') {
							Title = "The tile is occupied!";
							return;
						}

						if(hasNeighbors(i,j) == false) {
							Title = "The tile shoud be next to the others!";
							return;
						}

						if(turn == 0 && hasSameNeighbor(i,j,playing)) {
							Title = "In first turn avoid your color!";
							return;
						}

						turn++;
						playing = (turn%2==0)?'B':'R';
						board[i][j] = playing;

						if(isGameOver() == true) {
							Title = "Game over!";
							gameOver = true;
						}

						break;
					}
				}
			}
			show();
			send();
		};

		return button;
	}

	private int turn = 0;
	private string gguid = "";
	private char[][] board = {};
	private char playing = '\0';
	private bool gameOver = true;

	private static async void send(HttpClient client, string url, StringContent content) {
		try {
			await client.PostAsync(url, content);
		} catch(Exception ex) {}
	}

	private bool isNeighbor(int x, int y, char tile) {
		if(x < 0) {
			return false;
		}
		if(y < 0) {
			return false;
		}
		if(x >= board.Length) {
			return false;
		}
		if(y >= board[x].Length) {
			return false;
		}

		if(board[x][y] == tile) {
			return true;
		}

		return false;
	}

	private bool hasCircle(char tile) {
		for(int x=0; x<board.Length; x++) {
			for(int y=0; y<board[x].Length; y++) {
				int six = 0;
				
				if(x%2 != 0) {
					six += isNeighbor(x-1, y, tile) ? 1 : 0;
					six += isNeighbor(x-1, y+1, tile) ? 1 : 0;
					six += isNeighbor(x, y+1, tile) ? 1 : 0;
					six += isNeighbor(x+1, y+1, tile) ? 1 : 0;
					six += isNeighbor(x+1, y, tile) ? 1 : 0;
					six += isNeighbor(x, y-1, tile) ? 1 : 0;
					if(six == 6) {
						return true;
					}
				} else {
					six += isNeighbor(x-1, y-1, tile) ? 1 : 0;
					six += isNeighbor(x-1, y, tile) ? 1 : 0;
					six += isNeighbor(x, y+1, tile) ? 1 : 0;
					six += isNeighbor(x+1, y, tile) ? 1 : 0;
					six += isNeighbor(x+1, y-1, tile) ? 1 : 0;
					six += isNeighbor(x, y-1, tile) ? 1 : 0;
					if(six == 6) {
						return true;
					}
				}
			}
		}

		return false;
	}

	private bool hasLine(char tile) {
		return false;
	}

	private bool hasTriangle(char tile) {
		return false;
	}

	private bool isGameOver() {
		if(hasCircle(playing)) {
			return true;
		}

		if(hasLine(playing)) {
			return true;
		}

		if(hasTriangle(playing)) {
			return true;
		}

		return false;
	}

	private bool hasSameNeighbor(int x, int y, char tile) {
		if(x%2 != 0) {
			if(isNeighbor(x-1, y, tile) == true) {
				return true;
			}
			if(isNeighbor(x-1, y+1, tile) == true) {
				return true;
			}
			if(isNeighbor(x, y+1, tile) == true) {
				return true;
			}
			if(isNeighbor(x+1, y+1, tile) == true) {
				return true;
			}
			if(isNeighbor(x+1, y, tile) == true) {
				return true;
			}
			if(isNeighbor(x, y-1, tile) == true) {
				return true;
			}
		} else {
			if(isNeighbor(x-1, y-1, tile) == true) {
				return true;
			}
			if(isNeighbor(x-1, y, tile) == true) {
				return true;
			}
			if(isNeighbor(x, y+1, tile) == true) {
				return true;
			}
			if(isNeighbor(x+1, y, tile) == true) {
				return true;
			}
			if(isNeighbor(x+1, y-1, tile) == true) {
				return true;
			}
			if(isNeighbor(x, y-1, tile) == true) {
				return true;
			}
		}
		return false;
	}

	private bool isNeighbor(int x, int y) {
		if(x < 0) {
			return false;
		}
		if(y < 0) {
			return false;
		}
		if(x >= board.Length) {
			return false;
		}
		if(y >= board[x].Length) {
			return false;
		}

		if(board[x][y] == 'R' || board[x][y] == 'B') {
			return true;
		}

		return false;
	}

	private bool hasNeighbors(int x, int y) {
		if(x%2 != 0) {
			if(isNeighbor(x-1, y) == true) {
				return true;
			}
			if(isNeighbor(x-1, y+1) == true) {
				return true;
			}
			if(isNeighbor(x, y+1) == true) {
				return true;
			}
			if(isNeighbor(x+1, y+1) == true) {
				return true;
			}
			if(isNeighbor(x+1, y) == true) {
				return true;
			}
			if(isNeighbor(x, y-1) == true) {
				return true;
			}
			/*
			((Polygon)hexButtons[x-1,y].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x-1,y+1].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x,y+1].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x+1,y+1].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x+1,y].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x,y-1].Content).Fill=new SolidColorBrush(Colors.Green);
			*/
		} else {
			if(isNeighbor(x-1, y-1) == true) {
				return true;
			}
			if(isNeighbor(x-1, y) == true) {
				return true;
			}
			if(isNeighbor(x, y+1) == true) {
				return true;
			}
			if(isNeighbor(x+1, y) == true) {
				return true;
			}
			if(isNeighbor(x+1, y-1) == true) {
				return true;
			}
			if(isNeighbor(x, y-1) == true) {
				return true;
			}
			/*
			((Polygon)hexButtons[x-1,y-1].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x-1,y].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x,y+1].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x+1,y].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x+1,y-1].Content).Fill=new SolidColorBrush(Colors.Green);
			((Polygon)hexButtons[x,y-1].Content).Fill=new SolidColorBrush(Colors.Green);
			*/
		}
		return false;
	}

	private void send() {
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
		playing = 'R';
		gameOver = false;
	}

	private void show() {
		for(int i=0; i<board.Length; i++) {
			for(int j=0; j<board[i].Length; j++) {
				switch(board[i][j]) {
				case 'E':
					((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.Yellow);
					break;
				case 'B':
					((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.Blue);
					break;
				case 'R':
					((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.Red);
					break;
				default:
					((Polygon)hexButtons[i,j].Content).Fill=new SolidColorBrush(Colors.White);
					break;
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

