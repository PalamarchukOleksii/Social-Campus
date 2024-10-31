import React from "react";
import "./Profile.css";
import userData from "../../data/userProfile.json";

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
        <span>Followers: {userData.followers}</span>
        <span>Following: {userData.following}</span>
      </div>
      <div className="profile-tweets">
        <h3>Posts</h3>
        <div className="tweet"></div>
      </div>
    </div>
  );
}

export default Profile;
