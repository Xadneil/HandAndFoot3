import './Game.css';
import { useGet } from "network";
import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { SinglePlayerGameState } from "types/SinglePlayerGameState";
import Card from "./Card";

const Game: React.FC = () => {
	const get = useGet();
	const sessionId = useLocation<number>().state;
	const [state, setState] = useState<SinglePlayerGameState>(null);

	useEffect(() => {
		const abortHandler = new AbortController();
		if (!state) {
			get<SinglePlayerGameState>({
				endpoint: `game/init/${sessionId}`,
				signal: abortHandler.signal
			}).then(state => {
				if (!abortHandler.signal.aborted)
					setState(state);
			});
		}
		return () => abortHandler.abort();
	}, [sessionId, get, state])

	return (
		<div className="currentHand">
			{state && state.cardHolder.currentHand.map((c, i) => <Card key={i} card={c} />)}
		</div>
	)
};

export default Game;