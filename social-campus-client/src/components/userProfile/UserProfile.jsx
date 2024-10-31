import React from "react";
import { IoExit, IoExitOutline } from "react-icons/io5";
import "./UserProfile.css";
import PropTypes from "prop-types";

function UserProfile(props) {
  return (
    <div className="logout">
      <img
        src="https://via.placeholder.com/150"
        alt="Profile"
        className="profile-image"
      />
      <div className="profile-info">
        <h3 className="general-text">Username</h3>
        <h4 className="general-text">@login</h4>
      </div>
      <div
        onClick={props.handleLogout}
        className="logout-icon"
        onMouseEnter={() => props.setHoveredIcon("logout")}
        onMouseLeave={() => props.setHoveredIcon(null)}
      >
        {props.hoveredIcon === "logout" ? (
          <IoExit className="exit-icon" />
        ) : (
          <IoExitOutline className="exit-icon" />
        )}
      </div>
    </div>
  );
}

UserProfile.propTypes = {
  handleLogout: PropTypes.func.isRequired,
  setHoveredIcon: PropTypes.func.isRequired,
  hoveredIcon: PropTypes.string.isRequired,
};

export default UserProfile;
