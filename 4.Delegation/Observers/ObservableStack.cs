using System;
using System.Collections.Generic;
using System.Text;

namespace Delegates.Observers
{	
	public class StackOperationsLogger
	{
		private readonly StringBuilder _log = new StringBuilder();

		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.HandleEvent += (sender, eventData) => _log.Append(eventData);
		}

		public string GetLog() => _log.ToString();
	}

	public class ObservableStack<T>
	{
		public EventHandler<object> HandleEvent;
		
		private readonly List<T> _data = new List<T>();

		public void Push(T obj)
		{
			_data.Add(obj);
			HandleEvent?.Invoke(this, new StackEventData<T> {IsPushed = true, Value = obj});
		}

		public T Pop()
		{
			if (_data.Count == 0)
				throw new InvalidOperationException();
			var result = _data[_data.Count - 1];
			HandleEvent?.Invoke(this, new StackEventData<T> {IsPushed = false, Value = result});
			return result;
		}
	}
}
