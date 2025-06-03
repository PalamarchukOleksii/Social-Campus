import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import "./CreateComment.css";

const CREATE_COMMENT_URL = "/api/comments/create";
const UPDATE_COMMENT_URL = "/api/comments/update";

function CreateComment(props) {
  const { auth } = useAuth();
  const axios = useAxiosPrivate();

  const [commentText, setCommentText] = useState(
    props.isForEdit ? props.text || "" : ""
  );
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState({});

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setUser(currentUser);
    setLoading(false);
  }, [auth]);

  const handleInputChange = (e) => {
    setCommentText(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!commentText.trim()) {
      toast.error("Comment text cannot be empty.");
      return;
    }

    try {
      const payload = {
        description: commentText,
      };

      if (props.isForEdit && props.commentId) {
        payload.commentId = { value: props.commentId };
        payload.callerId = { value: user.id.value };

        await axios.patch(UPDATE_COMMENT_URL, payload);
        toast.success("Comment updated successfully.");
        if (props.setText) props.setText(commentText);
      } else {
        payload.publicationId = { value: props.publicationId };
        payload.creatorId = { value: user.id.value };

        await axios.post(CREATE_COMMENT_URL, payload);
        toast.success("Comment posted successfully.");
      }

      setCommentText("");
      if (props.onCloseClick) props.onCloseClick();
    } catch (error) {
      const { response } = error;
      if (response?.data?.error) {
        response.data.error.forEach((err) => toast.error(err.message));
      } else if (response?.data?.detail) {
        toast.error(response.data.detail);
      } else {
        console.error(error);
        toast.error("An unexpected error occurred.");
      }
    }
  };

  const closeCreateComment = () => {
    setCommentText("");
    if (props.onCloseClick) props.onCloseClick();
  };

  if (loading) return null;

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
        username={user.firstName + " " + user.lastName}
        login={user.login}
        profileImage={user.profileImage}
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
  publicationId: PropTypes.string,
  commentId: PropTypes.string,
  onCloseClick: PropTypes.func,
  isForEdit: PropTypes.bool,
  text: PropTypes.string,
  setText: PropTypes.func,
  addGoBack: PropTypes.bool,
};

export default CreateComment;
