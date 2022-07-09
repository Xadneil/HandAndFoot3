using System.Collections.Generic;
using System.Linq;

namespace HAF.WebServer.Game
{
    public class CardHolder
    {
        public List<Card> Hand { get; private set; }
        public List<Card> Foot { get; private set; }

        public CardHolder()
        {
        }

        public List<Card> CurrentHand
        {
            get
            {
                return Hand == null || (!Hand.Any() && Foot.Any()) ? Foot : Hand;
            }
        }

        public bool IsInFoot
        {
            get
            {
                return Hand == null || (!Hand.Any() && Foot.Any());
            }
        }

        public void DealCards(List<Card> hand, List<Card> foot)
        {
            Hand = hand;
            Foot = foot;
        }
    }
}
