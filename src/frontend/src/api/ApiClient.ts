import axios, {AxiosInstance} from 'axios';
import {ShowSearchDto} from '../models/ShowSearchDto';
import {ShowWithSeasonsDto} from '../models/ShowWithSeasonsDto';
import {ShowDto} from '../models/ShowDto';

export class ApiClient {
    private readonly client: AxiosInstance;
    constructor() {
        this.client = axios.create({
            baseURL: `https://localhost:5001/`
          });
    }

    searchShows(authToken: string, query: string) {
        const params = new URLSearchParams();
        params.set('query', query);

        return this.client.get<ShowSearchDto[]>('show/search', {
            params: params,
            headers: { Authorization: 'Bearer ' + authToken } 
        });
    }

    getShow(authToken: string, id: string) {
        return this.client.get<ShowWithSeasonsDto>(`show/${id}`, {
            headers: { Authorization: 'Bearer ' + authToken } 
        });
    }

    deleteShow(authToken: string, id: string) {
        return this.client.delete<void>(`show/${id}`, {
            headers: { Authorization: 'Bearer ' + authToken } 
        });
    }

    getShows(authToken: string) {
        return this.client.get<ShowDto[]>('show', {
            headers: { Authorization: 'Bearer ' + authToken } 
        });
    }

    startWatching(authToken: string, imdbId: string) {
        return this.client.post<ShowDto>(`show?imdbId=${imdbId}`, null, {
            headers: { Authorization: 'Bearer ' + authToken } 
        });
    }

    markAsWatched(authToken: string, showId: string, season: number, episode: number) {
        return this.client.put<ShowDto>(
            `show/${showId}/season/${season}/episode/${episode}/watched`, null, {
                headers: { Authorization: 'Bearer ' + authToken } 
            }
        );
    }

    markAsNotWatched(authToken: string, showId: string, season: number, episode: number) {
        return this.client.put<ShowDto>(
            `show/${showId}/season/${season}/episode/${episode}/not-watched`, null, {
                headers: { Authorization: 'Bearer ' + authToken } 
            })
        ;
    }
}

export default new ApiClient();