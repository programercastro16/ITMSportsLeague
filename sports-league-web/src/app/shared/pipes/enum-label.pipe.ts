import { Pipe, PipeTransform } from '@angular/core';
import {
  PlayerPosition,
  SponsorCategory,
  TournamentStatus,
} from '../../core/models/enums';

@Pipe({ name: 'enumLabel', standalone: true })
export class EnumLabelPipe implements PipeTransform {
  transform(value: number | string, type: string): string {
    switch (type) {
      case 'tournamentStatus':
        return this.tournamentStatus(Number(value));
      case 'sponsorCategory':
        return this.sponsorCategory(Number(value));
      case 'playerPosition':
        return this.playerPosition(Number(value));
      default:
        return String(value);
    }
  }

  private tournamentStatus(status: TournamentStatus): string {
    const labels: Record<TournamentStatus, string> = {
      [TournamentStatus.Pending]: 'Pendiente',
      [TournamentStatus.InProgress]: 'En curso',
      [TournamentStatus.Finished]: 'Finalizado',
    };
    return labels[status as TournamentStatus] ?? String(status);
  }

  private sponsorCategory(category: SponsorCategory): string {
    const labels: Record<SponsorCategory, string> = {
      [SponsorCategory.Main]: 'Principal',
      [SponsorCategory.Gold]: 'Oro',
      [SponsorCategory.Silver]: 'Plata',
      [SponsorCategory.Bronze]: 'Bronce',
    };
    return labels[category as SponsorCategory] ?? String(category);
  }

  private playerPosition(position: PlayerPosition): string {
    const labels: Record<PlayerPosition, string> = {
      [PlayerPosition.Goalkeeper]: 'Portero',
      [PlayerPosition.Defender]: 'Defensa',
      [PlayerPosition.Midfielder]: 'Mediocampista',
      [PlayerPosition.Forward]: 'Delantero',
    };
    return labels[position as PlayerPosition] ?? String(position);
  }
}
