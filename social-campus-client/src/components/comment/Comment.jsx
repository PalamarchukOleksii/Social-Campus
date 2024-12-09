import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";
import DateTime from "../dateTime/DateTime";
import { IoCreateOutline, IoCreate } from "react-icons/io5";
import login from "../../utils/consts/AuthUserLogin";
import userData from "../../data/userData.json";
import { useCreateItem } from "../../context/CreateItemContext";
import CreateComment from "../createComment/CreateComment";

function Comment(props) {
  const [isEditHovered, setIsEditHovered] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [commentText, setCommentText] = useState(props.text);
  const [loading, setLoading] = useState(true);

  const { closeCreateComment, openCreateComment } = useCreateItem();

  useEffect(() => {
    const fetchData = () => {
      const user = userData.find((user) => user.login === login);
      setCurrentUser(user);
      setLoading(false);
    };

    fetchData();
  }, []);

  const handleCreateCommentOpenClick = () => {
    openCreateComment();
    setIsEditOpen((prev) => !prev);
  };

  const handleCreateCommentCloseClick = () => {
    closeCreateComment();
    setIsEditOpen((prev) => !prev);
  };

  if (loading) {
    return <></>;
  }

  return (
    <div className="comment-container">
      {isEditOpen && (
        <div className="create-comment-modal-overlay">
          <CreateComment
            user={currentUser}
            text={commentText}
            setText={setCommentText}
            onCloseClick={handleCreateCommentCloseClick}
            addGoBack={true}
            isForEdit={true}
          />
        </div>
      )}
      <div className="comment-info">
        <div className="user-info">
          <div className="commenter-info">
            <ShortProfile
              username={props.username}
              login={props.login}
              profileImage={props.profileImage}
            />
            <DateTime dateTime={props.creationTime} locale="en-US" />
          </div>
          {currentUser.login === props.login && (
            <div
              className="edit-comment-icon general-text"
              onMouseEnter={() => setIsEditHovered(true)}
              onMouseLeave={() => setIsEditHovered(false)}
              onClick={handleCreateCommentOpenClick}
            >
              {isEditHovered ? <IoCreate /> : <IoCreateOutline />}
            </div>
          )}
        </div>
        <h2 className="comment-text">{commentText}</h2>
      </div>
      <div className="comment-interactions">
        <InteractionItem
          label={props.likeCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
        />
      </div>
    </div>
  );
}

Comment.propTypes = {
  username: PropTypes.string.isRequired,
  login: PropTypes.string.isRequired,
  profileImage: PropTypes.string.isRequired,
  text: PropTypes.string.isRequired,
  likeCount: PropTypes.number.isRequired,
  creationTime: PropTypes.string.isRequired,
};

export default Comment;
