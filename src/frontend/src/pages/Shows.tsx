import {makeStyles} from '@material-ui/core/styles';
import React, {Fragment, useEffect, useState} from 'react';
import ApiClient from '../api/ApiClient';
import {ShowDto} from '../models/ShowDto';
import {Link} from 'react-router-dom';
import {Container, Grid, CardMedia, Card, CardContent, Typography, CardActions} from '@material-ui/core';
import {useAuth0} from '../providers/Auth0Provider';

const useStyles = makeStyles(theme => ({
  cardGrid: {
    paddingTop: theme.spacing(8),
    paddingBottom: theme.spacing(8),
  },
  card: {
    height: '100%',
    display: 'flex',
    flexDirection: 'column',
  },
  cardMedia: {
    paddingTop: '56.25%', // 16:9
  },
  cardContent: {
    flexGrow: 1,
  },
  cardLink: {
    color: 'black',
    textDecoration: 'none',
  },
}));

export default function Shows() {
  const classes = useStyles();
  const [shows, setShows] = useState<ShowDto[]>();
  const { getTokenSilently } = useAuth0();

  useEffect(() => {
    const fn = async () => { 
      const token = await getTokenSilently() as string;
      ApiClient.getShows(token).then(response => {
        if(response.data) {
          setShows(response.data)
        }
      });
    };
    fn();
  }, [getTokenSilently]);

  if (!shows) {
    return (
      <div>Loading...</div>
    );
  }

  if (!shows.length) {
    return (
      <Fragment>
        <br/>
        <div>No shows found, use search to add new</div>
      </Fragment>
    );
  }

  return (
    <Fragment>
      <Container className={classes.cardGrid} maxWidth="md">
          <Grid container spacing={4}>
            {shows.map(show => (
              <Grid item key={show.id} xs={12} sm={6} md={4}>
                <Card className={classes.card}>
                  <CardMedia
                    className={classes.cardMedia}
                    image={show.poster}
                    title="Poster"
                  />
                  <CardContent className={classes.cardContent}>
                    <Typography gutterBottom variant="h5" component="h2">
                      <Link className={classes.cardLink} to={`/show/${show.id}`}>{show.title}</Link>
                    </Typography>
                  </CardContent>
                  <CardActions>
                    <Typography>
                      {show.nextSeasonNumber && show.nextEpisodeNumber 
                        ? <strong>Next to watch: season {show.nextSeasonNumber} episode {show.nextEpisodeNumber}</strong> 
                        : null}
                      </Typography>
                  </CardActions>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Container>
    </Fragment>
  );
}