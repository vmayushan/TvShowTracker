  
import React, {useState} from 'react';
import MUIList from '@material-ui/core/List';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Episode from '../components/Episode';
import {SeasonDto} from '../models/ShowWithSeasonsDto'

const Season = ({onCheckItem, isCurrent, season}: {season: SeasonDto, isCurrent: boolean, onCheckItem: any}) => {
    const [expanded, setExpanded] = useState(isCurrent);
    const handleChange = () => (event: React.ChangeEvent<{}>, isExpanded: boolean) => {
        setExpanded(isExpanded);
    };
    
    return (
        <ExpansionPanel expanded={expanded} onChange={handleChange()}>
            <ExpansionPanelSummary expandIcon={<ExpandMoreIcon/>}>
                <Typography>Season {season.seasonNumber}</Typography>
            </ExpansionPanelSummary>
            <ExpansionPanelDetails>
                {expanded &&
                <MUIList>
                    {season.episodes.map(episode =>
                        <Episode 
                        key={episode.episodeNumber} 
                        item={episode} 
                        onCheckItem={onCheckItem(episode)}></Episode>
                    )}
                </MUIList>}
            </ExpansionPanelDetails>
        </ExpansionPanel>
    )
};

export default Season;