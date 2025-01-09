import React from "react";
import PropTypes from "prop-types";
import { useLocation, useNavigate } from "react-router-dom";
import "./ShortProfile.css";

function ShortProfile({
  profileImage,
  username,
  login,
  redirectOnClick = true,
}) {
  const navigate = useNavigate();
  const location = useLocation();

  const handleProfileClick = () => {
    if (redirectOnClick && location.pathname !== `/profile/${login}`) {
      navigate(`/profile/${login}`);
      window.scrollTo(0, 0);
    }
  };

  return (
    <div className="short-info" onClick={handleProfileClick}>
      <img
        src={profileImage || "/default-profile.png"}
        alt="Profile"
        className="profile-image"
      />
      <div className="profile-info">
        <h3 className="general-text">{username}</h3>
        <h4 className="login not-general-text">@{login}</h4>
      </div>
    </div>
  );
}

ShortProfile.propTypes = {
  profileImage: PropTypes.string,
  username: PropTypes.string.isRequired,
  login: PropTypes.string.isRequired,
  redirectOnClick: PropTypes.bool,
};

export default ShortProfile;
