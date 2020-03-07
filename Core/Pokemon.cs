using System;

namespace Shakespokemon.Core
{
    public class Pokemon
    {
        public Pokemon(string name, string description) : this(name, description, DateTime.UtcNow)
        {
        }

        internal Pokemon(string name, string description, DateTime utcCreationDate)
        {
            Argument.IsNotNullOrWhitespace(name, nameof(name));
            Argument.IsNotNullOrWhitespace(description, nameof(description));
            Argument.IsValid(utcCreationDate != default, $"{nameof(utcCreationDate)} is not initialized.");

            Name = name.Trim().ToLower();
            Description = description;
            UtcCreationDate = utcCreationDate;
        }

        public string Name { get; }

        public string Description { get; }

        public DateTime UtcCreationDate { get; }

        public override bool Equals(object obj)
        {
            var item = obj as Pokemon;
            if (item == null)
            {
                return false;
            }

            return Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
