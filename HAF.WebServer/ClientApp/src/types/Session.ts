type Session = {
  sessionId: number;
  sessionName: string;
  hasPassword: boolean;
  lastUpdated: Date;
  numberOfPlayers: number;
};

type SessionResponse = {
  sessionId: number;
  task: 'Wait' | 'Start';
}