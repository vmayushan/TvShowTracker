import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import {Auth0Provider} from './providers/Auth0Provider';
import * as serviceWorker from './serviceWorker';
import config from "./auth_config.json";
import history from './utils/history';

const onRedirectCallback = () => {
    // always redirect to main page after login
    history.push('/');
  };

  ReactDOM.render(
    <Auth0Provider
      domain={config.domain}
      client_id={config.clientId}
      redirect_uri={window.location.origin}
      onRedirectCallback={onRedirectCallback}
      audience={config.audience}
  >
      <App />
    </Auth0Provider>,
    document.getElementById("root")
  );

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
