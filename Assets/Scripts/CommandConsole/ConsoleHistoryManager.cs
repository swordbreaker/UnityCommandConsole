using System.Collections.Generic;

namespace CommandConsole
{
    public class ConsoleHistoryManager
    {
        private readonly LinkedList<string> _history = new LinkedList<string>();
        private const int HistoryLenght = 20;

        private LinkedListNode<string> _pointer;

        public void AddToHistory(string s)
        {
            if (_pointer != null)
            {
                _history.Remove(_pointer);
                _history.AddFirst(_pointer);
            }
            _history.AddFirst(s);
            while (_history.Count > HistoryLenght)
            {
                _history.RemoveLast();
            }
            _pointer = _history.First;
        }

        public string Up()
        {
            if (_history.Count == 0) return "";
            var oldPointer = _pointer;
            if (_pointer != _history.Last)
            {
                _pointer = _pointer.Next;
            }

            return oldPointer.Value;
        }

        public string Down()
        {
            if (_history.Count == 0) return "";
            if (_pointer != _history.First)
            {
                _pointer = _pointer.Previous;
            }

            return _pointer != null ? _pointer.Value : "";
        }
    }
}
