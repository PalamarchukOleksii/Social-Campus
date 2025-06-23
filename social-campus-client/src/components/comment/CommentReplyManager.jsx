import React, { useState } from "react";
import { createPortal } from "react-dom";
import CreateComment from "../createComment/CreateComment";
import Comment from "../comment/Comment";
import PropTypes from "prop-types";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const COMMENT_BASE_URL = "/api/comments";
const GET_REPLIES_URL = "/api/comments/replies";
const PAGE_SIZE = 10;

function CommentReplyManager({ comment, onCommentDelete }) {
  const [currentComment, setCurrentComment] = useState(comment);
  const [replies, setReplies] = useState([]);
  const [isReplying, setIsReplying] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [isDeleted, setIsDeleted] = useState(false);

  const [replyPage, setReplyPage] = useState(1);
  const [loadingReplies, setLoadingReplies] = useState(false);
  const [allRepliesFetched, setAllRepliesFetched] = useState(false);
  const [repliesLoaded, setRepliesLoaded] = useState(false);

  const axios = useAxiosPrivate();

  if (isDeleted) {
    return null;
  }

  const fetchCommentById = async (commentId) => {
    try {
      const response = await axios.get(`${COMMENT_BASE_URL}/${commentId}`);
      return response.data;
    } catch (error) {
      console.error("Failed to fetch comment:", error);
      return null;
    }
  };

  const fetchReplies = async () => {
    if (allRepliesFetched) return;

    try {
      setLoadingReplies(true);
      const response = await axios.get(
        `${GET_REPLIES_URL}/${currentComment.id.value}/count/${PAGE_SIZE}/page/${replyPage}`
      );

      const newReplies = response.data;

      if (newReplies.length < PAGE_SIZE) {
        setAllRepliesFetched(true);
      }

      if (newReplies.length === 0) return;

      setReplies((prev) => [...prev, ...newReplies]);
      setReplyPage((prev) => prev + 1);
      setRepliesLoaded(true);
    } catch (error) {
      console.error("Failed to fetch replies:", error);
    } finally {
      setLoadingReplies(false);
    }
  };

  const handleLoadRepliesClick = () => {
    if (!repliesLoaded) {
      setReplies([]);
      setReplyPage(1);
      setAllRepliesFetched(false);
    }
    fetchReplies();
  };

  const handleEditClose = async () => {
    setIsEditing(false);
    const updatedComment = await fetchCommentById(currentComment.id.value);
    if (updatedComment) {
      setCurrentComment(updatedComment);
    }
  };

  const onDelete = async () => {
    setIsEditing(false);
    setIsReplying(false);
    setIsDeleted(true);

    if (onCommentDelete) {
      onCommentDelete(currentComment.id.value);
    }
  };

  const handleReplyDelete = (deletedReplyId) => {
    setReplies((prev) =>
      prev.filter((reply) => (reply.id.value || reply.id) !== deletedReplyId)
    );
  };

  return (
    <div className="comment-reply-manager">
      <Comment
        comment={currentComment}
        onReplyClick={() => setIsReplying(true)}
        onEditClick={() => setIsEditing(true)}
      />

      {isReplying &&
        createPortal(
          <div className="create-comment-modal-overlay">
            <CreateComment
              publicationId={currentComment.publicationId.value}
              replyToCommentId={currentComment.id.value}
              onCloseClick={() => setIsReplying(false)}
              addGoBack={true}
            />
          </div>,
          document.body
        )}

      {isEditing &&
        createPortal(
          <div className="edit-publication-modal-overlay">
            <CreateComment
              editCommentId={currentComment.id.value}
              isForEdit={true}
              onCloseClick={handleEditClose}
              onDelete={onDelete}
              addGoBack={true}
            />
          </div>,
          document.body
        )}

      {!repliesLoaded && currentComment.repliesCount > 0 && (
        <div className="load-more-container">
          <a
            className="load-replies-button"
            onClick={handleLoadRepliesClick}
            disabled={loadingReplies}
          >
            Load Replies
          </a>
        </div>
      )}

      {replies.length > 0 && (
        <div className="replies-section">
          {replies.map((reply) => (
            <CommentReplyManager
              key={reply.id.value || reply.id}
              comment={reply}
              onCommentDelete={handleReplyDelete}
            />
          ))}
        </div>
      )}

      {repliesLoaded && !allRepliesFetched && (
        <div className="load-more-container">
          <button onClick={fetchReplies} disabled={loadingReplies}>
            Load More
          </button>
        </div>
      )}
    </div>
  );
}

CommentReplyManager.propTypes = {
  comment: PropTypes.shape({
    id: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    description: PropTypes.string.isRequired,
    creationDateTime: PropTypes.string.isRequired,
    creatorId: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    publicationId: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    userWhoLikedIds: PropTypes.arrayOf(
      PropTypes.shape({
        value: PropTypes.string.isRequired,
      })
    ),
    repliesCount: PropTypes.number.isRequired,
  }).isRequired,
  onCommentDelete: PropTypes.func,
};

export default CommentReplyManager;
