import React, { useState } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5";
import "./CreateComment.css";

function CreateComment(props) {
  const [commentText, setCommentText] = useState("");
  const [isExitHovered, setIsExitHovered] = useState(false);

  const handleInputChange = (e) => {
    setCommentText(e.target.value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (commentText.trim()) {
      const newComment = {
        id: props.getMaxCommentId() + 1,
        text: commentText,
        creationTime: new Date().toISOString(),
        username: props.user.username,
        login: props.user.login,
        profileImage: props.user.profileImage,
        likeCount: 0,
      };

      props.setComments([newComment, ...props.comments]);

      setCommentText("");
      if (props.addGoBack) {
        props.onCloseClick();
      }
    } else {
      toast("Comment text cannot be empty.");
    }
  };

  const closeCreateComment = () => {
    setCommentText("");
    if (props.addGoBack) {
      props.onCloseClick();
    }
  };

  return (
    <div className={`create-comment ${props.addGoBack ? "go-back" : ""}`}>
      {props.addGoBack && (
        <div
          className="close-creation-icon general-text"
          onMouseEnter={() => setIsExitHovered(true)}
          onMouseLeave={() => setIsExitHovered(false)}
          onClick={closeCreateComment}
        >
          {isExitHovered ? <IoArrowBackCircle /> : <IoArrowBackCircleOutline />}
          <span className="general-text back-text">Back</span>
        </div>
      )}
      <ShortProfile
        username={props.user.username}
        login={props.user.login}
        profileImage={props.user.profileImage}
        redirectOnClick={false}
      />
      <form className="create-form" onSubmit={handleSubmit}>
        <textarea
          className="comment-text"
          type="text"
          placeholder="Write your comment..."
          value={commentText}
          onChange={handleInputChange}
          required
          autoFocus
        />
        <div className="controles">
          <button className="publish-button" type="submit">
            Comment
          </button>
        </div>
      </form>
    </div>
  );
}

CreateComment.propTypes = {
  user: PropTypes.shape({
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
    profileImage: PropTypes.string,
  }).isRequired,
  comments: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      text: PropTypes.string,
      creationTime: PropTypes.string,
      username: PropTypes.string,
      login: PropTypes.string,
      profileImage: PropTypes.string,
      likeCount: PropTypes.number,
    })
  ).isRequired,
  setComments: PropTypes.func.isRequired,
  getMaxCommentId: PropTypes.func.isRequired,
  onCloseClick: PropTypes.func,
  addGoBack: PropTypes.bool,
};

CreateComment.defaultProps = {
  addGoBack: false,
};

export default CreateComment;
