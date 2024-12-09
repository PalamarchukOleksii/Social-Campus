import React from "react";
import PropTypes from "prop-types";
import { useNavigate } from "react-router-dom";
import "./Tag.css";

function Tag(props) {
  const navigate = useNavigate();

  const handleTagClick = () => {
    const tagWithoutHash = props.tagName.replace("#", "");
    navigate(`/tag/${tagWithoutHash}`);
    window.scrollTo(0, 0);
  };

  return (
    <div className="tag-container" onClick={handleTagClick}>
      <h2 className="tag-name genetal-text">{props.tagName}</h2>
      <h3 className="posts-count not-general-text">
        {props.postsCount.toLocaleString()} posts
      </h3>
    </div>
  );
}

Tag.propTypes = {
  tagName: PropTypes.string.isRequired,
  postsCount: PropTypes.number.isRequired,
};

export default Tag;
