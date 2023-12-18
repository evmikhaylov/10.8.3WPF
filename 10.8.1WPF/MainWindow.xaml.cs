using System;
using System.Collections.Generic;
using System.IO;
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

namespace _10._8._1WPF
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<Client> clients;
		private string filePath = "ClientsCons.txt";
		private Client selectedClient;
		private IConsultant consultant = new Consultant();

		public MainWindow()
		{
			InitializeComponent();
			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{ fs.Close(); }
			clients = Client.GetClients(filePath);
			ClientsDataGrid.ItemsSource = consultant.ViewClients(clients);
		}
		
		private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ClientsDataGrid.SelectedItem is Client selectedClientView)
			{
				selectedClient = clients.FirstOrDefault(c => c.Id == selectedClientView.Id);
			}
			else
			{
				selectedClient = null;
			}
		}
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			consultant.ChangeInfoClient(clients, selectedClient, filePath, NewPhoneNumberTextBox.Text);
			ClientsDataGrid.ItemsSource = consultant.ViewClients(clients);

		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (textBox.Foreground == Brushes.Gray)
			{
				textBox.Tag = textBox.Text;
				textBox.Text = "";
				textBox.Foreground = Brushes.Black;
			}
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (string.IsNullOrWhiteSpace(textBox.Text))
			{
				textBox.Text = textBox.Tag.ToString();
				textBox.Foreground = Brushes.Gray;
			}
		}


	}

}
