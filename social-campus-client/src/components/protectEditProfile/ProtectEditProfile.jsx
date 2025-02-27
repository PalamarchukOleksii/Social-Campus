import React from "react";
import { Outlet, useParams, useLocation, Navigate } from "react-router-dom";
import useAuth from "../../hooks/useAuth";

const ProtectEditProfile = () => {
  const { auth } = useAuth();
  const { login } = useParams();
  const location = useLocation();

  if (auth.shortUser.login !== login) {
    return (
      <Navigate
        to={location.state?.from || `/profile/${auth.shortUser.login}/edit`}
        replace
      />
    );
  }

  return <Outlet />;
};

export default ProtectEditProfile;
