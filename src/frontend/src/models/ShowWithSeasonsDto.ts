export interface ShowWithSeasonsDto {
    id: string;
    title: string;
    poster: string;
    plot: string;
    nextSeasonNumber?: number;
    nextEpisodeNumber?: number;
    seasons: SeasonDto[];
}

export interface SeasonDto {
    seasonNumber: number;
    isCompleted: boolean;
    episodes: EpisodeDto[];
}

export interface EpisodeDto {
    isCompleted: boolean;
    episodeNumber: number;
    title: string;
    imdbId: string;
}