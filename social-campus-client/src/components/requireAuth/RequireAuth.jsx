import React from "react";
import { Navigate, useLocation, Outlet } from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import ROUTES from "../../utils/consts/Routes";
import PropTypes from "prop-types";

function RequireAuth() {
  const { auth } = useAuth();
  const location = useLocation();

  return auth?.shortUser ? (
    <Outlet />
  ) : (
    <Navigate to={ROUTES.LANDING} state={{ from: location }} replace />
  );
}

RequireAuth.propTypes = {
  children: PropTypes.node,
};

export default RequireAuth;
