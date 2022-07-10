import { useGet, usePost } from "utils/network";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "reactstrap";
import './Sessions.css';

const Sessions: React.FC = () => {
  const post = usePost();
  const get = useGet();
  const navigate = useNavigate();

  const [sessions, setSessions] = useState<Session[]>([]);

  useEffect(() => {
    get<Session[]>('session/JoinableSessions').then(setSessions);
  }, [get]);

  const waitOrStart = ({ task, sessionId }: SessionResponse) => {
    if (task === 'Wait') {
      navigate('/wait', { state: sessionId });
    }
    else if (task === 'Start') {
      navigate('/game', { state: sessionId });
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
