import React from "react";
import { Link } from "react-router-dom"; // Import Link from react-router-dom
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
        <div className="profile-info">
          <h2 className="profile-name">{userData.username}</h2>
          <p className="profile-username">{userData.login}</p>
          <p className="profile-bio">{userData.bio}</p>
        </div>
      </div>
      <div className="profile-stats">
        <span>Posts: {userData.postCount}</span>
        <Link to="/followers">
          <span>Followers: {userData.followers}</span>
        </Link>
        <Link to="/following">
          <span>Following: {userData.following}</span>
        </Link>
      </div>
      <div className="profile-tweets">
        <h3>Posts</h3>
        <div className="tweet"></div>
      </div>
    </div>
  );
}

export default Profile;
