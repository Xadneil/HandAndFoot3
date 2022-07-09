import './Card.css'
import React from "react";
import { CardType } from "types/CardType";
import { Rank, Suit } from "types/Enums";

interface CardProps {
  card: CardType;
}

const Card: React.FC<CardProps> = ({ card }) => {
  return <div className='mycard'>
    <p>{Suit[card.suit]}</p>
    <p>{Rank[card.rank]}</p>
  </div>
};

export default Card;