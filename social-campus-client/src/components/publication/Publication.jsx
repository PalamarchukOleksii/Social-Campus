import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";
import DateTime from "../dateTime/DateTime";
import CreateComment from "../createComment/CreateComment";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";
import { IoCreateOutline, IoCreate } from "react-icons/io5";

function Publication({ publication, addCreateOpen = true }) {
  const navigate = useNavigate();
  const location = useLocation();
  const [comments, setComments] = useState([]);
  const [currentUser, setCurrentUser] = useState(null);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [publicationImg, setPublicationImg] = useState(publication.imageData);
  const [publicationDescription, setPublicationDescription] = useState(
    publication.description
  );
  const [isEditHovered, setIsEditHovered] = useState(false);

  useEffect(() => {}, []);

  const handlePublicationClick = () => {
    if (location.pathname !== `/publication/${publication.id.value}`) {
      navigate(`/publication/${publication.id.value}`);
    }
  };

  const handleCreateCommentOpenClick = () => {
    if (addCreateOpen) {
      setIsCreateOpen((prev) => !prev);
    }
  };

  const handleCreateCommentCloseClick = () => {
    setIsCreateOpen((prev) => !prev);
  };

  return (
    <div className="publication-container">
      {isCreateOpen && (
        <div className="create-comment-modal-overlay">
          <CreateComment
            user={currentUser}
            comments={comments}
            setComments={setComments}
            getMaxCommentId={getMaxCommentId}
            onCloseClick={handleCreateCommentCloseClick}
            addGoBack={true}
          />
        </div>
      )}
      <div>
        <div className="short-info-container">
          <div className="creator-info">
            <ShortProfile
              username={
                publication.creatorInfo.firstName +
                " " +
                publication.creatorInfo.lastName
              }
              login={publication.creatorInfo.login}
              profileImage={publication.creatorInfo.profileImageData}
            />
            <DateTime
              dateTime={publication.creationDateTime.split(".")[0]}
              locale="en-US"
            />
          </div>
          {currentUser?.login === publication.creatorInfo.login && (
            <div
              className="edit-pub-icon general-text"
              onMouseEnter={() => setIsEditHovered(true)}
              onMouseLeave={() => setIsEditHovered(false)}
              onClick={() => {}}
            >
              {isEditHovered ? <IoCreate /> : <IoCreateOutline />}
            </div>
          )}
        </div>
        <div className="content-container" onClick={handlePublicationClick}>
          <h2 className="description general-text">
            {publicationDescription || "Description"}
          </h2>
          <div className="image-wrapper">
            {publicationImg && <img src={publicationImg} alt="Publication" />}
          </div>
        </div>
        <div className="interaction-stat">
          <InteractionItem
            itemType="like"
            label={publication.userWhoLikedIds.length}
            icon={InteractionItems.likeIcon}
            activeIcon={InteractionItems.activeLikeIcon}
            className="like-element"
          />
          <InteractionItem
            itemType="comment"
            label={publication.commentsCount}
            icon={InteractionItems.commentIcon}
            onClick={handleCreateCommentOpenClick}
            className="comment-element"
          />
        </div>
      </div>
    </div>
  );
}

Publication.propTypes = {
  publication: PropTypes.shape({
    id: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    description: PropTypes.string.isRequired,
    imageData: PropTypes.string,
    creationDateTime: PropTypes.string.isRequired,
    creatorInfo: PropTypes.shape({
      id: PropTypes.shape({
        value: PropTypes.string.isRequired,
      }).isRequired,
      login: PropTypes.string.isRequired,
      firstName: PropTypes.string,
      lastName: PropTypes.string,
      bio: PropTypes.string,
      profileImageData: PropTypes.string,
    }).isRequired,
    userWhoLikedIds: PropTypes.arrayOf(PropTypes.string),
    commentsCount: PropTypes.number.isRequired,
  }).isRequired,
  addCreateOpen: PropTypes.bool,
};

export default Publication;
