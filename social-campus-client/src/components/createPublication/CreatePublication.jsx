import React, { useState } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import {
  IoImageOutline,
  IoImage,
  IoCloseOutline,
  IoClose,
  IoArrowBackCircleOutline,
  IoArrowBackCircle,
} from "react-icons/io5";
import "./CreatePublication.css";

function CreatePublication(props) {
  const [publicationText, setPublicationText] = useState("");
  const [image, setImage] = useState(null);
  const [imagePreview, setImagePreview] = useState(null);
  const [isHovered, setIsHovered] = useState(false);
  const [isCloseHovered, setIsCloseHovered] = useState(false);
  const [isExitHovered, setIsExitHovered] = useState(false);

  const handleInputChange = (e) => {
    setPublicationText(e.target.value);
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (publicationText.trim()) {
      const newPublication = {
        id: props.getMaxPublicationId() + 1,
        description: publicationText,
        imageUrl: image ? URL.createObjectURL(image) : null,
        creationTime: new Date().toISOString(),
        likesCount: 0,
        comments: [],
        username: props.user.username,
        login: props.user.login,
        profileImage: props.user.profileImage,
      };

      props.setPublications([newPublication, ...props.publications]);

      setPublicationText("");
      setImage(null);
      setImagePreview(null);
      props.close();
      window.scrollTo(0, 0);
    } else {
      toast("Publication text cannot be empty.");
    }
  };

  const removeImage = () => {
    setImage(null);
    setImagePreview(null);
  };

  const closeCreatePublication = () => {
    setPublicationText("");
    setImage(null);
    setImagePreview(null);
    props.close();
  };

  return (
    <div className="create-publication">
      <div
        className="close-creation-icon general-text"
        onMouseEnter={() => setIsExitHovered(true)}
        onMouseLeave={() => setIsExitHovered(false)}
        onClick={closeCreatePublication}
      >
        {isExitHovered ? <IoArrowBackCircle /> : <IoArrowBackCircleOutline />}
        <span className="general-text back-text">Back</span>
      </div>
      <ShortProfile
        username={props.user.username}
        login={props.user.login}
        profileImage={props.user.profileImage}
        redirectOnClick={false}
      />
      <form className="create-form" onSubmit={handleSubmit}>
        <textarea
          className="publication-text"
          type="text"
          placeholder="Your publication text..."
          value={publicationText}
          onChange={handleInputChange}
          onInput={(e) => {
            if (e.target.scrollHeight > e.target.clientHeight) {
              e.target.style.height = "auto";
              e.target.style.height = `${e.target.scrollHeight}px`;
            }
          }}
          required
          autoFocus
        />
        {imagePreview && (
          <div className="image-preview-container">
            <img src={imagePreview} alt="Preview" className="image-preview" />
            <div
              className="remove-image-icon general-text"
              onMouseEnter={() => setIsCloseHovered(true)}
              onMouseLeave={() => setIsCloseHovered(false)}
              onClick={removeImage}
            >
              {isCloseHovered ? <IoClose /> : <IoCloseOutline />}
            </div>
          </div>
        )}
        <div className="controles">
          <label
            className="image-upload-icon general-text"
            htmlFor="image-upload"
            onMouseEnter={() => setIsHovered(true)}
            onMouseLeave={() => setIsHovered(false)}
          >
            {isHovered ? <IoImage /> : <IoImageOutline />}
          </label>
          <input
            id="image-upload"
            type="file"
            accept="image/*"
            onChange={handleImageChange}
            style={{ display: "none" }}
          />
          <button className="publish-button" type="submit">
            Publish
          </button>
        </div>
      </form>
    </div>
  );
}

CreatePublication.propTypes = {
  user: PropTypes.shape({
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
    profileImage: PropTypes.string,
  }).isRequired,
  publications: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      description: PropTypes.string,
      imageUrl: PropTypes.string,
      creationTime: PropTypes.string,
      likesCount: PropTypes.number,
      comments: PropTypes.arrayOf(PropTypes.object).isRequired,
      username: PropTypes.string,
      login: PropTypes.string,
      profileImage: PropTypes.string,
    })
  ).isRequired,
  setPublications: PropTypes.func.isRequired,
  getMaxPublicationId: PropTypes.func.isRequired,
  close: PropTypes.func.isRequired,
};

export default CreatePublication;
