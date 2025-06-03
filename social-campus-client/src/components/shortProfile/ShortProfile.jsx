import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import { useLocation, useNavigate } from "react-router-dom";
import "./ShortProfile.css";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import Loading from "../loading/Loading";
import { toast } from "react-toastify";

const GET_USER_BY_ID_URL = "/api/users/by-id/";

function ShortProfile({ userId, redirectOnClick = true }) {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();
  const axios = useAxiosPrivate();

  useEffect(() => {
    if (!userId) return;

    const fetchUserData = async () => {
      try {
        const response = await axios.get(`${GET_USER_BY_ID_URL}${userId}`);
        setUser(response.data);
      } catch (err) {
        toast(err);
      }
    };

    fetchUserData();
  }, [userId]);

  const handleProfileClick = () => {
    if (
      redirectOnClick &&
      user &&
      location.pathname !== `/profile/${user.login}`
    ) {
      navigate(`/profile/${user.login}`);
      window.scrollTo(0, 0);
    }
  };

  if (!user) {
    return <Loading />;
  }

  return (
    <div className="short-info" onClick={handleProfileClick}>
      <img
        src={user.profileImageUrl || "/default-profile.png"}
        alt="Profile"
        className="profile-image"
      />
      <div className="profile-info">
        <h3 className="general-text">{user.firstName + " " + user.lastName}</h3>
        <h4 className="login not-general-text">@{user.login}</h4>
      </div>
    </div>
  );
}

ShortProfile.propTypes = {
  userId: PropTypes.string.isRequired,
  redirectOnClick: PropTypes.bool,
};

export default ShortProfile;
