import React, { useEffect } from "react";
import { Route, RouteProps, RouteComponentProps } from "react-router-dom";
import { useAuth0 } from "../providers/Auth0Provider"

const PrivateRoute = ({ component: Component, path, ...rest }: RouteProps) => {
  if (!Component) {
    throw Error("component is undefined");
  }

  const { loading, isAuthenticated, loginWithRedirect } = useAuth0();

  useEffect(() => {
    if (loading || isAuthenticated) {
      return;
    }

    const fn = async () => {
      await loginWithRedirect({
        appState: { targetUrl: path }
      });
    };
    fn();
  }, [loading, isAuthenticated, loginWithRedirect, path]);

  const render = (props: RouteComponentProps<any>) => isAuthenticated === true 
  ? <Component {...props} /> 
  : null;

  return <Route path={path} render={render} {...rest} />;
};

export default PrivateRoute;