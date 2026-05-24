export interface Referee {
  id: number;
  firstName: string;
  lastName: string;
  nationality: string;
  createdAt: string;
  updatedAt: string | null;
}
 
export interface RefereeRequest {
  firstName: string;
  lastName: string;
  nationality: string;
}
