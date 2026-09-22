import { SponsorCategory } from './enums';

export interface Sponsor {
  id: number;
  name: string;
  contactEmail: string;
  phone?: string | null;
  websiteUrl?: string | null;
  category: SponsorCategory;
  createdAt: string;
  updatedAt?: string | null;
}

export interface SponsorRequest {
  name: string;
  contactEmail: string;
  phone?: string | null;
  websiteUrl?: string | null;
  category: SponsorCategory;
}

export interface TournamentSponsor {
  id: number;
  tournamentId: number;
  tournamentName: string;
  sponsorId: number;
  sponsorName: string;
  contractAmount: number;
  joinedAt: string;
}

export interface TournamentSponsorRequest {
  tournamentId: number;
  contractAmount: number;
}
