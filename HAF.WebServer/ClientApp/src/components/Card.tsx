import React from "react";
import { CardType } from "types/CardType";
import { getImageUrl } from 'utils/imageService';

interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  card: CardType;
}

const Card: React.FC<CardProps> = ({ card, ...props }) => {
  return <img alt={getImageUrl(card)} src={getImageUrl(card)} {...props}></img>
};

export default Card;