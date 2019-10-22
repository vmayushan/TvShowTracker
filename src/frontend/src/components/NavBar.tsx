import React from "react";
import { useAuth0 } from "../providers/Auth0Provider";
import { AppBar as MuiAppBar, Button, IconButton, Toolbar } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import MenuIcon from '@material-ui/icons/Menu';
import ShowSearch from '../components/ShowSearch'
 
const useStyles = makeStyles({
  grow: {
    flexGrow: 1,
  },
  search: {
    width: '400px',
    color: 'black',
  },
  menuButton: {
    marginLeft: -12,
    marginRight: 20,
  },
});
 
function NavBar() {
    const { isAuthenticated, loginWithRedirect, logout } = useAuth0();
    const classes = useStyles();
   
    return (
      <MuiAppBar position="static">
        <Toolbar>
          <IconButton className={classes.menuButton} color="inherit" aria-label="Menu">
            <MenuIcon />
          </IconButton>
          <div className={classes.search}>
            <ShowSearch></ShowSearch>
          </div>
          <div className={classes.grow}></div>
          {isAuthenticated && <Button color="inherit" onClick={() => logout()}>Log Out</Button>}
          {!isAuthenticated && <Button color="inherit" onClick={() => loginWithRedirect({})}>Log In</Button>}
        </Toolbar>
      </MuiAppBar>
    );
  }
 
export default NavBar;