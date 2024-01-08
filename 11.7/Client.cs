using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11._7
{
	public class Client
	{
		public int Id { get; set; }
		public string FamilyName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public virtual string SerialNumberDoc { get; set; }
		public string LastModified { get; set; }
		public string ModifiedData { get; set; }
		public string ChangeType { get; set; }
		public string ModifiedBy { get; set; }

		public Client(int Id, string FamilyName, string FirstName, string LastName, string PhoneNumber, string SerialNumberDoc, string LastModified, string ModifiedData,
			string ChangeType, string ModifiedBy)
		{
			this.Id = Id;
			this.FamilyName = FamilyName;
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.PhoneNumber = PhoneNumber;
			this.SerialNumberDoc = SerialNumberDoc;
			this.LastModified = LastModified;
			this.ModifiedData = ModifiedData;
			this.ChangeType = ChangeType;
			this.ModifiedBy = ModifiedBy;
		}





		public static List<Client> GetClients(string filePath)
		{
			var clients = new List<Client>();
			var lines = File.ReadAllLines(filePath);
			foreach (var line in lines)
			{
				var parts = line.Split(';');
				if (parts.Length >= 10)
				{
					var client = new Client(int.Parse(parts[0]), parts[1], parts[2], parts[3], parts[4], parts[5],
					parts[6], parts[7], parts[8], parts[9]);
					clients.Add(client);
				}
			}
			return clients;
		}

		public Client GetClientInfo(string worker)
		{
			switch (worker)
			{
				case "Консультант":
					return new Client(Id, FamilyName, FirstName, LastName, PhoneNumber, "**** ******", LastModified, ModifiedData, ChangeType, ModifiedBy);
				case "Менеджер":
					return new Client(Id, FamilyName, FirstName, LastName, PhoneNumber, SerialNumberDoc, LastModified, ModifiedData, ChangeType, ModifiedBy);
				default: throw new ArgumentException("Роль не распознана");
			}
		}
	}

	public class ClientView
	{
		public int Id { get; set; }
		public string FamilyName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string SerialNumberDoc { get; set; }

		public ClientView(Client client, string worker)
		{
			Id = client.Id;
			FamilyName = client.FamilyName;
			FirstName = client.FirstName;
			LastName = client.LastName;
			PhoneNumber = client.PhoneNumber;
			SerialNumberDoc = worker == "Консультант" ? "**** ******" : client.SerialNumberDoc;
		}
	}
}
