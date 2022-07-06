import './custom.css';
import React, { useEffect, useState } from 'react';
import { Route, Redirect } from 'react-router-dom';
import Layout from './components/Layout';
import Login from './components/Login';
import { defaultUserContext, IUserContext, UserContext } from 'context';
import Sessions from 'components/Sessions';
import { Player } from 'types/Player';

const initialUserContext: IUserContext = JSON.parse(localStorage.getItem('userContext')) || defaultUserContext;

const App = () => {
  const [contextValue, setContextValue] = useState(initialUserContext);

  // const setContextProperty = (property: keyof IUserContext) => (value: any) => {
  //   setContextValue({ ...contextValue, [property]: value });
  // };

  const setPlayer = (player: Player) => {
    setContextValue({ ...contextValue, ...player });
  }

  useEffect(() => {
    localStorage.setItem('userContext', JSON.stringify(contextValue));
  }, [contextValue])

  return (
    <UserContext.Provider value={contextValue}>
      <Layout>
        {(contextValue.playerId !== null && contextValue.playerId !== undefined) ?
          <>
            <Route exact path='/'>
              <Redirect to={'/sessions'} />
            </Route>
            <Route path='/sessions' component={Sessions} />
          </>
          :
          <Login
            setPlayer={setPlayer}
          />}
      </Layout>
    </UserContext.Provider>
  );
};

export default App;