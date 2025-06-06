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
const GET_COMMENT_URL = "/api/comments/";
const REPLY_TO_COMMENT_URL = "/api/comments/reply";

function CreateComment(props) {
  const { auth } = useAuth();
  const axios = useAxiosPrivate();

  const [commentText, setCommentText] = useState("");
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState({});

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setUser(currentUser);
    setLoading(false);
  }, [auth]);

  useEffect(() => {
    const fetchComment = async () => {
      try {
        const response = await axios.get(
          `${GET_COMMENT_URL}${props.editCommentId}`
        );
        const comment = response.data;
        setCommentText(comment.description || "");
      } catch (error) {
        console.error("Fetching comment error:", error);
        toast.error("Failed to fetch the comment data.");
      }
    };

    if (props.isForEdit && props.editCommentId) {
      fetchComment();
    }
  }, [props.isForEdit, props.editCommentId, axios]);

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
        creatorId: { value: user.id.value },
      };

      if (props.isForEdit && props.editCommentId) {
        payload.commentId = { value: props.editCommentId };
        payload.callerId = { value: user.id.value };

        await axios.patch(UPDATE_COMMENT_URL, payload);
        toast.success("Comment updated successfully.");
      } else if (props.replyToCommentId) {
        payload.replyToCommentId = { value: props.replyToCommentId };
        payload.publicationId = { value: props.publicationId };

        await axios.post(REPLY_TO_COMMENT_URL, payload);
        toast.success("Reply posted successfully.");
      } else {
        payload.publicationId = { value: props.publicationId };

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
      <ShortProfile userId={user.id.value} />
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
  editCommentId: PropTypes.string,
  onCloseClick: PropTypes.func,
  isForEdit: PropTypes.bool,
  addGoBack: PropTypes.bool,
  replyToCommentId: PropTypes.string,
};

export default CreateComment;
