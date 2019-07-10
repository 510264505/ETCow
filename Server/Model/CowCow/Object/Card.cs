using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public partial class Card : IEquatable<Card>
    {
        public bool Equals(Card other)
        {
            return this.CardWeight == other.CardWeight && this.CardSuits == other.CardSuits;
        }
        public string GetName()
        {
            return this.CardSuits == Suits.None ? this.CardWeight.ToString() : $"{this.CardSuits.ToString()}{this.CardWeight.ToString()}";
        }
    }
}
