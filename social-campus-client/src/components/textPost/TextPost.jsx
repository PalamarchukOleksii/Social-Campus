// textPost/TextPost.jsx
import React from "react";
import "./TextPost.css";

const TextPost = ({ name, login, bio }) => (
  <div className="profile-info">
    <h2 className="profile-name">{name}</h2>
    <p className="profile-username">{login}</p>
    <p className="profile-bio">{bio}</p>
  </div>
);

export default TextPost;
