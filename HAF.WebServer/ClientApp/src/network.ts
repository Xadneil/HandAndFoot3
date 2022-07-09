import { UserContext } from "context";
import { useContext, useMemo } from "react";
import { useNavigate, NavigateFunction } from "react-router-dom";

interface GetParams {
  endpoint: string;
  headers?: Record<string, string>;
  signal?: AbortSignal;
}

interface PostParams extends GetParams {
  body: any;
}

const serverURLBase = 'https://localhost:44352/api/';

const responseHandler = (navigate?: NavigateFunction) => (response: Response) => {
  if (navigate && !response.ok && response.status === 401) {
    localStorage.removeItem('userContext');
    navigate('/');
    throw new Error(`HTTP status code: ${response.status}`);
  }
  if (!response.ok) {
    const error = new Error(`HTTP status code: ${response.status}`);
    alert(error);
    throw error;
  }
  return response.json();
}

export const unauthorizedPost = <T>({ endpoint, body, signal, headers }: PostParams, navigate?: NavigateFunction): Promise<T> =>
  fetch(serverURLBase + endpoint, {
    body: JSON.stringify(body),
    headers: {
      'Content-Type': 'application/json',
      ...headers
    },
    method: 'POST',
    signal: signal
  }).then(responseHandler(navigate));

export const unauthorizedGet = <T>({ endpoint, signal, headers }: GetParams, navigate?: NavigateFunction): Promise<T> =>
  fetch(serverURLBase + endpoint, {
    headers: headers,
    method: 'GET',
    signal: signal
  }).then(responseHandler(navigate));

export const usePost = () => {
  const { playerId, secret } = useContext(UserContext);
  const navigate = useNavigate();
  return useMemo(() => <T>(params: PostParams): Promise<T> => {
    params.headers = {
      ...params.headers,
      'X-PlayerId': String(playerId),
      'X-Secret': secret
    };
    return unauthorizedPost(params, navigate);
  }, [playerId, secret, navigate]);
};

export const useGet = () => {
  const { playerId, secret } = useContext(UserContext);
  const navigate = useNavigate();
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
    return unauthorizedGet(params, navigate);
  }, [playerId, secret, navigate]);
};

export const websocket = (endpoint: string) => {
  return new WebSocket(serverURLBase.replace('https', 'wss') + endpoint);
};