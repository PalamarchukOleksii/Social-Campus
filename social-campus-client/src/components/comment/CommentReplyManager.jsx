import React, { useState } from "react";
import CreateComment from "../createComment/CreateComment";
import Comment from "../comment/Comment";
import PropTypes from "prop-types";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";

function CommentReplyManager({ comment, currentUser, comments, setComments }) {
  const [replies, setReplies] = useState(comment?.replies || []);
  const [isReplying, setIsReplying] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [editText, setEditText] = useState(comment.text);

  const handleReplySubmit = (newReplyText) => {
    const newReply = {
      id: getMaxCommentId() + 1,
      text: newReplyText,
      creationTime: new Date().toISOString(),
      username: currentUser.username,
      login: currentUser.login,
      profileImage: currentUser.profileImage,
      likeCount: 0,
    };

    setReplies((prevReplies) => [...prevReplies, newReply]);
    setIsReplying(false);
  };

  const handleEditSubmit = (newText) => {
    comment.text = newText;
    setEditText(newText);
    setIsEditing(false);
  };

  return (
    <div className="comment-reply-manager">
      <Comment
        {...comment}
        onReplyClick={() => setIsReplying(true)}
        onEditClick={() => setIsEditing(true)}
        currentUser={currentUser}
        replies={replies}
      />

      {isReplying && (
        <div className="create-comment-modal-overlay">
          <CreateComment
            user={currentUser}
            onSubmit={handleReplySubmit}
            onCloseClick ={() => setIsReplying(false)}
            addGoBack
            getMaxCommentId={getMaxCommentId}
            comments={comments}
            setComments={setComments}
          />
        </div>
      )}

      {isEditing && (
        <CreateComment
          user={currentUser}
          text={editText}
          isForEdit={true}
          onSubmit={handleEditSubmit}
          onClose={() => setIsEditing(false)}
        />
      )}

      {!!replies?.length && (
        <div className="replies-section">
          {replies.map((reply) => (
            <CommentReplyManager
              key={reply.id}
              comment={reply}
              currentUser={currentUser}
            />
          ))}
        </div>
      )}
    </div>
  );
}

CommentReplyManager.propTypes = {
  comment: PropTypes.shape({
    id: PropTypes.number.isRequired,
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
    profileImage: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    likeCount: PropTypes.number.isRequired,
    creationTime: PropTypes.string.isRequired,
    replies: PropTypes.array,
  }).isRequired,
  currentUser: PropTypes.object.isRequired,
};

export default CommentReplyManager;
