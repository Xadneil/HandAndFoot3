import { useGet, usePost } from "network";
import React, { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { Button } from "reactstrap";
import './Sessions.css';

const Sessions: React.FC = () => {
  const post = usePost();
  const get = useGet();
  const history = useHistory();

  const [sessions, setSessions] = useState<Session[]>([]);

  const refresh = () => {
    get<Session[]>('session/JoinableSessions').then(setSessions);
  };

  useEffect(refresh, []);

  const waitOrStart = ({ task, sessionId }: SessionResponse) => {
    if (task === 'Wait') {
      history.push('/wait', sessionId);
    }
    else if (task === 'Start') {
      history.push('/game', sessionId);
    }
  };

  const onNewSession = () => {
    post<SessionResponse>({
      endpoint: 'session/CreateSession',
      body: { sessionName: 'Test' }
    }).then(waitOrStart);
  }

  const onJoinSession = (sessionId: number) => {
    post<SessionResponse>({
      endpoint: 'session/JoinSession',
      body: sessionId
    }).then(waitOrStart);
  }

  return (
    <div>
      <Button className="newSession" onClick={onNewSession}>New Session</Button>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th># Players Waiting</th>
            <th>Password Protected</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {sessions.map(s => <tr key={s.sessionId}>
            <td>{s.sessionName}</td>
            <td>{s.numberOfPlayers}</td>
            <td>{s.hasPassword ? 'Yes' : 'No'}</td>
            <th><Button onClick={() => onJoinSession(s.sessionId)}>Join</Button></th>
          </tr>)}
        </tbody>
      </table>
    </div>
  )
};

export default Sessions;