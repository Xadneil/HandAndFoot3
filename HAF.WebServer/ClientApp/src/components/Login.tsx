import { UserContext } from 'utils/context';
import { unauthorizedPost } from 'utils/network';
import React, { FunctionComponent, useContext, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { Button, Input } from 'reactstrap';
import { Player } from 'types/Player';

interface LoginProps {
  setPlayer(player: Player): void;
};

const Login: FunctionComponent<LoginProps> = ({ setPlayer }) => {
  const { playerId } = useContext(UserContext);
  const [playerName, setPlayerName] = useState('');

  const login = () => {
    unauthorizedPost<Player>({ endpoint: 'player/login', body: playerName }).then(setPlayer);
  };

  return (playerId !== null && playerId !== undefined) ? <Navigate to="/" /> : (
    <div>
      <h1>Enter your name</h1>
      <Input
        value={playerName}
        onChange={e => setPlayerName(e.target.value)}
        onKeyDown={e => {
          if (e.key === 'Enter')
            login();
        }}
      ></Input>
      <Button onClick={login}>Log In</Button>
    </div>
  );
}

export default Login;