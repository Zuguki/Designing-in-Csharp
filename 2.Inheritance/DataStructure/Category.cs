using System;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        private string Title { get; }
        private MessageType MessageType { get; }
        private MessageTopic MessageTopic { get; }

        public Category(string title, MessageType messageType, MessageTopic messageTopic)
        {
            Title = title;
            MessageType = messageType;
            MessageTopic = messageTopic;
        }

        public override bool Equals(object obj)
        {
            if (obj is Category category)
                return Equals(category);

            return false;
        }

        public static bool operator <=(Category first, Category second) => first.CompareTo(second) <= 0;
        public static bool operator >=(Category first, Category second) => first.CompareTo(second) >= 0;
        public static bool operator <(Category first, Category second) => first.CompareTo(second) < 0;
        public static bool operator >(Category first, Category second) => first.CompareTo(second) > 0;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Title != null ? Title.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (int) MessageType;
                hashCode = (hashCode * 397) ^ (int) MessageTopic;
                return hashCode;
            }
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category category))
                return 1;

            var stringCompare = string.Compare(Title, category.Title, StringComparison.InvariantCulture);
            var messageTypeCompare = MessageType.CompareTo(category.MessageType);
            var messageTopicCompare = MessageTopic.CompareTo(category.MessageTopic);

            if (stringCompare != 0)
                return stringCompare;
            if (messageTypeCompare != 0)
                return messageTypeCompare;

            return messageTopicCompare != 0 ? messageTopicCompare : 0;
        }

        public override string ToString() => $"{Title}.{MessageType}.{MessageTopic}";

        private bool Equals(Category other) =>
            Title == other.Title && MessageType == other.MessageType && MessageTopic == other.MessageTopic;
    }
}