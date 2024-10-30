import React from "react";
import "./Profile.css";

function Profile() {
  return (
    <div className="profile">
      <div className="profile-header">
        <img
          src="https://via.placeholder.com/150"
          alt="Profile"
          className="profile-image"
        />
        <div className="profile-info">
          <h2 className="profile-name">Username</h2>
          <p className="profile-username">@username</p>
          <p className="profile-bio">Write a few words about yourself</p>
        </div>
      </div>
      <div className="profile-stats">
        <span>Posts: 100</span>
        <span>Subscribers: 200</span>
        <span>Subscriptions: 180</span>
      </div>
      <div className="profile-tweets">
        <h3>Posts</h3>
        <div className="tweet"></div>
      </div>
    </div>
  );
}

export default Profile;
