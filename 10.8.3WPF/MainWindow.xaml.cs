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

namespace _10._8._3WPF
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
		

		public MainWindow()
		{
			InitializeComponent(); 
			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{ fs.Close(); }
			clients = Client.GetClients(filePath);
		}
		private void WorkerChangeRole_SelectionChanged (object sender, SelectionChangedEventArgs e)
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
						worker = new Consultant();
						break;
					}
				case "Менеджер":
					{
						if (selectedClient == null) 
						{
							AddButton.IsEnabled = true;
						}
						foreach (var textBox in managerTextBoxes)
						{
							textBox.IsEnabled = true;
						}
						worker = new Manager();
						break;
					}
					
			}
			ClientsDataGrid.ItemsSource = worker.ViewClients(clients, selectedRole);
		}
		private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ClientsDataGrid.SelectedItem is ClientView selectedClientView)
			{
				selectedClient = clients.FirstOrDefault(c => c.Id == selectedClientView.Id);
			}
			else
			{
				selectedClient = null;
			}
		}
		private void GetTextBoxValues(out string newPhoneNumber, out string newFamilyName, out string newFirstName, out string newLastName, out string newSerialNumberDoc)
		{
			newPhoneNumber = NewPhoneNumberTextBox.Foreground == Brushes.Gray ? null : NewPhoneNumberTextBox.Text;
			newFamilyName = NewFamilyNameTextBox.Foreground == Brushes.Gray ? null : NewFamilyNameTextBox.Text;
			newLastName = NewLastNameTextBox.Foreground == Brushes.Gray ? null : NewLastNameTextBox.Text;
			newFirstName = NewFirstNameTextBox.Foreground == Brushes.Gray ? null : NewFirstNameTextBox.Text;
			newSerialNumberDoc = NewSerialDocTextBox.Foreground == Brushes.Gray & NewNumberDocTextBox.Foreground == Brushes.Gray ? null : NewSerialDocTextBox.Text + " " + NewNumberDocTextBox.Text;
		}
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			GetTextBoxValues(out string newPhoneNumber, out string newFamilyName, out string newFirstName, out string newLastName, out string newSerialNumberDoc);
			
			worker.ChangeInfoClient(clients, selectedClient, filePath, selectedRole, newPhoneNumber,
				newFamilyName, newFirstName, newLastName, newSerialNumberDoc);
			ClientsDataGrid.ItemsSource = worker.ViewClients(clients, selectedRole);

		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			GetTextBoxValues(out string newPhoneNumber, out string newFamilyName, out string newFirstName, out string newLastName, out string newSerialNumberDoc);
			worker.AddClient(clients, filePath, selectedRole, newPhoneNumber,
				newFamilyName, newFirstName, newLastName, newSerialNumberDoc);
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
