using System.Linq;

namespace HAF.WebServer.Game
{
    public class CardHolder
    {
        public Hand Hand { get; private set; }
        public Hand Foot { get; private set; }

        public CardHolder()
        {
        }

        public Hand CurrentHand
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

        public void DealCards(Hand hand, Hand foot)
        {
            Hand = hand;
            Foot = foot;
        }
    }
}
