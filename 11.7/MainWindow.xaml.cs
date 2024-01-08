using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace _11._7
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private List<Client> clients;
		private string filePath = "Clients.txt";
		private Client selectedClient;
		private IConsultant worker;
		private string selectedRole;
		private List<Client> originalClients;
		

		public MainWindow()
		{
			InitializeComponent();
			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{ fs.Close(); }
			originalClients = Client.GetClients(filePath);
			clients = new List<Client>(originalClients);
		}
		private void WorkerChangeRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedRole = ((ComboBoxItem)WorkerChangedRole.SelectedItem).Content.ToString();
			NewPhoneNumberTextBox.IsEnabled = true;
			TextBox[] managerTextBoxes = { NewFamilyNameTextBox, NewFirstNameTextBox,
				NewLastNameTextBox, NewSerialDocTextBox, NewNumberDocTextBox };

			foreach (var textBox in managerTextBoxes)
			{
				textBox.IsEnabled = false;
			}

			switch (selectedRole)
			{
				case "Консультант":
					{
						AddButton.IsEnabled = false;
						DeleteClientButton.IsEnabled = false;
						worker = new Consultant();
						break;
					}
				case "Менеджер":
					{
						if (selectedClient == null)
						{
							AddButton.IsEnabled = true;
							DeleteClientButton.IsEnabled = true;
						}
						foreach (var textBox in managerTextBoxes)
						{
							textBox.IsEnabled = true;
						}
						worker = new Manager();
						break;
					}

			}
			clients = new List<Client>(originalClients);
			ClientsDataGrid.ItemsSource = worker.ViewClients(clients.ToList(), selectedRole); ;
		}
		private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ClientsDataGrid.SelectedItem is ClientView selectedClientView)
			{
				selectedClient = clients.FirstOrDefault(c => c.Id == selectedClientView.Id);
				if (selectedRole == "Менеджер")
				{
					DeleteClientButton.IsEnabled = true;
				}
			}
			else
			{
				selectedClient = null;
				DeleteClientButton.IsEnabled = false;

			}
		}
		private void GetTextBoxValues(out string newPhoneNumber, out string newFamilyName, out string newFirstName, out string newLastName, out string newSerialNumberDoc)
		{
			newPhoneNumber = NewPhoneNumberTextBox.Foreground == Brushes.Gray ? null : NewPhoneNumberTextBox.Text;
			newFamilyName = NewFamilyNameTextBox.Foreground == Brushes.Gray ? null : NewFamilyNameTextBox.Text;
			newLastName = NewLastNameTextBox.Foreground == Brushes.Gray ? null : NewLastNameTextBox.Text;
			newFirstName = NewFirstNameTextBox.Foreground == Brushes.Gray ? null : NewFirstNameTextBox.Text;
			newSerialNumberDoc = NewSerialDocTextBox.Foreground != Brushes.Gray && NewNumberDocTextBox.Foreground != Brushes.Gray ? NewSerialDocTextBox.Text + " " + NewNumberDocTextBox.Text : null;
		}
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			GetTextBoxValues(out string newPhoneNumber, out string newFamilyName, out string newFirstName, out string newLastName, out string newSerialNumberDoc);

			worker.ChangeInfoClient(originalClients, selectedClient, filePath, selectedRole, newPhoneNumber,
				newFamilyName, newFirstName, newLastName, newSerialNumberDoc);
			clients = new List<Client>(originalClients);
			ClientsDataGrid.ItemsSource = worker.ViewClients(clients, selectedRole);

		}
		

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			GetTextBoxValues(out string newPhoneNumber, out string newFamilyName, out string newFirstName, out string newLastName, out string newSerialNumberDoc);
			worker.AddClient(originalClients, filePath, selectedRole, newPhoneNumber,
				newFamilyName, newFirstName, newLastName, newSerialNumberDoc);
			clients = new List<Client>(originalClients);
			ClientsDataGrid.ItemsSource = worker.ViewClients(clients, selectedRole);
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

		private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
		{
			if (worker is Manager manager)
			{
				manager.DeleteClient(originalClients, selectedClient, filePath);
				clients = new List<Client>(originalClients);
				ClientsDataGrid.ItemsSource = worker.ViewClients(clients, selectedRole);
			}
		}

		private void SortByFirstNameButton_Click(object sender, RoutedEventArgs e)
		{
			var sortedClients = new List<Client>(clients).OrderBy(c => c.FirstName).ToList();
			ClientsDataGrid.ItemsSource = worker.ViewClients(sortedClients, selectedRole);
		}

		private void SortByFamilyNameButton_Click(object sender, RoutedEventArgs e)
		{
			var sortedClients = new List<Client>(clients).OrderBy(c => c.LastName).ToList();
			ClientsDataGrid.ItemsSource = worker.ViewClients(sortedClients, selectedRole);
		}

		private void SortByLastNameButton_Click(object sender, RoutedEventArgs e)
		{
			var sortedClients = new List<Client>(clients).OrderBy(c => c.FamilyName).ToList();
			ClientsDataGrid.ItemsSource = worker.ViewClients(sortedClients, selectedRole);
		}
	}
}
