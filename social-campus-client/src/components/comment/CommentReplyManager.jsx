import React, { useState, useEffect } from "react";
import { createPortal } from "react-dom";
import CreateComment from "../createComment/CreateComment";
import Comment from "../comment/Comment";
import PropTypes from "prop-types";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const GET_REPLIES_URL = "/api/comments/replies/";

function CommentReplyManager({ comment }) {
  const [replies, setReplies] = useState([]);
  const [isReplying, setIsReplying] = useState(false);
  const [isEditing, setIsEditing] = useState(false);

  const axios = useAxiosPrivate();

  const fetchReplies = async () => {
    try {
      const response = await axios.get(`${GET_REPLIES_URL}${comment.id.value}`);
      setReplies(response.data);
    } catch (error) {
      console.error("Failed to fetch replies:", error);
    }
  };

  useEffect(() => {
    fetchReplies();
  }, [comment.id.value]);

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
