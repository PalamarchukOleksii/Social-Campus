import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5";
import "./CreateComment.css";
import login from "../../utils/consts/AuthUserLogin";
import userData from "../../data/userData.json";

function CreateComment(props) {
  const [commentText, setCommentText] = useState("");
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [authUser, setAuthUser] = useState();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);
      setAuthUser(foundUser || null);
      setLoading(false);
    };

    fetchUserData();
  }, []);

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
        username: authUser.username,
        login: authUser.login,
        profileImage: authUser.profileImage,
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
