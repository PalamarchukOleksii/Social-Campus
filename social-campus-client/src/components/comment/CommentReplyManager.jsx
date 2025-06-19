import React, { useState } from "react";
import { createPortal } from "react-dom";
import CreateComment from "../createComment/CreateComment";
import Comment from "../comment/Comment";
import PropTypes from "prop-types";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const GET_REPLIES_URL = "/api/comments/replies/";
const PAGE_SIZE = 10;

function CommentReplyManager({ comment }) {
  const [replies, setReplies] = useState([]);
  const [isReplying, setIsReplying] = useState(false);
  const [isEditing, setIsEditing] = useState(false);

  const [replyPage, setReplyPage] = useState(1);
  const [loadingReplies, setLoadingReplies] = useState(false);
  const [allRepliesFetched, setAllRepliesFetched] = useState(false);
  const [repliesLoaded, setRepliesLoaded] = useState(false);

  const axios = useAxiosPrivate();

  const fetchReplies = async () => {
    if (allRepliesFetched) return;

    try {
      setLoadingReplies(true);
      const response = await axios.get(
        `${GET_REPLIES_URL}${comment.id.value}/count/${PAGE_SIZE}/page/${replyPage}`
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

  return (
    <div className="comment-reply-manager">
      <Comment
        comment={comment}
        onReplyClick={() => setIsReplying(true)}
        onEditClick={() => setIsEditing(true)}
      />

      {isReplying &&
        createPortal(
          <div className="create-comment-modal-overlay">
            <CreateComment
              publicationId={comment.publicationId.value}
              replyToCommentId={comment.id.value}
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
              editCommentId={comment.id.value}
              isForEdit={true}
              onCloseClick={() => setIsEditing(false)}
              addGoBack={true}
            />
          </div>,
          document.body
        )}

      {!repliesLoaded && comment.repliesCount > 0 && (
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
};

export default CommentReplyManager;
