using System;
using System.Linq;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        private Guid Guid { get; }

        public Enterprise (Guid guid) =>
            Guid = guid;

        public string Name { get; set; }

        public string Inn
        {
            get => _inn;
            set
            {
                if (_inn.Length != 10 || !_inn.All(char.IsDigit))
                    throw new ArgumentException();
                _inn = value;
            }
        }

        private string _inn;
        
        public DateTime EstablishDate { get; set; }

        public TimeSpan GetActiveTimeSpan() => DateTime.Now - EstablishDate;

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            return DataBase.Transactions()
                .Where(transaction => transaction.EnterpriseGuid == Guid)
                .Sum(transaction => transaction.Amount);
        }
    }
}
