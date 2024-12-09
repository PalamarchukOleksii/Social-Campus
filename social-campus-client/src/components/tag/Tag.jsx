import React from "react";
import PropTypes from "prop-types";
import "./Tag.css";

function Tag({ tagName, postsCount }) {
  return (
    <div className="tag-container">
      <h2 className="tag-name genetal-text">{tagName}</h2>
      <h3 className="posts-count not-general-text">
        {postsCount.toLocaleString()} posts
      </h3>
    </div>
  );
}

Tag.propTypes = {
  tagName: PropTypes.string.isRequired,
  postsCount: PropTypes.number.isRequired,
};

export default Tag;
