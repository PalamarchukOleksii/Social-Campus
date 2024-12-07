import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItem from "../interactionItem/InteractionItem";
import InteractionItems from "../../utils/consts/InteractionItems";
import "./Publication.css";
import DateTime from "../dateTime/DateTime";
import CreateComment from "../createComment/CreateComment";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";
import userData from "../../data/userData.json";
import login from "../../utils/consts/AuthUserLogin";

function Publication(props) {
  const navigate = useNavigate();
  const location = useLocation();
  const [comments, setComments] = useState([]);
  const [currentUser, setCurrentUser] = useState(null);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [description, setDescription] = useState(props.publication.description);
  const [imageUrl, setImageUrl] = useState(props.publication.imageUrl);

  useEffect(() => {
    const user = userData.find((user) => user.login === login);
    setCurrentUser(user);
    setComments(props.publication.comments);
  }, [props.publication.comments]);

  const handleCreateCommentOpenClick = () => {
    setIsCreateOpen(true);
  };

  const handleCreateCommentCloseClick = () => {
    setIsCreateOpen(false);
  };

  const handleEditClick = () => {
    setIsEditing(true);
  };

  const handleSaveClick = () => {
    setIsEditing(false);
    console.log("Updated Description:", description);
    console.log("Updated Image URL:", imageUrl);
  };

  const handleCancelClick = () => {
    setIsEditing(false);
    setDescription(props.publication.description);
    setImageUrl(props.publication.imageUrl);
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImageUrl(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const canEdit = currentUser && currentUser.login === props.publication.login;

  return (
    <div className="publication-container">
      {isCreateOpen && (
        <div className="create-comment-modal-overlay">
          <CreateComment
            user={currentUser}
            comments={comments}
            setComments={setComments}
            getMaxCommentId={getMaxCommentId}
            onCloseClick={handleCreateCommentCloseClick}
            addGoBack={true}
          />
        </div>
      )}
      <div>
        <div className="short-info-container">
          <ShortProfile
            username={props.publication.username}
            login={props.publication.login}
            profileImage={props.publication.profileImage}
          />
          <DateTime dateTime={props.publication.creationTime} locale="en-US" />
        </div>
        <div className="content-container">
          {isEditing ? (
            <>
              <textarea
                className="edit-description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
              />
              <div className="image-upload">
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                />
              </div>
            </>
          ) : (
            <>
              <h2 className="description general-text">{description || "Description"}</h2>
              {imageUrl && <img src={imageUrl} alt="Publication" />}
            </>
          )}
        </div>
        <div className="interaction-stat">
          <InteractionItem
            itemType="like"
            label={props.publication.likesCount}
            icon={InteractionItems.likeIcon}
            activeIcon={InteractionItems.activeLikeIcon}
          />
          <InteractionItem
            itemType="comment"
            label={props.publication.comments.length}
            icon={InteractionItems.commentIcon}
            onClick={handleCreateCommentOpenClick}
          />
        </div>
        {canEdit && isEditing && (
          <div className="edit-buttons">
            <button className="save-btn" onClick={handleSaveClick}>
              Save
            </button>
            <button className="cancel-btn" onClick={handleCancelClick}>
              Cancel
            </button>
          </div>
        )}
        {canEdit && !isEditing && (
          <button className="edit-btn" onClick={handleEditClick}>
            Edit
          </button>
        )}
      </div>
    </div>
  );
}

Publication.propTypes = {
  publication: PropTypes.shape({
    id: PropTypes.number.isRequired,
    description: PropTypes.string.isRequired,
    imageUrl: PropTypes.string,
    creationTime: PropTypes.string.isRequired,
    likesCount: PropTypes.number.isRequired,
    comments: PropTypes.array.isRequired,
    profileImage: PropTypes.string,
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
  }).isRequired,
};

export default Publication;

