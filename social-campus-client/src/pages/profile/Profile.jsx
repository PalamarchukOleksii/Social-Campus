import React from "react";
import { Link } from "react-router-dom";
import "./Profile.css";
import userData from "../../data/userData.json";

function Profile() {
  return (
    <div className="profile">
      <div className="profile-header">
        <img
          src="/default-profile.png"
          alt="Profile"
          className="profile-image"
        />
      </div>
      <div className="profile-info">
        <h2 className="profile-name general-text">{userData.username}</h2>
        <p className="profile-login not-general-text">{userData.login}</p>
        <p className="profile-bio general-text">{userData.bio}</p>
      </div>
      <div className="profile-stats">
        <Link to="/followers">
          <span>{userData.followers} Followers</span>
        </Link>
        <Link to="/following">
          <span>{userData.following} Following</span>
        </Link>
      </div>
    </div>
  );
}

export default Profile;
