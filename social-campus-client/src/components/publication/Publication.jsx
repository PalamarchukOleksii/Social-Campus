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
    if (location.pathname !== `/publication/${props.publication.id}`) {
      navigate(`/publication/${props.publication.id}`);
    }
  };

  return (
    <div className="publication-container">
      <div className="short-info-container">
        <ShortProfile
          username={props.publication.username}
          login={props.publication.login}
          profileImage={props.publication.profileImage}
        />
        <DateTime dateTime={props.publication.creationTime} locale="en-US" />
      </div>
      <div className="content-container" onClick={handlePublicationClick}>
        <h2 className="description general-text">
          {props.publication.description || "Description"}
        </h2>
        <div className="image-wrapper">
          {props.publication.imageUrl && (
            <img src={props.publication.imageUrl} alt="Publication" />
          )}
        </div>
      </div>
      <div className="interaction-stat">
        <InteractionItem
          itemType="like"
          label={props.publication.likesCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
        />
        <InteractionItem
          itemType="comment"
          label={props.publication.comments.length}
          icon={InteractionItems.commentIcon}
        />
      </div>
    </div>
  );
}

Publication.propTypes = {
  publication: PropTypes.shape({
    id: PropTypes.number.isRequired,
    description: PropTypes.string.isRequired,
    imageUrl: PropTypes.string,
    creationTime: PropTypes.string.isRequired,
    likesCount: PropTypes.number.isRequired,
    comments: PropTypes.array.isRequired,
    profileImage: PropTypes.string,
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
  }).isRequired,
};

export default Publication;
