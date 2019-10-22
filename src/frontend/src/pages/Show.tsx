import React, {Fragment, useEffect, useState} from 'react';
import {useParams} from 'react-router';
import ApiClient from '../api/ApiClient';
import {ShowWithSeasonsDto, EpisodeDto, SeasonDto} from '../models/ShowWithSeasonsDto';
import Season from '../components/Season';
import {Grid, Typography} from '@material-ui/core';
import {useAuth0} from '../providers/Auth0Provider';
export default function Show() {
  const [show, setShow] = useState<ShowWithSeasonsDto>();
  const [count, setCount] = useState(0);
  const { id } = useParams();
  const { getTokenSilently } = useAuth0();

  if(!id) throw new Error('show id is not defined');

  const handleChange = (season: SeasonDto) => (episode: EpisodeDto) => async () => {
    const token = await getTokenSilently() as string;
    if(episode.isCompleted) {
      await ApiClient.markAsNotWatched(token, id, season.seasonNumber, episode.episodeNumber);
    } else {
      await ApiClient.markAsWatched(token, id, season.seasonNumber, episode.episodeNumber);
    }
    setCount(count+1);
  };

  useEffect(() => {
    const fn = async () => { 
      const token = await getTokenSilently() as string;
      ApiClient.getShow(token, id).then(response => {
        if(response.data) {
          setShow(response.data)
        }
      });
    };
    fn();
  }, [id, count, getTokenSilently]);

  if (!show) {
    return (
      <div>Loading...</div>
    );
  }

  return (
    <Fragment>
      <br/>
      <Grid container spacing={3}>
        <Grid item xs>
          <Typography component="h3" variant="h4" align="center" color="textPrimary">
            {show.title}
          </Typography>
          <Typography variant="h5" align="center" color="textSecondary" paragraph>
            {show.nextSeasonNumber && show.nextEpisodeNumber 
            ? <strong>Next to watch: season {show.nextSeasonNumber} episode {show.nextEpisodeNumber}</strong> 
            : null}
          </Typography>
          <p>{show.plot}</p>
          <img src={show.poster} alt="Poster" />
        </Grid>
        <Grid item xs>
          {show.seasons.map(season =>  (
            <Season 
              key={season.seasonNumber}
              season={season} 
              isCurrent={season.seasonNumber === show.nextSeasonNumber} 
              onCheckItem={handleChange(season)}></Season>
          ))}
        </Grid>
      </Grid>
    </Fragment>
  );
}