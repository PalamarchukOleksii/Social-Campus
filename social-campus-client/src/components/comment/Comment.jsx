import React, { useState } from "react";
import PropTypes from "prop-types";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";
import DateTime from "../dateTime/DateTime";

function Comment(props) {
  const [isEditing, setIsEditing] = useState(false); // Режим редактирования
  const [commentText, setCommentText] = useState(props.text); // Текущий текст комментария

  const handleEditToggle = () => {
    setIsEditing(!isEditing);
  };

  const handleTextChange = (event) => {
    setCommentText(event.target.value);
  };

  const handleSave = () => {
    setIsEditing(false);
  };

  return (
    <div className="comment-container">
      <div className="comment-info">
        <div className="user-info">
          <ShortProfile
            username={props.username}
            login={props.login}
            profileImage={props.profileImage}
          />
          <DateTime dateTime={props.creationTime} locale="en-US" />
        </div>
        {isEditing ? (
          <div className="edit-container">
            <textarea
              className="edit-textarea"
              value={commentText}
              onChange={handleTextChange}
            />
            <button onClick={handleSave} className="save-button">Save</button>
            <button onClick={handleEditToggle} className="cancel-button">Cancel</button>
          </div>
        ) : (
          <h2 className="comment-text">{commentText}</h2>
        )}
      </div>
      <div className="comment-interactions">
        <InteractionItem
          label={props.likeCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
        />
        <button onClick={handleEditToggle} className="edit-button">
          {isEditing ? "Editing" : "Edit"}
        </button>
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
