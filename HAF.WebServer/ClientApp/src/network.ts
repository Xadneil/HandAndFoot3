import { UserContext } from "context";
import { useContext, useMemo } from "react";
import { History } from 'history';
import { useHistory } from "react-router-dom";

interface GetParams {
  endpoint: string;
  headers?: Record<string, string>;
  signal?: AbortSignal;
}

interface PostParams extends GetParams {
  body: any;
}

const serverURLBase = 'https://localhost:44352/api/';

const responseHandler = (history?: History) => (response: Response) => {
  if (history && !response.ok && response.status === 401) {
    localStorage.removeItem('userContext');
    history.push('/');
    throw new Error(`HTTP status code: ${response.status}`);
  }
  if (!response.ok) {
    const error = new Error(`HTTP status code: ${response.status}`);
    alert(error);
    throw error;
  }
  return response.json();
}

export const unauthorizedPost = <T>({ endpoint, body, signal, headers }: PostParams, history?: History): Promise<T> =>
  fetch(serverURLBase + endpoint, {
    body: JSON.stringify(body),
    headers: {
      'Content-Type': 'application/json',
      ...headers
    },
    method: 'POST',
    signal: signal
  }).then(responseHandler(history));

export const unauthorizedGet = <T>({ endpoint, signal, headers }: GetParams, history?: History): Promise<T> =>
  fetch(serverURLBase + endpoint, {
    headers: headers,
    method: 'GET',
    signal: signal
  }).then(responseHandler(history));

export const usePost = () => {
  const { playerId, secret } = useContext(UserContext);
  const history = useHistory();
  return useMemo(() => <T>(params: PostParams): Promise<T> => {
    params.headers = {
      ...params.headers,
      'X-PlayerId': String(playerId),
      'X-Secret': secret
    };
    return unauthorizedPost(params, history);
  }, [playerId, secret, history]);
};

export const useGet = () => {
  const { playerId, secret } = useContext(UserContext);
  const history = useHistory();
  return useMemo(() => <T>(endpointOrParams: GetParams | string): Promise<T> => {
    let params: GetParams;
    if (typeof endpointOrParams === 'string')
      params = { endpoint: endpointOrParams }
    else
      params = endpointOrParams;
    params.headers = {
      ...params.headers,
      'X-PlayerId': String(playerId),
      'X-Secret': secret
    };
    return unauthorizedGet(params, history);
  }, [playerId, secret, history]);
};

export const websocket = (endpoint: string) => {
  return new WebSocket(serverURLBase.replace('https', 'wss') + endpoint);
};