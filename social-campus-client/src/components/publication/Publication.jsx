import React from "react";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";

function Publication(props) {
  return (
    <div className="publication-container">
      <div className="short-info-container">
        <ShortProfile />
        <h4 className="creation-time not-general-text">
          {props.creationTime || "Time of creation"}
        </h4>
      </div>
      <div className="content-container">
        <h2 className="description general-text">
          {props.description || "Description"}
        </h2>
        <div className="image-wrapper">
          {props.imageUrl && <img src={props.imageUrl} alt="Publication" />}
        </div>
      </div>
      <div className="interaction-stat">
        <InteractionItem
          label={props.likesCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
        />
        <InteractionItem
          label={props.commentsCount}
          icon={InteractionItems.commentIcon}
        />
      </div>
    </div>
  );
}

Publication.propTypes = {
  description: PropTypes.string.isRequired,
  imageUrl: PropTypes.string,
  creationTime: PropTypes.string.isRequired,
  likesCount: PropTypes.number.isRequired,
  commentsCount: PropTypes.number.isRequired,
};

export default Publication;
