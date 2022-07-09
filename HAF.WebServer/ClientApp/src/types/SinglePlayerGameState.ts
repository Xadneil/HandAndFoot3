import { CardType } from "./CardType";

export type SinglePlayerGameState = {
  cardHolder: {
    hand: CardType[];
    foot: CardType[];
    currentHand: CardType[];
    isInFoot: boolean;
  };
  myTeamTable: CardType[][];
  theirTeamTable: CardType[][];
  discardPileSize: number;
  drawPileSize: number;
  round: number;
  discard?: CardType;
};