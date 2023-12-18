using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _10._8._1WPF
{
	public interface IConsultant
	{
		void ChangeInfoClient(List<Client> clients, Client selectedClient, string filePath, string newPhoneNumber);
		List<Client> ViewClients(List<Client> clients);
	}
	public class Consultant : IConsultant
	{
		public virtual List<Client> ViewClients(List<Client> clients)
		{
			return clients.Select(client => new Client(client.Id, client.FamilyName, client.FirstName, client.LastName, client.PhoneNumber, "**** ******")).ToList();
		}

		public void SaveClients(List<Client> clients, string filePath)
		{
			var lines = new List<string>();
			foreach (var client in clients)
			{
				var line = $"{client.Id};{client.FamilyName};{client.FirstName};{client.LastName};{client.PhoneNumber};{client.SerialNumberDoc};";
				lines.Add(line);
			}
			File.WriteAllLines(filePath, lines);
		}

		public virtual void ChangeInfoClient(List<Client> clients, Client selectedClient, string filePath, string newPhoneNumber)
		{

			if (selectedClient != null)
			{
				if (string.IsNullOrEmpty(newPhoneNumber))
				{
					MessageBox.Show("Номер телефона не может быть пустым");
					return;
				}
				selectedClient.PhoneNumber = newPhoneNumber;

			}

			SaveClients(clients, filePath);
		}

	}
}
