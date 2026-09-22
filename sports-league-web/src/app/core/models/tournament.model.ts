import { TournamentStatus } from './enums';

export interface Tournament {
  id: number;
  name: string;
  season: string;
  startDate: string;
  endDate: string;
  status: TournamentStatus;
  teamsCount: number;
  createdAt: string;
  updatedAt?: string | null;
}

export interface TournamentRequest {
  name: string;
  season: string;
  startDate: string;
  endDate: string;
}

export interface UpdateStatusRequest {
  status: TournamentStatus;
}

export interface RegisterTeamRequest {
  teamId: number;
}
