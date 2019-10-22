import React from 'react';
import NavBar from './components/NavBar';
import { useAuth0 } from './providers/Auth0Provider';
import { Router, Switch } from 'react-router-dom';
import './App.css';
import PrivateRoute from './components/PrivateRoute';
import history from './utils/history';
import Show from './pages/Show';
import Shows from './pages/Shows';

const App: React.FC = () => {
  const { loading } = useAuth0();

  if (loading) {
    return (
      <div>Loading...</div>
    );
  }

  return (
    <div className="App">
       <Router history={history}>
        <header>
          <NavBar />
        </header>
        <Switch>
          <PrivateRoute path="/" exact component={Shows} />
          <PrivateRoute path="/show/:id" component={Show} />
        </Switch>
      </Router>
    </div>
  );
}

export default App;
