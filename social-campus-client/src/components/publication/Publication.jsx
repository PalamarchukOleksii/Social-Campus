import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { createPortal } from "react-dom";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";
import DateTime from "../dateTime/DateTime";
import CreateComment from "../createComment/CreateComment";
import { IoCreateOutline, IoCreate } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import CreatePublication from "../createPublication/CreatePublication";
import Loading from "../loading/Loading";

const REMOVE_LIKE_URL = "/api/publicationlikes/remove/";
const ADD_LIKE_URL = "/api/publicationlikes/add";

function Publication({ publicationId, disableCreateComment }) {
  const navigate = useNavigate();
  const location = useLocation();
  const { auth } = useAuth();
  const axios = useAxiosPrivate();

  const [publication, setPublication] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isInitialLoad, setIsInitialLoad] = useState(true);

  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [isEditHovered, setIsEditHovered] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [isLiked, setIsLiked] = useState(false);
  const [likeCount, setLikeCount] = useState(0);

  const fetchPublication = async () => {
    if (!publicationId) return;

    if (!publication) {
      setLoading(true);
    }

    try {
      const res = await axios.get(`/api/publications/${publicationId}`);
      const pub = res.data;
      setPublication(pub);

      const userLikedIds =
        pub.userWhoLikedIds?.map((idObj) => idObj.value) || [];
      setIsLiked(userLikedIds.includes(auth.shortUser?.id?.value));
      setLikeCount(userLikedIds.length);

      setLoading(false);
      setIsInitialLoad(false);
    } catch (err) {
      console.error("Failed to fetch publication:", err);
      setLoading(false);
      setIsInitialLoad(false);
    }
  };

  useEffect(() => {
    fetchPublication();
  }, [publicationId, auth.shortUser?.id?.value, axios]);

  const handleLikeToggle = async () => {
    if (!publication) return;
    const userId = auth.shortUser?.id?.value;
    if (!userId) return;

    try {
      if (isLiked) {
        setIsLiked(false);
        setLikeCount((prev) => prev - 1);
        await axios.delete(
          `${REMOVE_LIKE_URL}${publication.id.value}/${userId}`
        );
      } else {
        setIsLiked(true);
        setLikeCount((prev) => prev + 1);
        await axios.post(ADD_LIKE_URL, {
          userId: { value: userId },
          publicationId: { value: publication.id.value },
        });
      }
    } catch (error) {
      console.error("Error toggling like:", error);
      setIsLiked((prev) => !prev);
      setLikeCount((prev) => (isLiked ? prev + 1 : prev - 1));
    }
  };

  const handlePublicationClick = () => {
    if (
      publication &&
      location.pathname !== `/publication/${publication.id.value}`
    ) {
      navigate(`/publication/${publication.id.value}`);
    }
  };

  const handleCreateCommentOpenClick = () => setIsCreateOpen(true);
  const handleCreateCommentCloseClick = () => setIsCreateOpen(false);

  const handleEditPublicationOpenClick = () => setIsEditOpen(true);
  const handleEditPublicationCloseClick = () => {
    setIsEditOpen(false);
    fetchPublication();
  };

  const handleTagClick = (e, tagName) => {
    e.stopPropagation();
    const tagWithoutHash = tagName.replace("#", "");
    if (location.pathname !== `/tag/${tagWithoutHash}`) {
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

  if (loading && isInitialLoad) {
    return <Loading />;
  }

  if (!publication) {
    return <div className="publication-container">No publication found.</div>;
  }

  return (
    <div className="publication-container">
      {isCreateOpen && !disableCreateComment && (
        <div className="create-comment-modal-overlay">
          <CreateComment
            publicationId={publication.id.value}
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
              editPublicationId={publication.id.value}
            />
          </div>,
          document.body
        )}

      <div>
        <div className="short-info-container">
          <div className="creator-info">
            <ShortProfile userId={publication.creatorId.value} />
            <DateTime
              dateTime={publication.creationDateTime.split(".")[0]}
              locale="en-US"
            />
          </div>

          {auth.shortUser?.id?.value === publication.creatorId.value && (
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
            {renderDescriptionWithTags(publication.description) ||
              "Description"}
          </h2>
          <div className="image-wrapper">
            {publication.imageUrl && (
              <img src={publication.imageUrl} alt="Publication" />
            )}
          </div>
        </div>

        <div className="interaction-stat">
          <InteractionItem
            itemType="like"
            label={likeCount}
            icon={InteractionItems.likeIcon}
            activeIcon={InteractionItems.activeLikeIcon}
            isActive={isLiked}
            onClick={handleLikeToggle}
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
  publicationId: PropTypes.string.isRequired,
  disableCreateComment: PropTypes.bool,
};

export default Publication;
