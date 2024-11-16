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

function Publication(props) {
  const navigate = useNavigate();
  const location = useLocation();
  const [comments, setComments] = useState([]);
  const [currentUser, setCurrentUser] = useState(null);
  const [isCreateOpen, setIsCreateOpen] = useState(false);

  const { closeCreateComment, openCreateComment } = useCreateItem();

  useEffect(() => {
    const fetchData = () => {
      const user = userData.find((user) => user.login === login);
      setCurrentUser(user);

      setComments(props.publication.comments);
    };

    fetchData();
  }, [comments]);

  const handlePublicationClick = () => {
    if (location.pathname !== `/publication/${props.publication.id}`) {
      navigate(`/publication/${props.publication.id}`);
    }
  };

  const handleCreateCommentOpenClick = () => {
    openCreateComment();
    setIsCreateOpen((prev) => !prev);
  };

  const handleCreateCommentCloseClick = () => {
    closeCreateComment();
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
            onClick={handleCreateCommentOpenClick}
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
};

export default Publication;
