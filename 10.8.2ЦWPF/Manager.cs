using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10._8._2ЦWPF
{
	interface IManager : IConsultant
	{
		new void ChangeInfoClient(List<Client> clients, Client selectedClient, string filePath, string worker, string newPhoneNumber = null,
			string newFamilyName = null, string newFirstName = null, string newLastName = null, string newSerialNumberDoc = null);

	}
	internal class Manager : Consultant, IManager
	{

		public override void ChangeInfoClient(List<Client> clients, Client selectedClient, string filePath, string worker, string newPhoneNumber = null,
			string newFamilyName = null, string newFirstName = null, string newLastName = null, string newSerialNumberDoc = null)
		{
			base.ChangeInfoClient(clients, selectedClient, filePath, worker, newPhoneNumber, newFamilyName, newFirstName, newLastName, newSerialNumberDoc);

			if (selectedClient != null)
			{
				var infoToUpdate = new Dictionary<Action<string>, Tuple<string, string>>
				{
					{ x => selectedClient.FamilyName = x, new Tuple<string, string>(newFamilyName, "Фамилия") },
					{ x => selectedClient.FirstName = x, new Tuple<string, string>(newFirstName, "Имя") },
					{ x => selectedClient.LastName = x, new Tuple<string, string>(newLastName, "Отчество") },
					{ x => selectedClient.SerialNumberDoc = x, new Tuple<string, string>(newSerialNumberDoc, "Серия номер паспорта") }
				};

				foreach (var info in infoToUpdate)
				{
					if (!string.IsNullOrEmpty(info.Value.Item1))
					{
						info.Key(info.Value.Item1);
						selectedClient.LastModified = DateTime.Now.ToString();
						selectedClient.ModifiedData = info.Value.Item2;
						selectedClient.ChangeType = "Изменено";
						selectedClient.ModifiedBy = worker;
					}
				}
			}
			SaveClients(clients, filePath);
		}
		
	}
}
