import React from "react";
import "./ShortProfile.css";
import userData from "../../data/userData.json";

function UserProfile() {
  return (
    <div className="short-info">
      <img
        src={userData.profileImage}
        alt="Profile"
        className="profile-image"
      />
      <div className="profile-info">
        <h3 className="general-text">{userData.username}</h3>
        <h4 className="not-general-text">{userData.login}</h4>
      </div>
    </div>
  );
}

export default UserProfile;
