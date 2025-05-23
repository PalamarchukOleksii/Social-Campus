import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5";
import "./CreateComment.css";

function CreateComment(props) {
  const [commentText, setCommentText] = useState(
    props.isForEdit ? props.text || "" : ""
  );
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [authUser, setAuthUser] = useState();
  const [loading, setLoading] = useState(true);

  useEffect(() => {}, []);

  const handleInputChange = (e) => {
    setCommentText(e.target.value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (commentText.trim()) {
      if (props.isForEdit) {
        props.setText(commentText);
      } else {
        const newComment = {
          id: props.getMaxCommentId() + 1,
          text: commentText,
          creationTime: new Date().toISOString(),
          username: authUser.username,
          login: authUser.login,
          profileImage: authUser.profileImage,
          likeCount: 0,
        };

        props.setComments([newComment, ...props.comments]);
      }

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

  if (loading) {
    return <></>;
  }

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
        username={authUser.username}
        login={authUser.login}
        profileImage={authUser.profileImage}
        redirectOnClick={false}
      />
      <form
        className="create-form"
        onSubmit={
          props.onSubmit ? () => props.onSubmit(commentText) : handleSubmit
        }
      >
        <textarea
          className="comment-text"
          type="text"
          placeholder="Write your comment..."
          value={commentText}
          onChange={handleInputChange}
          required
        />
        <div className="controles">
          <button className="publish-button" type="submit">
            {props.isForEdit ? "Save Changes" : "Comment"}
          </button>
        </div>
      </form>
    </div>
  );
}

CreateComment.propTypes = {
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
  ),
  setComments: PropTypes.func,
  getMaxCommentId: PropTypes.func,
  onCloseClick: PropTypes.func,
  addGoBack: PropTypes.bool,
  text: PropTypes.string,
  setText: PropTypes.func,
  isForEdit: PropTypes.bool,
  onSubmit: PropTypes.func,
};

export default CreateComment;
