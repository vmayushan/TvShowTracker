import React from 'react';
import MUIListItem from '@material-ui/core/ListItem';
import Checkbox from '@material-ui/core/Checkbox';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import {EpisodeDto} from '../models/ShowWithSeasonsDto';

const Episode = React.memo(({onCheckItem, item}: {onCheckItem: any, item: EpisodeDto}) => {
    return (
        <MUIListItem>
            <FormControlLabel control={
                <Checkbox onChange={onCheckItem} checked={item.isCompleted} value={item.title}/>
            } label={item.title ? `Episode ${item.episodeNumber} - ${item.title}` : `Episode ${item.episodeNumber}`}/>
        </MUIListItem>
    )
});

export default Episode;