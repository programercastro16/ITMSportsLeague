export interface Match {
  id: number;
  tournamentId: number;
  tournamentName: string;
  homeTeamId: number;
  homeTeamName: string;
  awayTeamId: number;
  awayTeamName: string;
  refereeId: number;
  refereeFullName: string;
  matchDate: string;
  venue: string;
  matchday: number;
  status: MatchStatus;
  createdAt: string;
  updatedAt: string | null;
}
 
export interface MatchRequest {
  tournamentId: number;
  homeTeamId: number;
  awayTeamId: number;
  refereeId: number;
  matchDate: string;
  venue: string;
  matchday: number;
}
 
export enum MatchStatus {
  Scheduled = 0,
  InProgress = 1,
  Finished = 2,
  Suspended = 3
}
 
export interface MatchResult {
  id: number;
  matchId: number;
  homeGoals: number;
  awayGoals: number;
  observations: string | null;
  createdAt: string;
}
 
export interface MatchResultRequest {
  homeGoals: number;
  awayGoals: number;
  observations: string | null;
}
 
export interface Goal {
  id: number;
  matchId: number;
  playerId: number;
  playerName: string;
  minute: number;
  type: GoalType;
  createdAt: string;
}
 
export interface GoalRequest {
  playerId: number;
  minute: number;
  type: GoalType;
}
 
export interface Card {
  id: number;
  matchId: number;
  playerId: number;
  playerName: string;
  minute: number;
  type: CardType;
  createdAt: string;
}
 
export interface CardRequest {
  playerId: number;
  minute: number;
  type: CardType;
}
 
export enum GoalType { Normal = 0, Penalty = 1, OwnGoal = 2 }
export enum CardType { Yellow = 0, Red = 1 }
