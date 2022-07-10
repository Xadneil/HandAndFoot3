import { websocket } from "utils/network";
import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";

const Wait: React.FC = () => {
  const [numPlayers, setNumPlayers] = useState(0);
  const sessionId = useLocation().state as number;
  const navigate = useNavigate();

  useEffect(() => {
    const ws = websocket(`wait/WaitForGameStart/${sessionId}`);

    const messageReceived = (newNumPlayers: number) => {
      setNumPlayers(newNumPlayers);
      ws.send('ping');
    };
    ws.addEventListener('message', (e) => messageReceived(Number(e.data)));
    let closed = false;
    const closeHandler = (e: CloseEvent) => {
      closed = true;
      if (e.reason === 'Start')
        navigate('/game', { state: sessionId });
      else if (e.reason)
        alert(e.reason);
    };
    ws.addEventListener('close', closeHandler);

    ws.addEventListener('open', () => ws.send('ping'));

    return () => {
      if (!closed) {
        ws.removeEventListener('close', closeHandler);
        ws.close();
      }
    };
  }, [sessionId, navigate]);

  return (
    <div>
      <p>Waiting for players to join.</p>
      {numPlayers > 0 ? <p>{numPlayers} out of 4 currently waiting.</p> : null}
      <button onClick={() => navigate('/game', { state: sessionId })}>Force game entry</button>
    </div>
  )
};

export default Wait;