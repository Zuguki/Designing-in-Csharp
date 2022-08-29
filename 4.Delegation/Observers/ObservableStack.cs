using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		private readonly Observer _observer = new Observer();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(_observer);
		}

		public string GetLog() => _observer.Log.ToString();
	}

	public interface IObserver
	{
		void HandleEvent(object eventData);
	}

	public class Observer : IObserver
	{
		public readonly StringBuilder Log = new StringBuilder();

		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}
	}

	public interface IObservable
	{
		void Add(IObserver observer);
		void Remove(IObserver observer);
		void Notify(object eventData);
	}

	public class ObservableStack<T> : IObservable
	{
		private readonly List<IObserver> _observers = new List<IObserver>();
		private readonly List<T> _data = new List<T>();

		public void Add(IObserver observer)
		{
			_observers.Add(observer);
		}

		public void Notify(object eventData)
		{
			foreach (var observer in _observers)
				observer.HandleEvent(eventData);
		}

		public void Remove(IObserver observer)
		{
			_observers.Remove(observer);
		}

		public void Push(T obj)
		{
			_data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (_data.Count == 0)
				throw new InvalidOperationException();
			var result = _data[_data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;
		}
	}
}
