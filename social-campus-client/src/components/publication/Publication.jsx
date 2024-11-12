import React from "react";
import { useNavigate, useLocation } from "react-router-dom";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";
import DateTime from "../dateTime/DateTime";

function Publication(props) {
  const navigate = useNavigate();
  const location = useLocation();

  const handlePublicationClick = () => {
    if (location.pathname !== `/publication/${props.publicationId}`) {
      navigate(`/publication/${props.publicationId}`);
    }
  };

  return (
    <div className="publication-container">
      <div className="short-info-container">
        <ShortProfile
          username={props.username}
          login={props.login}
          profileImage={props.profileImage}
        />
        <DateTime dateTime={props.creationTime} locale="en-US" />
      </div>
      <div className="content-container" onClick={handlePublicationClick}>
        <h2 className="description general-text">
          {props.description || "Description"}
        </h2>
        <div className="image-wrapper">
          {props.imageUrl && <img src={props.imageUrl} alt="Publication" />}
        </div>
      </div>
      <div className="interaction-stat">
        <InteractionItem
          itemType="like"
          label={props.likesCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
        />
        <InteractionItem
          itemType="comment"
          label={props.commentsCount}
          icon={InteractionItems.commentIcon}
        />
      </div>
    </div>
  );
}

Publication.propTypes = {
  publicationId: PropTypes.number.isRequired,
  description: PropTypes.string.isRequired,
  imageUrl: PropTypes.string,
  creationTime: PropTypes.string.isRequired,
  likesCount: PropTypes.number.isRequired,
  commentsCount: PropTypes.number.isRequired,
  profileImage: PropTypes.string,
  username: PropTypes.string.isRequired,
  login: PropTypes.string.isRequired,
};

export default Publication;
