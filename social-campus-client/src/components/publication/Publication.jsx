import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { createPortal } from "react-dom";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";
import DateTime from "../dateTime/DateTime";
import CreateComment from "../createComment/CreateComment";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";
import { IoCreateOutline, IoCreate } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";
import CreatePublication from "../createPublication/CreatePublication";

function Publication(props) {
  const navigate = useNavigate();
  const location = useLocation();
  const { auth } = useAuth();

  const [comments, setComments] = useState([]);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [isEditHovered, setIsEditHovered] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);

  const handlePublicationClick = () => {
    if (location.pathname !== `/publication/${props.publication.id.value}`) {
      navigate(`/publication/${props.publication.id.value}`);
    }
  };

  const handleCreateCommentOpenClick = () => {
    setIsCreateOpen(true);
  };

  const handleCreateCommentCloseClick = () => {
    setIsCreateOpen(false);
  };

  const handleEditPublicationOpenClick = () => {
    setIsEditOpen(true);
  };

  const handleEditPublicationCloseClick = () => {
    setIsEditOpen(false);
  };

  return (
    <div className="publication-container">
      {isCreateOpen && (
        <div className="create-comment-modal-overlay">
          <CreateComment
            user={auth.shortUser}
            comments={comments}
            setComments={setComments}
            getMaxCommentId={getMaxCommentId}
            onCloseClick={handleCreateCommentCloseClick}
            addGoBack={true}
          />
        </div>
      )}
      {isEditOpen &&
        createPortal(
          <div className="edit-publication-modal-overlay">
            <CreatePublication
              onCloseClick={handleEditPublicationCloseClick}
              isForEdit={true}
              editPublicationId={props.publication.id.value}
            />
          </div>,
          document.body
        )}
      <div>
        <div className="short-info-container">
          <div className="creator-info">
            <ShortProfile
              username={
                props.publication.creatorInfo.firstName +
                " " +
                props.publication.creatorInfo.lastName
              }
              login={props.publication.creatorInfo.login}
              profileImage={props.publication.creatorInfo.profileImageData}
            />
            <DateTime
              dateTime={props.publication.creationDateTime.split(".")[0]}
              locale="en-US"
            />
          </div>
          {auth.shortUser?.login === props.publication.creatorInfo.login && (
            <div
              className="edit-pub-icon general-text"
              onMouseEnter={() => setIsEditHovered(true)}
              onMouseLeave={() => setIsEditHovered(false)}
              onClick={handleEditPublicationOpenClick}
            >
              {isEditHovered ? <IoCreate /> : <IoCreateOutline />}
            </div>
          )}
        </div>
        <div className="content-container" onClick={handlePublicationClick}>
          <h2 className="description general-text">
            {props.publication.description || "Description"}
          </h2>
          <div className="image-wrapper">
            {props.publication.imageData && (
              <img src={props.publication.imageData} alt="Publication" />
            )}
          </div>
        </div>
        <div className="interaction-stat">
          <InteractionItem
            itemType="like"
            label={props.publication.userWhoLikedIds.length}
            icon={InteractionItems.likeIcon}
            activeIcon={InteractionItems.activeLikeIcon}
            className="like-element"
          />
          <InteractionItem
            itemType="comment"
            label={props.publication.commentsCount}
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
};

export default Publication;
