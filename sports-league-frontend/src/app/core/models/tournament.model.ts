export interface Tournament {
  id: number;
  name: string;
  season: string;
  startDate: string;
  endDate: string;
  status: TournamentStatus;
  teamsCount: number;
  createdAt: string;
  updatedAt: string | null;
}
 
export interface TournamentRequest {
  name: string;
  season: string;
  startDate: string;
  endDate: string;
}
 
export enum TournamentStatus {
  Pending = 0,
  InProgress = 1,
  Finished = 2
}
