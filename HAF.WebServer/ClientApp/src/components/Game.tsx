import './Game.css';
import { useGet, usePost } from "utils/network";
import React, { useEffect, useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import { SinglePlayerGameState } from "types/SinglePlayerGameState";
import Card from "./Card";
import { Button } from 'reactstrap';

const Game: React.FC = () => {
	const get = useGet();
	const post = usePost();
	const { sessionId } = useParams();
	const headers = useMemo(() => ({ 'X-SessionId': sessionId }), [sessionId]);
	const [state, setState] = useState<SinglePlayerGameState>(null);

	useEffect(() => {
		const abortHandler = new AbortController();
		if (!state) {
			get<SinglePlayerGameState>({
				endpoint: `game/init`,
				headers: headers,
				signal: abortHandler.signal
			}).then(state => {
				if (!abortHandler.signal.aborted)
					setState(state);
			});
		}
		return () => abortHandler.abort();
	}, [headers, get, state]);

	const onDraw = () => {
		post<SinglePlayerGameState>({
			endpoint: `game/draw`,
			headers: headers,
		}).then(state => setState(state));
	};

	return (<div className='game'>
		<div className='game-area'></div>
		<div className='buttons'>
			<Button onClick={onDraw}>Draw 2</Button>
		</div>
		<div className="currentHand">
			{state && state.cardHolder.currentHand.map((c, i) => <div key={i} className='card-container'><Card className="haf-card" card={c} /></div>)}
		</div>
	</div>
	)
};

export default Game;