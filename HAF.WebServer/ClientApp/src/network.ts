import { UserContext } from "context";
import { useContext } from "react";

interface GetParams {
  endpoint: string;
  headers?: Record<string, string>;
  signal?: AbortSignal;
}

interface PostParams extends GetParams {
  body: any;
}

const serverURLBase = 'https://localhost:44352/api/';

function responseHandler(response: Response) {
  if (!response.ok) {
    const error = new Error(`HTTP status code: ${response.status}`);
    alert(error);
    throw error;
  }
  return response.json();
}

export const unauthorizedPost = <T>({ endpoint, body, signal, headers }: PostParams): Promise<T> =>
  fetch(serverURLBase + endpoint, {
    body: JSON.stringify(body),
    headers: {
      'Content-Type': 'application/json',
      ...headers
    },
    method: 'POST',
    signal: signal
  }).then(responseHandler);

export const unauthorizedGet = <T>({ endpoint, signal, headers }: GetParams): Promise<T> =>
  fetch(serverURLBase + endpoint, {
    headers: headers,
    method: 'GET',
    signal: signal
  }).then(responseHandler);

export const usePost = () => {
  const { playerId, secret } = useContext(UserContext);
  return <T>(params: PostParams): Promise<T> => {
    params.headers = {
      ...params.headers,
      'X-PlayerId': String(playerId),
      'X-Secret': secret
    };
    return unauthorizedPost(params);
  }
};

export const useGet = () => {
  const { playerId, secret } = useContext(UserContext);
  return <T>(endpointOrParams: GetParams | string): Promise<T> => {
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
    return unauthorizedGet(params);
  }
};