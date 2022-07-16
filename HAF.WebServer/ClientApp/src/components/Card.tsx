import './Card.css'
import React from "react";
import { CardType } from "types/CardType";
import { Rank, Suit } from "types/Enums";
import { getImageUrl } from 'utils/imageService';

interface CardProps {
  card: CardType;
}

const Card: React.FC<CardProps> = ({ card }) => {
  return <div className='mycard'>
    <p>{Suit[card.suit]}</p>
    <p>{Rank[card.rank]}</p>
    <img alt={getImageUrl(card)} src={getImageUrl(card)}></img>
  </div>
};

export default Card;