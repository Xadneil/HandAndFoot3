import './custom.css';
import React, { useEffect, useState } from 'react';
import { Route, Navigate, useLocation, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import Login from './components/Login';
import { defaultUserContext, IUserContext, UserContext } from 'utils/context';
import Sessions from 'components/Sessions';
import { Player } from 'types/Player';
import Wait from 'components/Wait';
import Game from 'components/Game';

const initialUserContext: IUserContext = JSON.parse(localStorage.getItem('userContext')) || defaultUserContext;

const App = () => {
  const [contextValue, setContextValue] = useState(initialUserContext);
  const location = useLocation();

  const setPlayer = (player: Player) => {
    setContextValue({ ...contextValue, ...player });
  }

  useEffect(() => {
    localStorage.setItem('userContext', JSON.stringify(contextValue));
  }, [contextValue])

  useEffect(() => {
    if (location.pathname === '/') {
      setContextValue(JSON.parse(localStorage.getItem('userContext')) || defaultUserContext);
    }
  }, [location]);

  return (
    <UserContext.Provider value={contextValue}>
      <Layout>
        {(contextValue.playerId !== null && contextValue.playerId !== undefined) ?
          <Routes>
            <Route path='/' element={<Navigate to="/sessions" />} />
            <Route path='/sessions' element={<Sessions />} />
            <Route path='/wait/:sessionId' element={<Wait />} />
            <Route path='/game/:sessionId' element={<Game />} />
          </Routes>
          :
          <Login
            setPlayer={setPlayer}
          />}
      </Layout>
    </UserContext.Provider>
  );
};

export default App;