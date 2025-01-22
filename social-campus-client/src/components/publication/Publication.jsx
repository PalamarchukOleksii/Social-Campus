import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";
import DateTime from "../dateTime/DateTime";
import { useCreateItem } from "../../context/CreateItemContext";
import CreateComment from "../createComment/CreateComment";
import login from "../../utils/consts/AuthUserLogin";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";
import userData from "../../data/userData.json";
import CreatePublication from "../../components/createPublication/CreatePublication";
import { IoCreateOutline, IoCreate } from "react-icons/io5";

function Publication({ publication, addCreateOpen = true }) {
  const navigate = useNavigate();
  const location = useLocation();
  const [comments, setComments] = useState([]);
  const [currentUser, setCurrentUser] = useState(null);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [publicationImgUrl, setPublicationImgUrl] = useState(
    publication.imageUrl
  );
  const [publicationDescription, setPublicationDescription] = useState(
    publication.description
  );
  const [isEditHovered, setIsEditHovered] = useState(false);
  const [loading, setLoading] = useState(true);
  const [isEditOpen, setIsEditOpen] = useState(false);

  const { closeCreateComment, openCreateComment } = useCreateItem();

  useEffect(() => {
    const fetchData = () => {
      const user = userData.find((user) => user.login === login);
      setCurrentUser(user);
      setComments(publication.comments);
      setLoading(false);
    };

    fetchData();
  }, [publication.comments]);

  const handlePublicationClick = () => {
    if (location.pathname !== `/publication/${publication.id}`) {
      navigate(`/publication/${publication.id}`);
    }
  };

  const handleCreateCommentOpenClick = () => {
    if (addCreateOpen) {
      openCreateComment();
      setIsCreateOpen((prev) => !prev);
    }
  };

  const handleCreateCommentCloseClick = () => {
    closeCreateComment();
    setIsCreateOpen((prev) => !prev);
  };

  const handleEditPublicationOpenClick = () => {
    document.body.classList.add("no-scroll");
    setIsEditOpen((prev) => !prev);
  };

  const handleEditPublicationCloseClick = () => {
    document.body.classList.remove("no-scroll");
    setIsEditOpen((prev) => !prev);
  };

  const handleTagClick = (e, tagName) => {
    e.stopPropagation();
    const tagWithoutHash = tagName.replace("#", "");

    const currentPath = location.pathname;
    if (currentPath !== `/tag/${tagWithoutHash}`) {
      navigate(`/tag/${tagWithoutHash}`);
      window.scrollTo(0, 0);
    }
  };

  const renderDescriptionWithTags = (description) => {
    const regex = /#\w+/g;
    const parts = description.split(regex);
    const tags = description.match(regex);

    return (
      <span>
        {parts.map((part, index) => (
          <React.Fragment key={index}>
            {part}
            {tags && tags[index] && (
              <span
                className="tag"
                onClick={(e) => handleTagClick(e, tags[index])}
              >
                {tags[index]}
              </span>
            )}
          </React.Fragment>
        ))}
      </span>
    );
  };

  if (loading) {
    return <></>;
  }

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
      {isEditOpen && (
        <div className="edit-publication-modal-overlay">
          <CreatePublication
            publicationImgUrl={publicationImgUrl}
            setPublicationImgUrl={setPublicationImgUrl}
            publicationDescription={publicationDescription}
            setPublicationDescription={setPublicationDescription}
            close={handleEditPublicationCloseClick}
            isForEdit={true}
          />
        </div>
      )}
      <div>
        <div className="short-info-container">
          <div className="creator-info">
            <ShortProfile
              username={publication.username}
              login={publication.login}
              profileImage={publication.profileImage}
            />
            <DateTime dateTime={publication.creationTime} locale="en-US" />
          </div>
          {currentUser.login === publication.login && (
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
            {renderDescriptionWithTags(publicationDescription) || "Description"}
          </h2>
          <div className="image-wrapper">
            {publicationImgUrl && (
              <img src={publicationImgUrl} alt="Publication" />
            )}
          </div>
        </div>
        <div className="interaction-stat">
          <InteractionItem
            itemType="like"
            label={publication.likesCount}
            icon={InteractionItems.likeIcon}
            activeIcon={InteractionItems.activeLikeIcon}
            className="like-element"
          />
          <InteractionItem
            itemType="comment"
            label={publication.comments.length}
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
  addCreateOpen: PropTypes.bool,
};

export default Publication;
