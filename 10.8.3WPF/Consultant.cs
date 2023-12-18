using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _10._8._3WPF
{
	public interface IConsultant
	{
		void ChangeInfoClient(List<Client> clients, Client selectedClient, string filePath, string worker, string newPhoneNumber = null,
			string newFamilyName = null, string newFirstName = null, string newLastName = null, string newSerialNumberDoc = null);
		List<ClientView> ViewClients(List<Client> clients, string worker);
		void AddClient(List<Client> clients, string filePath, string worker, string newPhoneNumber = null,
			string newFamilyName = null, string newFirstName = null, string newLastName = null, string newSerialNumberDoc = null);
	}
	public class Consultant : IConsultant
	{
		public virtual List<ClientView> ViewClients(List<Client> clients, string worker)
		{
			var result = new List<ClientView>();
			foreach(var client in clients)
			{
				result.Add(new ClientView(client, worker));
			}
			return result;
		}

		public void SaveClients(List<Client> clients, string filePath)
		{
			var lines = new List<string>();
			foreach (var client in clients)
			{
				var line = $"{client.Id};{client.FamilyName};{client.FirstName};{client.LastName};{client.PhoneNumber};{client.SerialNumberDoc};{client.LastModified};" +
					$"{client.ModifiedData};{client.ChangeType};{client.ModifiedBy};";
				lines.Add(line);
			}
			File.WriteAllLines(filePath, lines);
		}

		public virtual void ChangeInfoClient(List<Client> clients, Client selectedClient, string filePath, string worker, string newPhoneNumber = null,
			string newFamilyName = null, string newFirstName = null, string newLastName = null, string newSerialNumberDoc = null)
		{

			if (selectedClient != null)
			{
				if (string.IsNullOrEmpty(newPhoneNumber))
				{
					MessageBox.Show("Номер телефона не может быть пустым");
					return;
				}
				selectedClient.PhoneNumber = newPhoneNumber;
				selectedClient.LastModified = DateTime.Now.ToString();
				selectedClient.ModifiedData = "Номер телефона";
				selectedClient.ChangeType = "Изменено";
				selectedClient.ModifiedBy = worker;

			}
			
			SaveClients(clients, filePath);
		}
		public virtual void AddClient(List<Client> clients, string filePath, string worker, string newPhoneNumber = null,
			string newFamilyName = null, string newFirstName = null, string newLastName = null, string newSerialNumberDoc = null)
		{ }
	}
}
