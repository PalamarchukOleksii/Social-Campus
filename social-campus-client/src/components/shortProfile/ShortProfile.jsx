import React from "react";
import PropTypes from "prop-types";
import { useLocation, useNavigate } from "react-router-dom";
import "./ShortProfile.css";

function ShortProfile(props) {
  const navigate = useNavigate();
  const location = useLocation();

  const handleProfileClick = () => {
    if (location.pathname !== `/profile/${props.login}`) {
      navigate(`/profile/${props.login}`);
      window.scrollTo(0, 0);
    }
  };

  return (
    <div className="short-info" onClick={handleProfileClick}>
      <img
        src={props.profileImage || "/default-profile.png"}
        alt="Profile"
        className="profile-image"
      />
      <div className="profile-info">
        <h3 className="general-text">{props.username}</h3>
        <h4 className="not-general-text">@{props.login}</h4>
      </div>
    </div>
  );
}

ShortProfile.propTypes = {
  profileImage: PropTypes.string,
  username: PropTypes.string.isRequired,
  login: PropTypes.string.isRequired,
};

export default ShortProfile;
