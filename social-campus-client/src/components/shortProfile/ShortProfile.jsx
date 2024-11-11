import React from "react";
import PropTypes from "prop-types";
import "./ShortProfile.css";

function ShortProfile(props) {
  return (
    <div className="short-info">
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
