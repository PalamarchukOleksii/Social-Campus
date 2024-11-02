import React from "react";
import "./PicturePost.css";

const PicturePost = ({ src, alt }) => (
  <img src={src} alt={alt} className="profile-image" />
);

export default PicturePost;
