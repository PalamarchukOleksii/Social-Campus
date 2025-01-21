import React from "react";
import { Navigate, useLocation, Outlet } from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import ROUTES from "../../utils/consts/Routes";

function RequireAuth() {
  const { auth } = useAuth();
  const location = useLocation();

  return auth?.accessToken ? (
    <Outlet />
  ) : (
    <Navigate to={ROUTES.LANDING} state={{ from: location }} replace />
  );
}

export default RequireAuth;
