using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Game_Six;

public partial class MainWindow : Window {
	private int turn = 0;
	private string gguid = "";
	private char[][] board = null;

	private static async void send(HttpClient client, string url, StringContent content) {
		await client.PostAsync(url, content);
	}

	private void onMainWindowPointerPressed(object? sender, PointerPressedEventArgs e) {
		turn++;
	
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
		gguid = Guid.NewGuid().ToString();
	}

	public MainWindow() {
		InitializeComponent();
		reset();
	}
}
