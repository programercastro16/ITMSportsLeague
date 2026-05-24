export interface Team {
  id: number;
  name: string;
  city: string;
  stadium: string;
  logoUrl: string | null;
  foundedDate: string;
  createdAt: string;
  updatedAt: string | null;
}
 
export interface TeamRequest {
  name: string;
  city: string;
  stadium: string;
  logoUrl: string | null;
  foundedDate: string;
}
