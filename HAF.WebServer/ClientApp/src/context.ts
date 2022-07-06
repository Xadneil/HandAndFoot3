import React from "react";
import { Player } from "types/Player";

export interface IUserContext extends Player {
};

export const defaultUserContext: IUserContext = {
  name: '',
  secret: null,
  playerId: null
};

export const UserContext = React.createContext(defaultUserContext);