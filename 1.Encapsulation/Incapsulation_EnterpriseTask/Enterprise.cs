using System;
using System.Linq;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        public Guid Guid { get; }

        public Enterprise (Guid guid) =>
            Guid = guid;

        public string Name { get; set; }

        private string _inn;
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
        
        public DateTime EstablishDate { get; set; }

        public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            return DataBase.Transactions()
                .Where(transaction => transaction.EnterpriseGuid == Guid)
                .Sum(transaction => transaction.Amount);
        }
    }
}
