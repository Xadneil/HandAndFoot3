import { CardType } from "types/CardType";
import { Rank, Suit } from "types/Enums";
import { serverUrl } from "./network";
const imageURL = serverUrl + 'images/';

export const getImageUrl = (card: CardType) => {
  if (card.rank === Rank.JOKER)
    return `${imageURL}red_joker.svg`;
  return `${imageURL}${rankToName(card.rank)}_of_${Suit[card.suit].toLowerCase()}.svg`;
};

const rankToName = (rank: Rank) => {
  if (rank >= Rank.TWO && rank <= Rank.TEN)
    return rank.toString();
  return Rank[rank].toLowerCase();
};