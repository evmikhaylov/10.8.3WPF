using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10._8._1WPF
{
	public class Client
	{
		public int Id { get; set; }
		public string FamilyName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public virtual string SerialNumberDoc { get; set; }

		public Client(int Id, string FamilyName, string FirstName, string LastName, string PhoneNumber, string SerialNumberDoc)
		{
			this.Id = Id;
			this.FamilyName = FamilyName;
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.PhoneNumber = PhoneNumber;
			this.SerialNumberDoc = SerialNumberDoc;
			
		}

		public static List<Client> GetClients(string filePath)
		{
			var clients = new List<Client>();
			var lines = File.ReadAllLines(filePath);
			foreach (var line in lines)
			{
				var parts = line.Split(';');
				if (parts.Length >= 6)
				{
					var client = new Client(int.Parse(parts[0]), parts[1], parts[2], parts[3], parts[4], parts[5]);
					clients.Add(client);
				}
			}
			return clients;
		}
	}

}
